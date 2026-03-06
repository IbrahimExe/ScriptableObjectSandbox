using UnityEngine;

[CreateAssetMenu(fileName = "New CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [Header("Localization Keys")]
    public string nameLocKey;       
    public string descriptionLocKey;

    [Header("Card Stats")]
    public CardType type;
    public int cost;
    public int attack;
    public int defense;

    [Header("Visuals")]
    public Sprite cardImage;
}
public enum CardType
{
    Creature,
    Spell,
    Trap,
    Equipment
}