using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LocEntry;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public Button playButton;
    public Button showCardsButton;
    public Button quitButton;
    public Image backgroundImage;

    [Header("Button Texts")]
    public TextMeshProUGUI playButtonText;
    public TextMeshProUGUI showCardsButtonText;
    public TextMeshProUGUI quitButtonText;

    [Header("Language Buttons (optional)")]
    public Button btnEnglish;
    public Button btnFrench;
    public Button btnSpanish;

    private void Start()
    {
        playButton.onClick.AddListener(() => GameManager.Instance.LoadScene("PlayScreen"));
        showCardsButton.onClick.AddListener(() => GameManager.Instance.LoadScene("ShowCards"));
        quitButton.onClick.AddListener(() => GameManager.Instance.Quit());

        btnEnglish?.onClick.AddListener(() => LocalizationManager.Instance.SetLanguage(GameLanguage.English));
        btnFrench?.onClick.AddListener(() => LocalizationManager.Instance.SetLanguage(GameLanguage.French));
        btnSpanish?.onClick.AddListener(() => LocalizationManager.Instance.SetLanguage(GameLanguage.Spanish));

        ApplyTheme();
        ApplyLocalization();

        ThemeManager.Instance.OnThemeChanged += ApplyTheme;
        LocalizationManager.Instance.OnLanguageChanged += ApplyLocalization;
    }

    private void OnDestroy()
    {
        ThemeManager.Instance.OnThemeChanged -= ApplyTheme;
        LocalizationManager.Instance.OnLanguageChanged -= ApplyLocalization;
    }

    private void ApplyTheme()
    {
        ThemeData t = ThemeManager.Instance.activeTheme;
        backgroundImage.color = t.backgroundColor;

        titleText.font = t.specialFont;
        titleText.color = t.specialFontColor;
        titleText.fontStyle = t.specialFontStyle;

        ApplyButtonTheme(playButton, playButtonText, t);
        ApplyButtonTheme(showCardsButton, showCardsButtonText, t);
        ApplyButtonTheme(quitButton, quitButtonText, t);
    }

    private void ApplyButtonTheme(Button btn, TextMeshProUGUI label, ThemeData t)
    {
        btn.image.sprite = t.regularButtonSprite;
        label.font = t.regularFont;
        label.color = t.regularFontColor;
    }

    private void ApplyLocalization()
    {
        playButtonText.text = LocalizationManager.Instance.Get("UI_PLAY");
        showCardsButtonText.text = LocalizationManager.Instance.Get("UI_SHOWCARDS");
        quitButtonText.text = LocalizationManager.Instance.Get("UI_QUIT");
    }
}