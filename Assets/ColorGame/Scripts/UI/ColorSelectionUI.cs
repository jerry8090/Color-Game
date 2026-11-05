using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Creates the color betting areas from the config. A tap puts one chip of the selected amount on that color
    /// (BetSlip decides if it is allowed). Bills and totals are drawn from the slip's events.
    /// </summary>
    public class ColorSelectionUI : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private BetSlip betSlip;
        [SerializeField] private ColorButtonView buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TMP_Text selectedColorLabel;

        private readonly List<ColorButtonView> views = new List<ColorButtonView>();
        private bool isReady;

        private void Awake()
        {
            if (config == null || gameManager == null || betSlip == null || buttonPrefab == null ||
                buttonContainer == null || selectedColorLabel == null)
            {
                Debug.LogError("[ColorSelectionUI] A reference is missing in the Inspector (Bet Slip included).", this);
                return;
            }

            selectedColorLabel.enableAutoSizing = true;
            selectedColorLabel.fontSizeMin = 18f;
            selectedColorLabel.fontSizeMax = 40f;

            BuildButtons();
            isReady = views.Count > 0;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnStateChanged += HandleStateChanged;
            betSlip.OnChipPlaced += HandleChipPlaced;
            betSlip.OnSlipCleared += HandleSlipCleared;
            SyncFromSlip();
            Refresh();
        }

        private void OnDisable()
        {
            if (gameManager != null) gameManager.OnStateChanged -= HandleStateChanged;
            if (betSlip != null)
            {
                betSlip.OnChipPlaced -= HandleChipPlaced;
                betSlip.OnSlipCleared -= HandleSlipCleared;
            }
        }

        private void BuildButtons()
        {
            if (config.Colors.Count == 0)
            {
                Debug.LogError("[ColorSelectionUI] The config has no color definitions.", config);
                return;
            }

            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition == null) continue;

                ColorButtonView view = Instantiate(buttonPrefab, buttonContainer);
                view.name = "ColorButton_" + definition.displayName;
                view.Setup(definition);
                view.OnClicked += HandleColorClicked;
                views.Add(view);
            }
        }

        private void HandleColorClicked(GameColor color)
        {
            // BetSlip validates (chip selected, balance, betting open). The manager only remembers the last tap.
            if (betSlip.TryPlaceChip(color)) gameManager.TrySelectColor(color);
        }

        private void HandleChipPlaced(GameColor color, int denomination, int colorTotal)
        {
            ColorButtonView view = Find(color);
            if (view == null) return;

            view.AddBill(denomination, colorTotal);
            view.PlayTapFeedback(); // a short flash only: no selected state
            UpdateLabel();
        }

        private void HandleSlipCleared()
        {
            foreach (ColorButtonView view in views) view.ClearBills();
            UpdateLabel();
        }

        private void HandleStateChanged(GameState state)
        {
            Refresh();
        }

        // Rebuilds the bills from the slip (for example when this object is enabled again).
        private void SyncFromSlip()
        {
            foreach (ColorButtonView view in views)
            {
                int total = betSlip.GetTotal(view.AssignedColor);
                if (total > 0) view.SetBills(betSlip.GetChips(view.AssignedColor), total);
                else view.ClearBills();
            }
            UpdateLabel();
        }

        private void Refresh()
        {
            bool canInteract = gameManager.State == GameState.Idle;
            foreach (ColorButtonView view in views) view.SetInteractable(canInteract);
            UpdateLabel();
        }

        private ColorButtonView Find(GameColor color)
        {
            foreach (ColorButtonView view in views)
            {
                if (view.AssignedColor == color) return view;
            }
            return null;
        }

        // No color names here: only how many colors carry a bet.
        private void UpdateLabel()
        {
            int colorsWithBets = 0;
            foreach (ColorDefinition definition in config.Colors)
            {
                if (definition != null && betSlip.GetTotal(definition.color) > 0) colorsWithBets++;
            }

            selectedColorLabel.text = colorsWithBets == 0
                ? "PLACE YOUR BETS"
                : $"BETS ON {colorsWithBets} {(colorsWithBets == 1 ? "COLOR" : "COLORS")}";
        }
    }
}