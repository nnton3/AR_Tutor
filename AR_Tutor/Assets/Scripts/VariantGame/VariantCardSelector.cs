using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantCardSelector : MonoBehaviour, IManageCards
{
    #region Variables
    [SerializeField] private Button startGameBtn;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private List<string> selectedCardsKeys = new List<string>();
    [SerializeField] private int maxCardCount;
    [SerializeField] private GameObject cardPanel;
    private Text indicator;
    private VariantGameLogic gameLogic;
    private List<VariantCardSelectable> selectedCards = new List<VariantCardSelectable>();
    public StringEvent SelectEvent = new StringEvent();
    public StringEvent UnselectEvent = new StringEvent();
    #endregion

    public void Initialize(List<GameObject> cards)
    {
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
        SelectEvent.AddListener((key) =>
        {
            selectedCardsKeys.Add(key);
            UpdateIndicator();
        });
        UnselectEvent.AddListener((key) =>
        {
            selectedCardsKeys.Remove(key);
            UpdateIndicator();
        });
    }

    private void StartGame()
    {
        gameLogic.FillPanel(selectedCardsKeys.ToArray());
        UnselectAll();
        cardPanel.SetActive(false);
        gamePanel.SetActive(true);
        Signals.VariantGameEvent.Invoke();
    }

    private bool DataIsValid()
    {
        if (selectedCardsKeys.Count == maxCardCount) return true;
        return false;
    }

    private void UpdateIndicator()
    {
        indicator.text = $"{selectedCardsKeys.Count}/{maxCardCount}";
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

    public void Reset()
    {
        gamePanel.SetActive(false);
        selectedCardsKeys.Clear();
        gameLogic.ClearOldData();
        UnselectAll();
    }
    #endregion

    public void SetMaxCard(int value)
    {
        if (value <= 0) return;
        maxCardCount = value;
    }

    public void SetTargetPanel(GameObject _panel)
    {
        cardPanel = _panel;
        indicator = _panel.transform.Find("Indicator/Text").GetComponent<Text>();
        UpdateIndicator();
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
