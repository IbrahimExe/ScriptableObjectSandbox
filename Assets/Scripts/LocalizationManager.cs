using UnityEngine;
using static LocEntry;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [Header("Assign your LanguageData SO here")]
    public LanguageData languageData;
    public GameLanguage currentLanguage = GameLanguage.English;

    // checks language change and updates all listeners
    public System.Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string Get(string key) =>
        languageData != null ? languageData.GetLocalized(key, currentLanguage) : key;

    public void SetLanguage(GameLanguage lang)
    {
        currentLanguage = lang;
        OnLanguageChanged?.Invoke();
    }
}