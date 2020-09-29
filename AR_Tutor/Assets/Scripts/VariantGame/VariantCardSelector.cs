using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VariantCardSelector : MonoBehaviour
{
    private List<VariantCardSelectable> selectedCards = new List<VariantCardSelectable>();
    private List<string> selectedCardsKeys = new List<string>();
    private int maxCardCount;
    public StringUnityEvent SelectEvent = new StringUnityEvent();
    public StringUnityEvent UnselectEvent = new StringUnityEvent();

    public void Initialize(List<GameObject> cards)
    {
        foreach (var card in cards)
        {
            var selectableComponent = card.GetComponent<VariantCardSelectable>();
            selectedCards.Add(selectableComponent);
        }

        SelectEvent.AddListener((key) => selectedCardsKeys.Add(key));
        UnselectEvent.AddListener((key) => selectedCardsKeys.Remove(key));
    }

    public void AddCard(GameObject _cardObj)
    {
        selectedCards.Add(_cardObj.GetComponent<VariantCardSelectable>());
    }

    public void RemoveCard(GameObject _cardObj)
    {
        selectedCards.Remove(selectedCards.Find((card) => card.gameObject == _cardObj));
    }

    private void Reset()
    {
        selectedCardsKeys.Clear();
    }

    public void SetMaxCard(int value)
    {
        if (value <= 0) return;
        maxCardCount = value;
    }

    public bool CanSelect()
    {
        return selectedCardsKeys.Count < maxCardCount;
    }

    private void OnDestroy()
    {
        SelectEvent.RemoveAllListeners();
        UnselectEvent.RemoveAllListeners();
    }
}

public class StringUnityEvent : UnityEvent<string> { }
