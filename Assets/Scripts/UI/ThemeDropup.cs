using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeDropup : MonoBehaviour
{
    [Header("References")]
    public Button themeButton;
    public GameObject optionsPanel;
    public Button btnDark;
    public Button btnLight;

    [Header("Theme Assets")]
    public ThemeData darkTheme;
    public ThemeData lightTheme;

    [Header("Animation")]
    public float animationDuration = 0.2f;
    public float slideDistance = 20f;

    private bool _isOpen = false;
    private Coroutine _animationCoroutine;
    private RectTransform _panelRect;

    private void Start()
    {
        _panelRect = optionsPanel.GetComponent<RectTransform>();

        themeButton.onClick.AddListener(TogglePanel);

        btnDark.onClick.AddListener(() => SelectTheme(darkTheme));
        btnLight.onClick.AddListener(() => SelectTheme(lightTheme));

        optionsPanel.SetActive(false);
    }

    private void SelectTheme(ThemeData theme)
    {
        ThemeManager.Instance.SetTheme(theme);

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(ClosePanel());
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

    private IEnumerator OpenPanel()
    {
        _isOpen = true;
        optionsPanel.SetActive(true);

        CanvasGroup cg = GetOrAddCanvasGroup();
        Vector2 openPos = _panelRect.anchoredPosition;
        Vector2 closedPos = openPos - new Vector2(0, slideDistance);

        float t = 0f;
        while (t < animationDuration)
        {
            float progress = t / animationDuration;
            float eased = 1f - Mathf.Pow(1f - progress, 3f);

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
        Vector2 closedPos = openPos - new Vector2(0, slideDistance);

        float t = 0f;
        while (t < animationDuration)
        {
            float progress = t / animationDuration;
            float eased = Mathf.Pow(progress, 3f);

            _panelRect.anchoredPosition = Vector2.Lerp(openPos, closedPos, eased);
            cg.alpha = Mathf.Lerp(1f, 0f, eased);

            t += Time.deltaTime;
            yield return null;
        }

        optionsPanel.SetActive(false);
        _panelRect.anchoredPosition = openPos;
    }

    private CanvasGroup GetOrAddCanvasGroup()
    {
        CanvasGroup cg = optionsPanel.GetComponent<CanvasGroup>();
        if (cg == null) cg = optionsPanel.AddComponent<CanvasGroup>();
        return cg;
    }

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