using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WordComposingReseter : MonoBehaviour
{
    [SerializeField] private GameObject firstRankPanel, secondRankPanel, categorySelector;
    [SerializeField] private List<GameObject> cardPanels = new List<GameObject>();
    private UnityAction resetAction;

    public void Initialize(ClauseType _gameMode)
    {
        SetMode(_gameMode);
        Signals.ResetWordComposingMenu.AddListener(() =>
        {
            if (MainMenuUIControl.Mode == MenuMode.Play) resetAction();
        });
    }

    public void SetMode(ClauseType _gameMode)
    {
        switch (_gameMode)
        {
            case ClauseType.OneWord:
                resetAction = ConfigurateForOneWord;
                break;
            case ClauseType.TwoWord:
                resetAction = ConfigurateForTwoWord;
                break;
            case ClauseType.ThreeWord:
                resetAction = ConfigurateForTreeWord;
                break;
            default:
                break;
        }

        resetAction();
    }

    private void ConfigurateForTreeWord()
    {
        firstRankPanel.SetActive(true);
        secondRankPanel.SetActive(false);
        categorySelector.SetActive(false);

        foreach (var panel in cardPanels)
            panel.SetActive(false);
    }

    private void ConfigurateForTwoWord()
    {
        firstRankPanel.SetActive(false);
        secondRankPanel.SetActive(true);
        categorySelector.SetActive(false);

        foreach (var panel in cardPanels)
            panel.SetActive(false);
    }

    private void ConfigurateForOneWord()
    {
        firstRankPanel.SetActive(false);
        secondRankPanel.SetActive(false);
        categorySelector.SetActive(true);

        foreach (var panel in cardPanels)
            panel.SetActive(false);
    }

    public void UpdatePanelList(List<GameObject> _newList)
    {
        cardPanels = _newList;
    }
}
