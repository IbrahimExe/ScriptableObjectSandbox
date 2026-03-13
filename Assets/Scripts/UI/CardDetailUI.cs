using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDetailUI : MonoBehaviour
{
    [Header("References")]
    public GameObject largeCardPrefab;
    public Transform cardSpawnParent;
    public Button backButton;
    public Image background;
    public TextMeshProUGUI typeText;

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.LoadScene("Card"));

        CardData selected = GameManager.SelectedCard;

        if (selected != null)
        {
            GameObject go = Instantiate(largeCardPrefab, cardSpawnParent);

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            CardDisplay display = go.GetComponent<CardDisplay>();

            if (display != null)
            {
                display.Initialize(selected);
                typeText.text = $"Type: {selected.type}";
                StartCoroutine(display.PlayFlipAnimation());
            }
            else
            {
                Debug.LogError("[CardDetail] CardDisplay component missing from largeCardPrefab root!");
            }
        }
        else
        {
            Debug.LogWarning("[CardDetail] No card selected — GameManager.SelectedCard is null!");
        }

        ApplyTheme();

        if (ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged += ApplyTheme;
    }

    private void OnDestroy()
    {
        if (ThemeManager.Instance != null)
            ThemeManager.Instance.OnThemeChanged -= ApplyTheme;
    }

    private void ApplyTheme()
    {
        if (ThemeManager.Instance?.activeTheme == null) return;
        background.color = ThemeManager.Instance.activeTheme.backgroundColor;
        typeText.font = ThemeManager.Instance.activeTheme.regularFont;
        typeText.color = ThemeManager.Instance.activeTheme.regularFontColor;
    }
}