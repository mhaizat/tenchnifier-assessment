using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageShapeBehavior : MonoBehaviour
{
    private ImageSpawnBehavior imageSpawnBehavior;

    [SerializeField] private List<ImageShapeScriptableObject> shapeList;

    [SerializeField] private ImageShapeScriptableObject selectedImageShape;

    [SerializeField] private Image shapeImage;

    private int index;

    void Start()
    {
        imageSpawnBehavior = gameObject.transform.parent.GetComponent<ImageSpawnBehavior>();

        index = Random.Range(0, 3);

        SetImageInfo();
    }

    public void SetImageInfo()
    {
        selectedImageShape = shapeList[index];

        shapeImage.sprite = selectedImageShape.shapeSprite;
        shapeImage.color = selectedImageShape.imageColor;
    }

    public void SpawnNewShape(Vector3 initialScale, Vector3 targetScale, float timer)
    {
        StartCoroutine(ShapeEffect(initialScale, targetScale, timer));
    }

    public IEnumerator ShapeEffect(Vector3 initialScale, Vector3 targetScale, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float lerpFactor = elapsedTime / time;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, lerpFactor);

            Color imageColor = shapeImage.color;

            imageColor.a -= lerpFactor * Time.deltaTime;
            shapeImage.color = imageColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        imageSpawnBehavior.SpawnNewShape();
        Destroy(gameObject);
    }

    public ImageShapeScriptableObject GetSelectedImageShape() { return selectedImageShape; }
}
