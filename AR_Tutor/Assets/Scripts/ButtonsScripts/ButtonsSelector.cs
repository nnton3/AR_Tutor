using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

public class ButtonsEvent : UnityEvent<ButtonsCard> { }

public class ButtonsSelector : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<GameObject> Cards = new List<GameObject>();
    [SerializeField] private List<ButtonsCard> selectedCards = new List<ButtonsCard>();
    [SerializeField] private Button startGameBtn;
    [SerializeField] private GameObject buttonsGamePanel;
    private ButtonsGameLogic gameLogic;
    private MenuTransitionController transitionController;
    public ButtonsEvent SelectEvent = new ButtonsEvent();
    public ButtonsEvent UnselectEvent = new ButtonsEvent();
    #endregion

    void Awake()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        gameLogic = FindObjectOfType<ButtonsGameLogic>();

        startGameBtn.onClick.AddListener(() =>
        {
            if (DataIsValid())
                StartGame();
        });

        SelectEvent.AddListener((card) => selectedCards.Add(card));
        UnselectEvent.AddListener((card) => selectedCards.Remove(card));
    }

    private void StartGame()
    {
        gameLogic.FillPanel(selectedCards.ToArray());
        //UnselectAll();
        transitionController.ActivatePanel(buttonsGamePanel);
        transitionController.AddEventToReturnBtn(() =>
        {
            gameLogic.ClearOldData();
        });
    }

    private bool DataIsValid()
    {
        if (selectedCards.Count == 2) return true;
        return false;
    }

    public void UnselectAll()
    {
        int count = selectedCards.Count;
        for (int i = 0; i < count; i++)
            selectedCards[0].Unselect();
    }

    public bool CanSelect()
    {
        return selectedCards.Count < 2;
    }
}
