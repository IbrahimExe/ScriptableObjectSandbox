using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance { get; private set; }

    [Header("Assign your ThemeData SO here")]
    public ThemeData activeTheme;

    public System.Action OnThemeChanged;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetTheme(ThemeData newTheme)
    {
        activeTheme = newTheme;
        OnThemeChanged?.Invoke();
    }
}