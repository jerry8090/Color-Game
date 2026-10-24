using System;
using System.Collections.Generic;
using UnityEngine;
using Shared.Wallet;
using ColorGame.Core;

namespace ColorGame.Gameplay
{
    /// <summary>
    /// The bets of the current round: chips per color. It never touches the wallet.
    /// ColorGameManager spends the total ONCE when the round starts, then clears the slip.
    /// The selected chip amount comes from BetManager.
    /// </summary>
    public class BetSlip : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private BetManager betManager;
        [SerializeField] private VirtualWalletManager walletManager;
        [Tooltip("Safety limit of bills on one color.")]
        [SerializeField] private int maxChipsPerColor = 99;

        /// <summary>Color, chip amount placed, new total on that color.</summary>
        public event Action<GameColor, int, int> OnChipPlaced;
        public event Action<int> OnTotalChanged;
        public event Action OnSlipCleared;

        private readonly Dictionary<GameColor, List<int>> chips = new Dictionary<GameColor, List<int>>();
        private readonly Dictionary<GameColor, int> totals = new Dictionary<GameColor, int>();
        private int totalBet;
        private bool isReady;

        public int TotalBet => totalBet;
        public bool HasBets => totalBet > 0;

        private void Awake()
        {
            if (config == null || betManager == null || walletManager == null || config.Colors.Count == 0)
            {
                Debug.LogError("[BetSlip] Config, Bet Manager or Wallet Manager is missing in the Inspector.", this);
                return;
            }

            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition == null || chips.ContainsKey(definition.color)) continue;
                chips[definition.color] = new List<int>();
                totals[definition.color] = 0;
            }

            isReady = true;
        }

        public int GetTotal(GameColor color)
        {
            return totals.TryGetValue(color, out int total) ? total : 0;
        }

        public IReadOnlyList<int> GetChips(GameColor color)
        {
            return chips.TryGetValue(color, out List<int> list) ? list : (IReadOnlyList<int>)new List<int>();
        }

        /// <summary>Copy of the bets (config color order), for the manager.</summary>
        public List<ColorBetEntry> GetBets()
        {
            List<ColorBetEntry> bets = new List<ColorBetEntry>();
            if (!isReady) return bets;

            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition == null) continue;
                int total = GetTotal(definition.color);
                if (total > 0) bets.Add(new ColorBetEntry { color = definition.color, amount = total });
            }
            return bets;
        }

        /// <summary>Puts one chip of the selected amount on a color. Returns true if accepted.</summary>
        public bool TryPlaceChip(GameColor color)
        {
            if (!isReady) return false;

            if (betManager.IsLocked)
            {
                Debug.LogWarning("[BetSlip] Betting is closed.");
                return false;
            }

            if (!chips.TryGetValue(color, out List<int> list))
            {
                Debug.LogError($"[BetSlip] {color} is not one of the configured colors.", this);
                return false;
            }

            if (!betManager.HasBet)
            {
                betManager.Reject("SELECT A BET AMOUNT FIRST");
                return false;
            }

            int amount = betManager.SelectedBet;
            if (amount <= 0 || !config.IsValidBetAmount(amount))
            {
                Debug.LogError($"[BetSlip] Invalid chip amount: {amount}", this);
                return false;
            }

            if (list.Count >= maxChipsPerColor)
            {
                betManager.Reject("TOO MANY BILLS ON THIS COLOR");
                return false;
            }

            // Available credits = balance minus what is already on the table.
            if (walletManager.GetBalance() - totalBet < amount)
            {
                betManager.Reject(BetManager.InsufficientCreditsMessage);
                return false;
            }

            list.Add(amount);
            totals[color] += amount;
            totalBet += amount;

            OnChipPlaced?.Invoke(color, amount, totals[color]);
            OnTotalChanged?.Invoke(totalBet);
            return true;
        }

        /// <summary>Removes every bet. Called by ColorGameManager at the end of a round.</summary>
        public void Clear()
        {
            if (!isReady) return;

            foreach (List<int> list in chips.Values) list.Clear();
            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition != null) totals[definition.color] = 0;
            }
            totalBet = 0;

            OnSlipCleared?.Invoke();
            OnTotalChanged?.Invoke(0);
        }
    }
}