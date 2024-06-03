using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Shape/Create new shape", order = 1)]
public class ImageShapeScriptableObject : ScriptableObject
{
    public Sprite shapeSprite;

    public string tag;

    public Color imageColor;
}
