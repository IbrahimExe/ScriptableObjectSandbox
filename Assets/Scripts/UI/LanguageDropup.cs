using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static LocEntry;

public class LanguageDropup : MonoBehaviour
{
    [Header("References")]
    public Button globeButton;
    public GameObject optionsPanel;
    public Button btnEnglish;
    public Button btnFrench;
    public Button btnSpanish;

    [Header("Animation")]
    public float animationDuration = 0.2f;
    public float slidDistance = 20f;     

    private bool _isOpen = false;
    private Coroutine _animationCoroutine;
    private RectTransform _panelRect;

    private void Start()
    {
        _panelRect = optionsPanel.GetComponent<RectTransform>();

        globeButton.onClick.AddListener(TogglePanel);

        btnEnglish.onClick.AddListener(() => SelectLanguage(GameLanguage.English));
        btnFrench.onClick.AddListener(() => SelectLanguage(GameLanguage.French));
        btnSpanish.onClick.AddListener(() => SelectLanguage(GameLanguage.Spanish));

        // Make sure panel starts hidden
        optionsPanel.SetActive(false);
    }

    private void TogglePanel()
    {
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        if (_isOpen)
            _animationCoroutine = StartCoroutine(ClosePanel());
        else
            _animationCoroutine = StartCoroutine(OpenPanel());
    }

    private void SelectLanguage(GameLanguage lang)
    {
        LocalizationManager.Instance.SetLanguage(lang);

        // Close the panel after selection
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(ClosePanel());
    }

    private IEnumerator OpenPanel()
    {
        _isOpen = true;
        optionsPanel.SetActive(true);

        CanvasGroup cg = GetOrAddCanvasGroup();
        Vector2 closedPos = _panelRect.anchoredPosition - new Vector2(0, slidDistance);
        Vector2 openPos = _panelRect.anchoredPosition;

        float t = 0f;
        while (t < animationDuration)
        {
            float progress = t / animationDuration;
            float eased = 1f - Mathf.Pow(1f - progress, 3f); // ease out cubic

            _panelRect.anchoredPosition = Vector2.Lerp(closedPos, openPos, eased);
            cg.alpha = Mathf.Lerp(0f, 1f, eased);

            t += Time.deltaTime;
            yield return null;
        }

        _panelRect.anchoredPosition = openPos;
        cg.alpha = 1f;
    }

    private IEnumerator ClosePanel()
    {
        _isOpen = false;

        CanvasGroup cg = GetOrAddCanvasGroup();
        Vector2 openPos = _panelRect.anchoredPosition;
        Vector2 closedPos = openPos - new Vector2(0, slidDistance);

        float t = 0f;
        while (t < animationDuration)
        {
            float progress = t / animationDuration;
            float eased = Mathf.Pow(progress, 3f); // ease in cubic

            _panelRect.anchoredPosition = Vector2.Lerp(openPos, closedPos, eased);
            cg.alpha = Mathf.Lerp(1f, 0f, eased);

            t += Time.deltaTime;
            yield return null;
        }

        optionsPanel.SetActive(false);
        _panelRect.anchoredPosition = openPos; // reset for next open
    }

    private CanvasGroup GetOrAddCanvasGroup()
    {
        CanvasGroup cg = optionsPanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = optionsPanel.AddComponent<CanvasGroup>();
        return cg;
    }

    // Clicking anywhere outside closes the panel
    private void Update()
    {
        if (_isOpen && Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    GetComponent<RectTransform>(),
                    Input.mousePosition,
                    null))
            {
                if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
                _animationCoroutine = StartCoroutine(ClosePanel());
            }
        }
    }
}