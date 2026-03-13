using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New CardList", menuName = "Scriptable Objects/CardList")]
public class CardList : ScriptableObject
{
    public List<string> cardResourcePaths = new List<string>();

    // method of loading where cards are loaded once and stored here
    private Dictionary<string, CardData> _loadedCards = new Dictionary<string, CardData>();

    public CardData GetCard(string resourcePath)
    {
        if (_loadedCards.TryGetValue(resourcePath, out CardData cached))
            return cached;

        CardData loaded = Resources.Load<CardData>(resourcePath);

        if (loaded == null)
            Debug.LogWarning($"[CardList] Could not find CardData at path: {resourcePath}");
        else
            _loadedCards[resourcePath] = loaded;

        return loaded;
    }

    public List<CardData> GetAllCards()
    {
        List<CardData> result = new List<CardData>();
        foreach (string path in cardResourcePaths)
        {
            CardData card = GetCard(path);
            if (card != null)
                result.Add(card);
        }
        return result;
    }

    public void ClearCache() => _loadedCards.Clear();
}