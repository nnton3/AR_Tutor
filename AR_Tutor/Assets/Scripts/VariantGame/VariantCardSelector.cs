using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantCardSelector : MonoBehaviour
{
    #region Variables
    [SerializeField] private Button startGameBtn;
    [SerializeField] private GameObject variantGamePanel;
    private MenuTransitionController transitionController;
    private VariantGameLogic gameLogic;
    private List<VariantCardSelectable> selectedCards = new List<VariantCardSelectable>();
    [SerializeField] private List<string> selectedCardsKeys = new List<string>();
    [SerializeField] private int maxCardCount;
    public StringUnityEvent SelectEvent = new StringUnityEvent();
    public StringUnityEvent UnselectEvent = new StringUnityEvent();
    #endregion

    public void Initialize(List<GameObject> cards)
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        gameLogic = FindObjectOfType<VariantGameLogic>();

        foreach (var card in cards)
        {
            var selectableComponent = card.GetComponent<VariantCardSelectable>();
            selectedCards.Add(selectableComponent);
        }

        startGameBtn.onClick.AddListener(() =>
        {
            if (DataIsValid())
                StartGame();
        });
        SelectEvent.AddListener((key) => selectedCardsKeys.Add(key));
        UnselectEvent.AddListener((key) => selectedCardsKeys.Remove(key));
        transitionController.AddEventToReturnBtn(() => selectedCardsKeys.Clear());
    }

    private void StartGame()
    {
        gameLogic.FillPanel(selectedCardsKeys.ToArray());
        UnselectAll();
        transitionController.ActivatePanel(new GameObject[] { variantGamePanel });
        transitionController.AddEventToReturnBtn(() => gameLogic.ClearOldData());
    }

    private bool DataIsValid()
    {
        if (selectedCardsKeys.Count == maxCardCount) return true;
        return false;
    }

    public void UnselectAll()
    {
        foreach (var card in selectedCards)
            card.Unselect();
    }

    #region Menu manage
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
    #endregion

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
