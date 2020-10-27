using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantCardSelector : MonoBehaviour, IManageCards
{
    #region Variables
    [SerializeField] private Button startGameBtn;
    [SerializeField] private GameObject variantGamePanel;
    [SerializeField] private List<string> selectedCardsKeys = new List<string>();
    [SerializeField] private int maxCardCount;
    private MainMenuUIControl mainMenuControl;
    private MenuTransitionController transitionController;
    private VariantGameLogic gameLogic;
    private List<VariantCardSelectable> selectedCards = new List<VariantCardSelectable>();
    public StringEvent SelectEvent = new StringEvent();
    public StringEvent UnselectEvent = new StringEvent();
    #endregion

    public void Initialize(List<GameObject> cards)
    {
        mainMenuControl = FindObjectOfType<MainMenuUIControl>();
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
            {
                startGameBtn.gameObject.SetActive(false);
                StartGame();
            }
        });
        SelectEvent.AddListener((key) => selectedCardsKeys.Add(key));
        UnselectEvent.AddListener((key) => selectedCardsKeys.Remove(key));
        transitionController.AddEventToReturnBtn(() => selectedCardsKeys.Clear());
    }

    private void StartGame()
    {
        gameLogic.FillPanel(selectedCardsKeys.ToArray());
        UnselectAll();
        transitionController.ActivatePanel(variantGamePanel);
        transitionController.AddEventToReturnBtn(() =>
        {
            startGameBtn.gameObject.SetActive(true);
            gameLogic.ClearOldData();
            transitionController.AddEventToReturnBtn(() =>
            {
                startGameBtn.gameObject.SetActive(false);
            });
        });
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
