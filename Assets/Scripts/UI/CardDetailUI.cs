using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDetailUI : MonoBehaviour
{
    [Header("References")]
    public CardDisplay cardDisplay;     
    public Button backButton;
    public Image background;
    public TextMeshProUGUI typeText;

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.LoadScene("ShowCards"));

        CardData selected = GameManager.SelectedCard;

        if (selected != null)
        {
            cardDisplay.Initialize(selected);
            typeText.text = $"Type: {selected.type}";
            StartCoroutine(cardDisplay.PlayFlipAnimation());
        }
        else
        {
            Debug.LogWarning("[CardDetailUI] No card selected!");
        }

        ApplyTheme();
        ThemeManager.Instance.OnThemeChanged += ApplyTheme;
    }

    private void OnDestroy() =>
        ThemeManager.Instance.OnThemeChanged -= ApplyTheme;

    private void ApplyTheme()
    {
        background.color = ThemeManager.Instance.activeTheme.backgroundColor;
        typeText.font = ThemeManager.Instance.activeTheme.regularFont;
        typeText.color = ThemeManager.Instance.activeTheme.regularFontColor;
    }
}