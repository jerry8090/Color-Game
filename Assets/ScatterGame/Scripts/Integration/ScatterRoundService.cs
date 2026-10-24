using System;
using UnityEngine;
using Shared.Save;
using Shared.Wallet;
using ScatterGame.Core;
using ScatterGame.Runtime;

namespace ScatterGame.Integration
{
    /// <summary>
    /// The one object the Scatter UI calls to play a paid round.
    /// It checks the bet, plays the whole round (paid spin plus every free spin) through RoundSettler,
    /// records it in the history, and saves. The UI only has to animate the returned result.
    /// The wallet is already settled when this returns, so closing the app mid-animation can never lose or duplicate credits.
    /// </summary>
    public class ScatterRoundService : MonoBehaviour
    {
        [SerializeField] private ScatterGameSettings settings;
        [SerializeField] private VirtualWalletManager walletManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private ScatterHistoryManager historyManager;

        /// <summary>Raised after a round was settled, recorded and saved.</summary>
        public event Action<SettlementResult, ScatterRoundRecord> OnRoundSettled;

        private RoundSettler settler;
        private ISymbolSource source;

        public bool IsReady { get; private set; }
        public ScatterGameSettings Settings => settings;

        private void Awake()
        {
            if (settings == null || walletManager == null || saveManager == null || historyManager == null)
            {
                Debug.LogError("[ScatterRoundService] A reference is missing in the Inspector.", this);
                return;
            }

            var problems = settings.GetProblems();
            if (problems.Count > 0)
            {
                Debug.LogError("[ScatterRoundService] The settings asset has problems: " + string.Join(" | ", problems), this);
                return;
            }

            SlotConfig config = settings.BuildSlotConfig();
            settler = new RoundSettler(config, new UnityScatterWallet(walletManager, saveManager), settings.BetAmounts);
            source = new WeightedSymbolSource(config, new System.Random()); // one stream, time-seeded
            IsReady = true;
        }

        /// <summary>Quick check the UI can use to enable the SPIN button or show a message.</summary>
        public bool CanPlay(int bet, out string reason)
        {
            reason = null;
            if (!IsReady) { reason = "The game is not set up correctly."; return false; }
            if (!saveManager.IsReady) { reason = "The save file is still loading."; return false; }
            if (!settler.IsBetAllowed(bet)) { reason = "That bet amount is not allowed."; return false; }
            if (!walletManager.CanAfford(bet)) { reason = "INSUFFICIENT CREDITS"; return false; }
            return true;
        }

        /// <summary>Plays one whole paid round. Returns false (and charges nothing) if the round could not be played.</summary>
        public bool TryPlayRound(int bet, out SettlementResult result)
        {
            result = null;

            if (!CanPlay(bet, out string reason))
            {
                Debug.LogWarning("[ScatterRoundService] Round not started: " + reason);
                return false;
            }

            string roundId = historyManager.ReserveNextRoundId();
            result = settler.Play(bet, roundId, source);

            if (!result.Success)
            {
                Debug.LogError($"[ScatterRoundService] {roundId} failed: {result.Status}. Owed credits: {result.OwedCredits}.", this);
                return false;
            }

            ScatterRoundRecord record = ScatterHistoryService.FromSettlement(result, DateTime.UtcNow);
            historyManager.AddRound(record);
            saveManager.SaveGame();

            OnRoundSettled?.Invoke(result, record);
            return true;
        }

        // Right-click the component header in Play mode.
        [ContextMenu("TEST: Play One Round (lowest bet)")]
        private void TestPlayOneRound()
        {
            if (!IsReady) { Debug.LogWarning("[ScatterRoundService] Press Play first."); return; }

            int bet = settings.BetAmounts[0];
            if (!TryPlayRound(bet, out SettlementResult r)) return;

            Debug.Log($"[ScatterRoundService] {r.RoundId} | bet {r.Bet} | won {r.CreditsPaid} | " +
                      $"balance {r.BalanceBefore} -> {r.BalanceAfter} | cascades {r.Round.TotalCascades} | " +
                      $"max x{r.Round.MaxMultiplier} | free spins {r.Round.FreeSpins.Count}");
        }
    }
}