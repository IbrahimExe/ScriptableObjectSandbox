using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DVD : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI cornerHitText;
    public Button backButton;

    [Header("Motion")]
    public float speed = 500f;

    private Vector2 direction;
    private RectTransform textRect;
    private RectTransform canvasRect;
    private int cornerHits = 0;

    private readonly Color[] _colors = new Color[]
    {
        Color.red, Color.cyan, Color.yellow, Color.green,
        Color.magenta, new Color(1f, 0.5f, 0f), Color.white
    };

    private void Start()
    {
        textRect = playText.GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // random start direction
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        backButton.onClick.AddListener(() => GameManager.Instance.LoadScene("MainMenu"));

        ApplyTheme();
    }

    private void Update()
    {
        Vector2 pos = textRect.anchoredPosition;
        pos += direction * speed * Time.deltaTime;

        Vector2 halfCanvas = canvasRect.sizeDelta / 2f;
        Vector2 halfText = textRect.sizeDelta / 2f;

        bool hitX = false, hitY = false;

        // Horizontal wall bounce
        if (pos.x + halfText.x >= halfCanvas.x)
        {
            pos.x = halfCanvas.x - halfText.x;
            direction.x = -Mathf.Abs(direction.x);
            hitX = true;
        }
        else if (pos.x - halfText.x <= -halfCanvas.x)
        {
            pos.x = -halfCanvas.x + halfText.x;
            direction.x = Mathf.Abs(direction.x);
            hitX = true;
        }

        // Vertical wall bounce
        if (pos.y + halfText.y >= halfCanvas.y)
        {
            pos.y = halfCanvas.y - halfText.y;
            direction.y = -Mathf.Abs(direction.y);
            hitY = true;
        }
        else if (pos.y - halfText.y <= -halfCanvas.y)
        {
            pos.y = -halfCanvas.y + halfText.y;
            direction.y = Mathf.Abs(direction.y);
            hitY = true;
        }

        // Color change on any wall hit
        if (hitX || hitY)
        {
            playText.color = _colors[Random.Range(0, _colors.Length)];

            // both walls
            if (hitX && hitY)
            {
                cornerHits++;
                cornerHitText.text = $"Corner Hits: {cornerHits}";
                StartCoroutine(CornerCelebration());
            }
        }

        textRect.anchoredPosition = pos;
    }

    private System.Collections.IEnumerator CornerCelebration()
    {
        // Quick scale pulse on corner hit
        Vector3 big = Vector3.one * 1.5f;
        float t = 0f;
        while (t < 0.2f) { textRect.localScale = Vector3.Lerp(Vector3.one, big, t / 0.2f); t += Time.deltaTime; yield return null; }
        t = 0f;
        while (t < 0.2f) { textRect.localScale = Vector3.Lerp(big, Vector3.one, t / 0.2f); t += Time.deltaTime; yield return null; }
        textRect.localScale = Vector3.one;
    }

    private void ApplyTheme()
    {
        if (ThemeManager.Instance?.activeTheme == null) return;
        playText.font = ThemeManager.Instance.activeTheme.specialFont;
        playText.fontStyle = ThemeManager.Instance.activeTheme.specialFontStyle;
        // starting color
        playText.color = ThemeManager.Instance.activeTheme.specialFontColor;
    }
}