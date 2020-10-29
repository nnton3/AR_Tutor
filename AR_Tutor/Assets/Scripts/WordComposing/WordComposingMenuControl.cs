using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public enum ClauseType { OneWord, TwoWord, ThreeWord}

public class WordComposingMenuControl : GameMenu, IEditableElement
{
    #region Variables
    [SerializeField] private GameObject firstRankPanel, secondRankPanel, categorySelectorPanel, playPanel;
    [SerializeField] private GameObject secondRankCardParent;
    [SerializeField] private Button secondRankCloseBtn, categorySelectorCloseBtn;
    [SerializeField] private Sprite backForCustomize, backForPlay;
    [SerializeField] private Image backImg;
    private WordComposingReseter reseter;
    private MenuTransitionController transitionController;
    private WordComposingSelector wordComposingSelector;
    public ClauseType GameMode { get; private set; } = ClauseType.ThreeWord;
    #endregion

    public override void Initialize()
    {
        gameName = GameName.WordComposing;
        var selector = FindObjectOfType<WordComposingSelector>();
        cardSelector = selector;
        wordComposingSelector = selector;
        reseter = FindObjectOfType<WordComposingReseter>();
        transitionController = FindObjectOfType<MenuTransitionController>();
        base.Initialize();

        CategoriesPanels.ObserveEveryValueChanged((temp) => temp)
            .Subscribe(_ => reseter.UpdatePanelList(CategoriesPanels))
            .AddTo(this);

        reseter.Initialize(GameMode);
        mainMenu.AddEditableElement(this);
        BindCloseBtns();
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
        {
            bool[] visibles = new bool[3];
            for (int i = 0; i < _categoryData.cardKeys.Count; i++)
            {
                var key = _categoryData.cardKeys[i];
                GameObject cardObj = AddCardInMenu((i == 0) ? firstRankPanel : secondRankCardParent, "default_4_category6", key);
                BindRankCardBtn(cardObj, (i == 0) ? 1 : 2);
                cardObj.GetComponent<WordComposingCard>().MarkCardAsRank((i == 0) ? 1 : 2);

                bool visible = true;

                if (_categoryData.cardsVisible != null)
                    if (_categoryData.cardsVisible.Count - 1 >= i)
                        visible = _categoryData.cardsVisible[i];

                visibles[i] = visible;
                InitializeEditableElement(cardObj, visible);
            }

            if (!visibles[0])
                GameMode = ClauseType.TwoWord;

            if (!visibles[1] && !visibles[2])
                GameMode = ClauseType.OneWord;
        }
    }

    #region Game mode configuration
    public void ConfigurateElement(MenuMode _mode)
    {
        if (_mode == MenuMode.Play)
            ConfigurateForPlay();
        else
            ConfigurateForCustomize();
    }

    private void ConfigurateForPlay()
    {
        backImg.sprite = backForPlay;
        categorySelectorPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210f);
        foreach (var panel in CategoriesPanels)
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 210f);

        Cards[0].transform.SetParent(firstRankPanel.transform);
        Cards[1].transform.SetParent(secondRankCardParent.transform);
        Cards[2].transform.SetParent(secondRankCardParent.transform);

        reseter.SetMode(GameMode);
    }

    private void ConfigurateForCustomize()
    {
        backImg.sprite = backForCustomize;
        categorySelectorPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 325f);
        foreach (var panel in CategoriesPanels)
            panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 325f);

        for (int i = 2; i > -1; i--)
        {
            Cards[i].transform.SetParent(categoryParent.transform);
            Cards[i].transform.SetAsFirstSibling();
        }

        reseter.SetMode(ClauseType.OneWord);
    }
    #endregion

    protected override void AddNewCategory(string _categoryKey)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        CategoryData data = new CategoryData();
        if (!categoryStorage.HasCategory(gameName, _categoryKey)) return;
        data = categoryStorage.GetData(gameName, _categoryKey);

        var obj = Instantiate(categoryCardPref, categoryParent.transform);
        var categoryPanel = Instantiate(categoryPanelPref, panelParent);
        CategoriesPanels.Add(categoryPanel);
        categoryPanel.SetActive(false);

        var initializer = obj.GetComponent<CategoryInitializer>();
        initializer.Initialize(gameName, _categoryKey, data);
        CategoryCards.Add(initializer);

        InitializeEditableElement(obj, data.visible);

        BindCategoryBtn(CategoryCards.Count - 1);

        ConfigurateCards(data, categoryPanel, _categoryKey);

        var closeBtn = categoryPanel.transform.Find("CloseButton").GetComponent<Button>();
        Debug.Log(closeBtn == null);
        closeBtn.onClick.AddListener(() =>
        {
            categoryPanel.SetActive(false);
            categorySelectorPanel.SetActive(true);
        });
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var btn = CategoryCards[_categoryIndex].GetSelectBtn();
        var panel = CategoriesPanels[_categoryIndex];
        var categoryKey = CategoryCards[_categoryIndex].categoryKey;

        btn.onClick.AddListener(() =>
        {
            HidePanels();
            categorySelectorPanel.SetActive(false);
            panel.SetActive(true);
        });
    }

    private void BindRankCardBtn(GameObject _card, int _rank)
    {
        var btn = _card.GetComponent<CardBase>().GetSelectBtn();

        if (_rank == 1)
        {
            btn.onClick.AddListener(() =>
            {
                if (MainMenuUIControl.Mode == MenuMode.Play)
                {
                    firstRankPanel.SetActive(false);
                    secondRankPanel.SetActive(true);
                }
            });
        }
        else if (_rank == 2)
            btn.onClick.AddListener(() =>
            {
                if (MainMenuUIControl.Mode == MenuMode.Play)
                {
                    secondRankPanel.SetActive(false);
                    categorySelectorPanel.SetActive(true);
                }
           });
    }

    private void BindCloseBtns()
    {
        secondRankCloseBtn.onClick.AddListener(() => 
        {
            secondRankPanel.SetActive(false);
            firstRankPanel.SetActive(true);
            Signals.RemoveWordFromClause.Invoke();
        });

        categorySelectorCloseBtn.onClick.AddListener(() =>
        {
            categorySelectorPanel.SetActive(false);
            secondRankPanel.SetActive(true);
            Signals.RemoveWordFromClause.Invoke();
        });
    }

}
