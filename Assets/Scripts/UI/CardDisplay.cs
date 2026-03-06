using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Image cardFrameImage;
    public Image cardImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI descriptionText;
    public Button cardButton;

    private CardData _data;

    private void OnEnable()
    {
        // update loc settings when language changes
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged += ApplyLocalization;
        if (ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged += ApplyTheme;
    }

    private void OnDisable()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= ApplyLocalization;
        if (ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged -= ApplyTheme;
    }

    public void Initialize(CardData data, System.Action<CardData> onClickCallback = null)
    {
        _data = data;
        ApplyData();
        ApplyLocalization();
        ApplyTheme();

        if (onClickCallback != null)
            cardButton.onClick.AddListener(() => onClickCallback(data));
    }

    private void ApplyData()
    {
        if (_data == null) return;
        cardImage.sprite = _data.cardImage;
        costText.text = _data.cost.ToString();
        attackText.text = $"⚔ {_data.attack}";
        defenseText.text = $"🛡 {_data.defense}";
    }

    private void ApplyLocalization()
    {
        if (_data == null || LocalizationManager.Instance == null) return;
        nameText.text = LocalizationManager.Instance.Get(_data.nameLocKey);
        descriptionText.text = LocalizationManager.Instance.Get(_data.descriptionLocKey);
    }

    private void ApplyTheme()
    {
        if (ThemeManager.Instance?.activeTheme == null) return;
        ThemeData theme = ThemeManager.Instance.activeTheme;

        // Card frame color by type
        cardFrameImage.color = theme.GetCardFrameColor(_data.type);

        // Fonts & colors
        nameText.font = theme.specialFont;
        nameText.color = theme.specialFontColor;
        nameText.fontStyle = theme.specialFontStyle;

        descriptionText.font = theme.regularFont;
        descriptionText.color = theme.regularFontColor;

        costText.font = theme.regularFont;
        costText.color = theme.regularFontColor;
        attackText.font = theme.regularFont;
        defenseText.font = theme.regularFont;
    }

   // for flip animation
    public IEnumerator PlayFlipAnimation()
    {
        float duration = 0.15f;
        float elapsed = 0f;
        Vector3 original = transform.localScale;

        // Squish to zero on X
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = new Vector3(Mathf.Lerp(1f, 0f, t), 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Expand back
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = new Vector3(Mathf.Lerp(0f, 1f, t), 1f, 1f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = original;
    }
}