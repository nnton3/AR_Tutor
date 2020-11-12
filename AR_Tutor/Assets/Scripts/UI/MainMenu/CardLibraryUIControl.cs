using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibraryUIControl : MonoBehaviour
{
    [SerializeField] private GameObject libraryCardPref, libraryPanel;
    [SerializeField] private GridLayoutGroup grid;
    private CardStorage storage;
    private List<LibraryCardInitializer> libraryCards = new List<LibraryCardInitializer>();

    public void Initialize()
    {
        storage = FindObjectOfType<CardStorage>();
        FindObjectOfType<LibraryCardSelector>().Initialize();

        Signals.AddCardEvent.AddListener(AddCard);

        FillLibrary();
    }

    private void FillLibrary()
    {
        foreach (var card in storage.cards)
            if (card.Value.IsCustom)
                CreateCardInstance(card.Key, card.Value);
    }

    public void AddCard(string _category, string _cardKey)
    {
        if (libraryCards.Find((card) => card.cardKey == _cardKey))
        {
            Debug.Log("Library contain this card");
            return;
        }
        else CreateCardInstance(_cardKey, storage.cards[_cardKey]);
    }

    private void CreateCardInstance(string key, CardData data)
    {
        var instance = Instantiate(libraryCardPref, libraryPanel.transform);
        var initializer = instance.GetComponent<LibraryCardInitializer>();
        initializer.Initialize(key, data);
        libraryCards.Add(initializer);
        UIInstruments.GetSizeForVerticalGrid(grid, libraryCards.Count);
    }

    public void BindCardsForSelect()
    {
        foreach (var card in libraryCards)
            card.EnableSelectable();
    }

    public void ClearBtnsEvents()
    {
        foreach (var card in libraryCards)
            card.ClearEvent();
    }

    public void UpdateCardImg(string cardKey, Sprite cardImg)
    {
        var target = libraryCards.Find((card) => card.cardKey == cardKey);
        target.UpdateImg(cardImg);
    }
}
