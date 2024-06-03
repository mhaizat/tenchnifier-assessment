using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehavior : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private List<CardScriptableObject> cardList;

    [SerializeField] private Image[] cardInterface;

    private GameSceneManager gameSceneManager;
    private CardSpawnBehavior cardSpawnBehavior;
    private ImageShapeBehavior imageShapeBehavior;

    private RectTransform cardRectTransform;

    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardText;

    public Canvas canvas;

    private Vector2 cardInitialPosition;
    private Vector2 cardPivotValue;

    private bool isMatched;
    private bool notMatched;

    private int index;

    private void Start()
    {
        gameSceneManager = GameSceneManager.Instance;

        cardSpawnBehavior = gameObject.transform.parent.GetComponent<CardSpawnBehavior>();

        if (gameSceneManager)
        {
            canvas = gameSceneManager.GetCanvas();
        }

        index = Random.Range(0, 3);

        SetCardInfo();

        cardInitialPosition = new Vector2(transform.position.x, transform.position.y);

        cardRectTransform = GetComponent<RectTransform>();
        cardPivotValue = new Vector2(cardRectTransform.pivot.x, cardRectTransform.pivot.y);

        ChangeCardInterface(false);
    }

    public void SetCardInfo()
    {
        cardImage.sprite = cardList[index].cardSprite;
        cardInterface[0].color = cardList[index].cardColor;

        cardText.text = cardList[index].cardName;
        cardInterface[1].sprite = cardList[index].cardSprite;
        cardInterface[1].color = cardList[index].cardColor;
    }

    private void ChangeCardInterface(bool isChange)
    {
        if (isChange)
        {
            cardInterface[0].gameObject.SetActive(false);
            cardInterface[1].gameObject.SetActive(true);
            return;
        }

        cardInterface[0].gameObject.SetActive(true);
        cardInterface[1].gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ChangeCardInterface(true);

        Vector3 mousePos = Input.mousePosition;

        cardRectTransform.pivot = new Vector2(0.5f, 0.5f);
        cardRectTransform.position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isMatched)
        {
            gameSceneManager.SetScore(10);
            cardSpawnBehavior.SpawnNewCard();
            imageShapeBehavior.SpawnNewShape(imageShapeBehavior.gameObject.transform.localScale, imageShapeBehavior.gameObject.transform.localScale * 1.5f, .5f);

            Destroy(gameObject);

            return;
        }

        if (notMatched)
        { 
            gameSceneManager.SetScore(-5);
        }

        ChangeCardInterface(false);

        cardRectTransform.pivot = cardPivotValue;
        transform.position = cardInitialPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shape"))
        {
            imageShapeBehavior = collision.gameObject.GetComponent<ImageShapeBehavior>();

            if (cardList[index].tag == imageShapeBehavior.GetSelectedImageShape().tag)
            {
                isMatched = true;
                return;
            }

            notMatched = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isMatched = false;
        notMatched = false;
    }
}
