using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordBookCardSelector : MonoBehaviour, IManageCards
{
    [SerializeField] private GameObject wordbookContentPref;
    [SerializeField] private Transform contentParent;
    private List<GameObject> cards = new List<GameObject>();

    private void Awake()
    {
        Signals.WordBookCardSelect.AddListener(ShowContent);
    }

    public void Initialize(List<GameObject> _cards)
    {
        foreach (var card in _cards)
            cards.Add(card);
    }

    private void ShowContent(string _cardKey)
    {
        var obj = Instantiate(wordbookContentPref, contentParent);
    }

    private void ScrollImages(int direction)
    {
        
    }

    public void AddCard(GameObject _card)
    {
        
    }

    public void RemoveCard(GameObject _card)
    {
        
    }
}
