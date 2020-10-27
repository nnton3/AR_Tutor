using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WordBookCardSelector : MonoBehaviour, IManageCards
{
    #region Variables
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Button closeContentBtn;
    private List<CardBase> cards = new List<CardBase>();
    private CategoryData currentCategory;
    private GameObject lastCardsPanel;
    private string currentCard;
    private WordBookContent content;
    private CategoryStorage categoryStorage;
    private CardStorage cardStorage;
    #endregion

    private void Awake()
    {
        closeContentBtn.onClick.AddListener(() =>
        {
            contentPanel.SetActive(false);
            lastCardsPanel.SetActive(true);
        });

        Signals.WordBookCardSelect.AddListener(ShowContent);
    }

    public void Initialize(List<GameObject> _cards)
    {
        categoryStorage = FindObjectOfType<CategoryStorage>();
        cardStorage = FindObjectOfType<CardStorage>();
        content = FindObjectOfType<WordBookContent>();

        foreach (var card in _cards)
            cards.Add(card.GetComponent<CardBase>());

        Signals.DownSwipeEvent.AddListener(() => ScrollCards(-1));
        Signals.UpSwipeEvent.AddListener(() => ScrollCards(1));
        Signals.LeftSwipeEvent.AddListener(() => ScrollImages(1));
        Signals.RightSwipeEvent.AddListener(() => ScrollImages(-1));
    }

    public void SetCurrentCategory(string _categoryKey, GameObject cardsPanel)
    {
        lastCardsPanel = cardsPanel;
        currentCategory = categoryStorage.GetData(GameName.WordBook, _categoryKey);
    }

    private void ShowContent(string _cardKey)
    {
        currentCard = _cardKey;
        content.Initialize(cardStorage.cards[_cardKey]);
        contentPanel.SetActive(true);
        lastCardsPanel.SetActive(false);
    }

    private void ScrollCards(int direction)
    {
        if (direction < 0)
        {
            var index = currentCategory.cardKeys.IndexOf(currentCard) - 1;
            if (index < 0) return;
            ShowContent(cards.Find((card) => card.Key == currentCategory.cardKeys[index]).Key);
        }
        if (direction > 0)
        {
            var index = currentCategory.cardKeys.IndexOf(currentCard) + 1;
            if (index > currentCategory.cardKeys.Count - 1) return;
            ShowContent(cards.Find((card) => card.Key == currentCategory.cardKeys[index]).Key);
        }
    }

    private void ScrollImages(int direction)
    {
        if (direction < 0)
            content.ShowPreviousImg();
        if (direction > 0)
            content.ShowNextImg();
    }

    public void AddCard(GameObject _card)
    {
        cards.Add(_card.GetComponent<CardBase>());
    }

    public void RemoveCard(GameObject _card)
    {
        cards.Remove(_card.GetComponent<CardBase>());
    }
}
