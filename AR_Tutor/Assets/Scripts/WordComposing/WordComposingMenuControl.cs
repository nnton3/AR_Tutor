using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordComposingMenuControl : GameMenu
{
    [SerializeField] private GameObject firstRankPanel, secondRankPanel, categorySelectorPanel, playPanel;
    private MenuTransitionController transitionController;
    private WordComposingSelector wordComposingSelector;

    public override void Initialize()
    {
        gameName = GameName.WordComposing;
        var selector = FindObjectOfType<WordComposingSelector>();
        cardSelector = selector;
        wordComposingSelector = selector;
        transitionController = FindObjectOfType<MenuTransitionController>();
        base.Initialize();

        Signals.WordComposingActivate.AddListener(() => playPanel.SetActive(true));
        Signals.WordComposingDisable.AddListener(() => playPanel.SetActive(false));
    }

    protected override void ConfigurateCategories()
    {
        ConfigurateRankCategories();
        base.ConfigurateCategories();
    }

    private void ConfigurateRankCategories()
    {
        CategoryData data = categoryStorage.WordComposingRanks["default_4_category6"];
        ConfigurateRankCards(data);
    }

    private void ConfigurateRankCards(CategoryData _categoryData)
    {
        if (_categoryData.cardKeys != null)
            for (int i = 0; i < _categoryData.cardKeys.Count; i++)
            {
                var key = _categoryData.cardKeys[i];
                if (!cardStorage.cards.ContainsKey(key))
                {
                    Debug.Log($"Key {key} not found in card storage");
                    continue;
                }

                GameObject cardObj = AddCardInMenu((i == 0) ? firstRankPanel : secondRankPanel, "default_4_category6", key);
                BindRankCardBtn(cardObj, (i == 0) ? 1 : 2);

                bool visible = true;

                if (_categoryData.cardsVisible != null)
                    if (_categoryData.cardsVisible.Count - 1 >= i)
                        visible = _categoryData.cardsVisible[i];

                InitializeEditableElement(cardObj, visible);
            }
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var btn = CategoryCards[_categoryIndex].GetSelectBtn();
        var panel = CategoriesPanels[_categoryIndex];
        var categoryKey = CategoryCards[_categoryIndex].categoryKey;

        btn.onClick.AddListener(() =>
        {
            HidePanels();
            transitionController.ActivatePanel(panel);
            transitionController.AddEventToReturnBtn(() =>
            {
                Signals.RemoveWordFromClause.Invoke();
                ReturnBtnEventRank2();
            });
        });
    }

    private void BindRankCardBtn(GameObject _card, int _rank)
    {
        var btn = _card.GetComponent<CardBase>().GetSelectBtn();

        if (_rank == 1)
        {
            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(secondRankPanel);
                ReturnBtnEventRank1();
            });
        }
        else if (_rank == 2)
            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(categorySelectorPanel);
                ReturnBtnEventRank2();
           });
    }

    private void ReturnBtnEventRank1()
    {
        transitionController.AddEventToReturnBtn(() =>
        {
            Signals.RemoveWordFromClause.Invoke();
            transitionController.AddEventToReturnBtn(() =>
            {
                Signals.WordComposingDisable.Invoke();
            });
        });
    }

    private void ReturnBtnEventRank2()
    {
        transitionController.AddEventToReturnBtn(() =>
        {
            Signals.RemoveWordFromClause.Invoke();
            ReturnBtnEventRank1();
        });
    }
}
