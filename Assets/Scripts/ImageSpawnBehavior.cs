using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSpawnBehavior : MonoBehaviour
{
    [SerializeField] private GameObject imageShapePrefab;

    void Start()
    {
        SpawnNewShape();
    }

    public void SpawnNewShape()
    {
        GameObject imageShape = Instantiate(imageShapePrefab, transform.position, Quaternion.identity);

        imageShape.transform.SetParent(transform);

        RectTransform imageRectTransform = imageShape.GetComponent<RectTransform>();

        if (imageRectTransform)
        {
            imageRectTransform.localScale = Vector2.one;
        }
    }
}
