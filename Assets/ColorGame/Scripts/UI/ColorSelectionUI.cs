using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ColorGame.Core;
using ColorGame.Gameplay;

namespace ColorGame.UI
{
    /// <summary>
    /// Creates the color buttons from the config and shows which one is selected.
    /// Clicks are forwarded to ColorGameManager. This script never decides game outcomes.
    /// </summary>
    public class ColorSelectionUI : MonoBehaviour
    {
        [SerializeField] private ColorGameConfig config;
        [SerializeField] private ColorGameManager gameManager;
        [SerializeField] private ColorButtonView buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private TMP_Text selectedColorLabel;

        private readonly List<ColorButtonView> views = new List<ColorButtonView>();
        private bool isReady;

        private void Awake()
        {
            if (config == null || gameManager == null || buttonPrefab == null ||
                buttonContainer == null || selectedColorLabel == null)
            {
                Debug.LogError("[ColorSelectionUI] A reference is missing in the Inspector.", this);
                return;
            }

            BuildButtons();
            isReady = views.Count > 0;
        }

        private void OnEnable()
        {
            if (!isReady) return;

            gameManager.OnSelectedColorChanged += HandleSelectedColorChanged;
            gameManager.OnStateChanged += HandleStateChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if (gameManager == null) return;
            gameManager.OnSelectedColorChanged -= HandleSelectedColorChanged;
            gameManager.OnStateChanged -= HandleStateChanged;
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
            gameManager.TrySelectColor(color);
        }

        private void HandleSelectedColorChanged(bool hasColor, GameColor color)
        {
            Refresh();
        }

        private void HandleStateChanged(GameState state)
        {
            Refresh();
        }

        private void Refresh()
        {
            bool canInteract = gameManager.State == GameState.Idle;

            foreach (ColorButtonView view in views)
            {
                bool isSelected = gameManager.HasSelectedColor && view.AssignedColor == gameManager.SelectedColor;
                view.SetSelected(isSelected);
                view.SetInteractable(canInteract);
            }

            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (gameManager.HasSelectedColor &&
                config.TryGetColorDefinition(gameManager.SelectedColor, out ColorDefinition definition))
            {
                string hex = ColorUtility.ToHtmlStringRGB(definition.uiColor);
                selectedColorLabel.text = $"SELECTED COLOR: <color=#{hex}>{definition.displayName}</color>";
            }
            else
            {
                selectedColorLabel.text = "SELECTED COLOR: NONE";
            }
        }
    }
}