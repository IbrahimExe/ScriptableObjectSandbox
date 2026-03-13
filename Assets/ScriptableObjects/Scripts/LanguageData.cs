using System.Collections.Generic;
using UnityEngine;

public enum GameLanguage { English, French, Spanish }

[CreateAssetMenu(fileName = "New LanguageData", menuName = "ScriptableObjectSandbox/Language Data")]
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