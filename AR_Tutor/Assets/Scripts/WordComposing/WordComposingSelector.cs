using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordComposingSelector : MonoBehaviour, IManageCards
{
    private CardStorage cardStorage;
    [SerializeField] private List<CardBase> cardsInClause = new List<CardBase>();
    [SerializeField] private GameObject clausePanel, clauseCardPref;
    [SerializeField] private Button playClauseBtn;

    public void Initialize(List<GameObject> _cards)
    {
        cardStorage = FindObjectOfType<CardStorage>();

        playClauseBtn.onClick.AddListener(PlayClause);

        Signals.AddWordToClause.AddListener(AddCardInClause);
        Signals.AddCategoryWord.AddListener(AddCardInClause);
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
        cardsInClause.Add(initializer);
    }

    private void AddCardInClause(string _title, Sprite _sprite, AudioClip _clip)
    {
        var obj = Instantiate(clauseCardPref, clausePanel.transform);

        var data = new CardData(
            _title, _title,
            _sprite, null, null,
            null, _clip);

        var initializer = obj.GetComponent<CardBase>();
        initializer.Initialize(GameName.WordComposing, null, null, data);
        obj.GetComponent<EditableElement>().ConfigurateElement(MenuMode.Play);
        cardsInClause.Add(initializer);
    }

    private void PlayClause()
    {
        StartCoroutine(PlayClauseRoutine());
    }

    private IEnumerator PlayClauseRoutine()
    {
        for(int i = 0; i < cardsInClause.Count; i++)
        {
            Signals.PlayAcudioClipEvent.Invoke(cardsInClause[i].Clip2);
            yield return new WaitForSeconds(cardsInClause[i].Clip2.length);
        }
    }

    private void RemoveCardFromClause()
    {
        if (cardsInClause.Count == 0) return;
        var lastWord = cardsInClause[cardsInClause.Count - 1];
        cardsInClause.Remove(lastWord);
        Destroy(lastWord.gameObject);
    }

    public void ClearClausePanel()
    {
        int count = cardsInClause.Count;
        for (int i = 0; i < count; i++)
            RemoveCardFromClause();
    }

    public void AddCard(GameObject _card) { }
    public void RemoveCard(GameObject _card) { }
}
