using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Card/Create new card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;

    public Sprite cardSprite;

    public string tag;

    public Color cardColor;
}
