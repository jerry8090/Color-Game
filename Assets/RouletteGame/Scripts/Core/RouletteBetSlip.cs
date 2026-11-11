using System;
using System.Collections.Generic;

namespace RouletteGame.Core
{
    /// <summary>
    /// The bets on the table before the spin. It NEVER touches the wallet: the settler charges once at spin time.
    /// Free credits = balance minus the slip total, so the player can never bet more than they own.
    /// Every change can be undone. After CONFIRM the slip is locked and nothing can change it.
    /// </summary>
    public sealed class RouletteBetSlip
    {
        public const string InsufficientCreditsMessage = "INSUFFICIENT CREDITS";
        public const string ClosedMessage = "BETTING IS CLOSED";
        private const int MaxUndoDepth = 200;

        private readonly RouletteLimits limits;
        private readonly List<RouletteBet> bets = new List<RouletteBet>();
        private readonly List<RouletteBet[]> history = new List<RouletteBet[]>();
        private RouletteBet[] lastRound = new RouletteBet[0];

        /// <summary>Raised after any change (bets, lock state or last-round memory).</summary>
        public event Action Changed;

        public bool IsLocked { get; private set; }
        public IReadOnlyList<RouletteBet> Bets => bets;
        public bool HasBets => bets.Count > 0;
        public bool CanUndo => !IsLocked && history.Count > 0;
        public bool HasLastRound => lastRound.Length > 0;

        public long TotalBet
        {
            get
            {
                long total = 0;
                foreach (RouletteBet bet in bets) total += bet.Amount;
                return total;
            }
        }

        public RouletteBetSlip(RouletteLimits limits = null)
        {
            this.limits = limits ?? RouletteLimits.Default;
        }

        public long GetAmount(string areaKey)
        {
            int index = IndexOf(areaKey);
            return index < 0 ? 0 : bets[index].Amount;
        }

        /// <summary>A copy of the bets, safe to hand to the settler.</summary>
        public RouletteBet[] Snapshot()
        {
            return bets.ToArray();
        }

        public long MaxPossibleReturn()
        {
            return RoulettePayout.MaxPossibleReturn(bets);
        }

        /// <summary>Puts one chip on an area (or adds to the chips already there). 'area' only says WHERE; its own amount is ignored.</summary>
        public bool TryPlaceChip(RouletteBet area, long chip, long balance, out string error)
        {
            error = null;
            if (IsLocked) { error = ClosedMessage; return false; }
            if (area == null || chip <= 0) { error = "INVALID BET"; return false; }

            int index = IndexOf(area.Key);
            long current = index < 0 ? 0 : bets[index].Amount;
            long total = TotalBet;

            if (current > limits.MaxBetPerArea - chip) { error = "BET LIMIT FOR THIS AREA"; return false; }
            if (total > limits.MaxTotalBet - chip) { error = "TABLE LIMIT REACHED"; return false; }
            if (total + chip > balance) { error = InsufficientCreditsMessage; return false; }

            PushHistory();
            RouletteBet updated = area.WithAmount(current + chip);
            if (index < 0) bets.Add(updated); else bets[index] = updated;
            RaiseChanged();
            return true;
        }

        /// <summary>Takes one chip off an area. The last chip removes the whole bet.</summary>
        public bool TryRemoveChip(string areaKey, long chip, out string error)
        {
            error = null;
            if (IsLocked) { error = ClosedMessage; return false; }

            int index = IndexOf(areaKey);
            if (index < 0 || chip <= 0) { error = "NO BET THERE"; return false; }

            PushHistory();
            long left = bets[index].Amount - chip;
            if (left <= 0) bets.RemoveAt(index); else bets[index] = bets[index].WithAmount(left);
            RaiseChanged();
            return true;
        }

        /// <summary>Removes the whole bet on one area.</summary>
        public bool TryRemoveArea(string areaKey, out string error)
        {
            error = null;
            if (IsLocked) { error = ClosedMessage; return false; }

            int index = IndexOf(areaKey);
            if (index < 0) { error = "NO BET THERE"; return false; }

            PushHistory();
            bets.RemoveAt(index);
            RaiseChanged();
            return true;
        }

        public bool Clear()
        {
            if (IsLocked || bets.Count == 0) return false;

            PushHistory();
            bets.Clear();
            RaiseChanged();
            return true;
        }

        /// <summary>Takes back the last betting action (place, remove, clear or repeat).</summary>
        public bool Undo()
        {
            if (IsLocked || history.Count == 0) return false;

            RouletteBet[] previous = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            bets.Clear();
            bets.AddRange(previous);
            RaiseChanged();
            return true;
        }

        /// <summary>Replaces the table with the bets of the previous round, if the player can afford them.</summary>
        public bool TryRepeatLast(long balance, out string error)
        {
            error = null;
            if (IsLocked) { error = ClosedMessage; return false; }
            if (lastRound.Length == 0) { error = "NO PREVIOUS BET"; return false; }

            long total = 0;
            foreach (RouletteBet bet in lastRound) total += bet.Amount;
            if (total > balance) { error = InsufficientCreditsMessage; return false; }
            if (total > limits.MaxTotalBet) { error = "TABLE LIMIT REACHED"; return false; }

            PushHistory();
            bets.Clear();
            bets.AddRange(lastRound);
            RaiseChanged();
            return true;
        }

        /// <summary>CONFIRM: locks the table. Nothing is charged yet.</summary>
        public bool TryConfirm(out string error)
        {
            error = null;
            if (IsLocked) { error = ClosedMessage; return false; }
            if (bets.Count == 0) { error = "PLACE A BET FIRST"; return false; }

            IsLocked = true;
            RaiseChanged();
            return true;
        }

        /// <summary>Lets the player edit again (only the controller calls this, and never after the spin has started).</summary>
        public void Unlock()
        {
            if (!IsLocked) return;
            IsLocked = false;
            RaiseChanged();
        }

        /// <summary>The round is over: remember these bets for REPEAT, clear the table and open it again.</summary>
        public void FinishRound()
        {
            if (bets.Count > 0) lastRound = bets.ToArray();
            bets.Clear();
            history.Clear();
            IsLocked = false;
            RaiseChanged();
        }

        private int IndexOf(string key)
        {
            for (int i = 0; i < bets.Count; i++)
            {
                if (bets[i].Key == key) return i;
            }
            return -1;
        }

        private void PushHistory()
        {
            history.Add(bets.ToArray());
            if (history.Count > MaxUndoDepth) history.RemoveAt(0);
        }

        private void RaiseChanged()
        {
            Changed?.Invoke();
        }
    }
}
