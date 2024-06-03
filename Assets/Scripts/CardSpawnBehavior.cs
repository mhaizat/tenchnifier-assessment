using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnBehavior : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject spawnedCard;

    private GameSceneManager gameSceneManager;

    private void Start()
    {
        gameSceneManager = GameSceneManager.Instance;
        SpawnNewCard();
    }

    private void OnEnable()
    {
        ManagerHub.OnReshuffleCards += SpawnNewCard;
    }

    private void OnDisable()
    {
        ManagerHub.OnReshuffleCards -= SpawnNewCard;
    }

    public void SpawnNewCard()
    {
        if (spawnedCard)
        {
            Destroy(spawnedCard);
        }

        spawnedCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
        spawnedCard.transform.SetParent(transform);

        RectTransform imageRectTransform = spawnedCard.GetComponent<RectTransform>();

        if (imageRectTransform)
        {
            imageRectTransform.localScale = Vector2.one;
        }
    }
}
