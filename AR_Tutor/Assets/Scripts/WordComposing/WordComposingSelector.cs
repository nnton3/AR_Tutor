using System.Collections.Generic;
using UnityEngine;

public class WordComposingSelector : MonoBehaviour, IManageCards
{
    private CardStorage cardStorage;
    [SerializeField] private GameObject clausePanel, clauseCardPref;
    public Stack<CardBase> cardsInClause { get; private set; } = new Stack<CardBase>();

    public void Initialize(List<GameObject> _cards)
    {
        cardStorage = FindObjectOfType<CardStorage>();

        Signals.AddWordToClause.AddListener(AddCardInClause);
        Signals.RemoveWordFromClause.AddListener(RemoveCardFromClause);
        Signals.ResetWordComposingMenu.AddListener(ClearClausePanel);
    }

    private void AddCardInClause(string key)
    {
        var obj = Instantiate(clauseCardPref, clausePanel.transform);
        var data = cardStorage.cards[key];
        var initializer = obj.GetComponent<CardBase>();
        initializer.Initialize(GameName.WordComposing, null, null, data);
        obj.GetComponent<EditableElement>().ConfigurateElement(MenuMode.Play);
        cardsInClause.Push(initializer);
    }

    private void RemoveCardFromClause()
    {
        if (cardsInClause.Count == 0) return;
        var lastWord = cardsInClause.Pop();
        Destroy(lastWord.gameObject);
    }

    private  void ClearClausePanel()
    {
        int count = cardsInClause.Count;
        for (int i = 0; i < count; i++)
            RemoveCardFromClause();
    }

    public void AddCard(GameObject _card) { }
    public void RemoveCard(GameObject _card) { }
}
