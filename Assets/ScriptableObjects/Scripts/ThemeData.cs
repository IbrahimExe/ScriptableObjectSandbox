using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New ThemeData", menuName = "Scriptable Objects/ThemeData")]
public class ThemeData : ScriptableObject
{
    [Header("Regular Style")]
    public TMP_FontAsset regularFont;
    public Color regularFontColor = Color.white;
    public Sprite regularButtonSprite;       

    [Header("Special / Title Style")]
    public TMP_FontAsset specialFont;
    public Color specialFontColor = Color.yellow;
    public FontStyles specialFontStyle = FontStyles.Bold;

    [Header("Background")]
    public Color backgroundColor = new Color(0.08f, 0.08f, 0.15f);
    public Sprite backgroundSprite;

    [Header("Card Frame Colors by Type")]
    public Color characterFrameColor = Color.green;
    public Color spellFrameColor = Color.cyan;
    public Color trapFrameColor = Color.red;
    public Color equipmentFrameColor = Color.yellow;

    // Returns the frame color for a given card type
    public Color GetCardFrameColor(CardType type) => type switch
    {
        CardType.Character => characterFrameColor,
        CardType.Spell => spellFrameColor,
        CardType.Trap => trapFrameColor,
        CardType.Equipment => equipmentFrameColor,
        _ => Color.white,
    };
}