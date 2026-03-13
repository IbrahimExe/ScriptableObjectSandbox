using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShowCardsUI : MonoBehaviour
{
    [Header("References")]
    public CardList cardList;           
    public GameObject cardPrefab;
    public Transform cardGridParent;    
    public Button backButton;
    public TextMeshProUGUI titleText;
    public Image background;

    private List<CardDisplay> _spawnedCards = new List<CardDisplay>();

    private void Start()
    {
        backButton.onClick.AddListener(() => GameManager.Instance.LoadScene("Main"));

        ApplyTheme();
        SpawnCards();

        ThemeManager.Instance.OnThemeChanged += ApplyTheme;
    }

    private void OnDestroy() =>
        ThemeManager.Instance.OnThemeChanged -= ApplyTheme;

    private void SpawnCards()
    {
        List<CardData> cards = cardList.GetAllCards();

        foreach (CardData data in cards)
        {
            GameObject go = Instantiate(cardPrefab, cardGridParent);
            CardDisplay display = go.GetComponent<CardDisplay>();
            display.Initialize(data, OnCardClicked);
            _spawnedCards.Add(display);

            // Staggered entrance animation
            int index = _spawnedCards.Count - 1;
            StartCoroutine(AnimateCardIn(go.GetComponent<RectTransform>(), index * 0.08f));
        }
    }

    private void OnCardClicked(CardData data)
    {
        GameManager.SelectedCard = data;
        GameManager.Instance.LoadScene("CardDetail");
    }

    private System.Collections.IEnumerator AnimateCardIn(RectTransform rt, float delay)
    {
        rt.localScale = Vector3.zero;
        yield return new WaitForSeconds(delay);

        float t = 0f;
        float duration = 0.25f;
        while (t < duration)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        rt.localScale = Vector3.one;
    }

    private void ApplyTheme()
    {
        ThemeData theme = ThemeManager.Instance.activeTheme;
        background.color = theme.backgroundColor;
        titleText.font = theme.specialFont;
        titleText.color = theme.specialFontColor;
        titleText.fontStyle = theme.specialFontStyle;
    }
}