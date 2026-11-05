
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonSpriteToggleUI : MonoBehaviour
{
    [Header("Button References")]
    [SerializeField] private Button button;
    [SerializeField] private Image targetImage;

    [Header("Sprites")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    [Header("Initial State")]
    [SerializeField] private bool isOn = true;

    [Header("Events")]
    public UnityEvent<bool> onStateChanged;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button == null || targetImage == null ||
            onSprite == null || offSprite == null)
        {
            Debug.LogError(
                "[ButtonSpriteToggleUI] Missing Inspector references.",
                this
            );
            enabled = false;
            return;
        }

        button.onClick.AddListener(Toggle);
        Refresh();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(Toggle);
    }

    public void Toggle()
{
    Debug.Log("AUTO BUTTON CLICKED!");

    isOn = !isOn;

    Debug.Log("New Auto State: " + (isOn ? "ON" : "OFF"));

    Refresh();
}

    public void SetState(bool state)
    {
        if (isOn == state)
        {
            Refresh();
            return;
        }

        isOn = state;
        Refresh();
        onStateChanged?.Invoke(isOn);
    }

   private void Refresh()
{
    if (targetImage == null)
    {
        Debug.LogError("Target Image is missing!");
        return;
    }

    targetImage.sprite = isOn ? onSprite : offSprite;

    Debug.Log("Displaying Sprite: " + targetImage.sprite.name);
}

    public bool IsOn => isOn;
}
