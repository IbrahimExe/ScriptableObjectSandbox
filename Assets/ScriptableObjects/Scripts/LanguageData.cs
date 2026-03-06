using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocEntry
{
    public string key;
    [TextArea(1, 3)] public string en;
    [TextArea(1, 3)] public string fr;   // eng text with fr
    [TextArea(1, 3)] public string sp;   // eng text with sp

public enum GameLanguage { English, French, Spanish }


[CreateAssetMenu(fileName = "New LanguageData", menuName = "Scriptable Objects/LanguageData")]
public class LanguageData : ScriptableObject
{
    public List<LocEntry> entries = new List<LocEntry>();

    private Dictionary<string, LocEntry> _lookup;

    private void OnEnable() => BuildLookup();

    public void BuildLookup()
    {
        _lookup = new Dictionary<string, LocEntry>();
        foreach (var entry in entries)
            if (!string.IsNullOrEmpty(entry.key))
                _lookup[entry.key] = entry;
    }

    // returns the localized string version of the key, eng as default if missing
    public string GetLocalized(string key, GameLanguage language)
    {
        if (_lookup == null) BuildLookup();

        if (!_lookup.TryGetValue(key, out LocEntry entry))
        {
            Debug.LogWarning($"[LanguageData] Missing LocKey: {key}");
            return $"[MISSING: {key}]";
        }

        return language switch
        {
            GameLanguage.French => entry.fr,
            GameLanguage.Spanish => entry.sp,
            _ => entry.en,
        };
    }
}
