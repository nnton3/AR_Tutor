using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum ClauseType { OneWord, TwoWord, ThreeWord}

public class WordComposingMenuControl : GameMenu, IEditableElement
{
    #region Variables
    [SerializeField] private GameObject firstRankParent, secondRankPanel, categorySelectorPanel, playPanel;
    [SerializeField] private GameObject secondRankParent;
    [SerializeField] private GameObject rankCardPref;
    [SerializeField] private GridLayoutGroup categoriesGrid;
    [SerializeField] private Button resetBtn;
    [SerializeField] private HorizontalContentMover categoriesMover;
    [SerializeField] private WordcomposingGridCalculater gridControl;
    private WordComposingReseter reseter;
    private WordComposingSelector wordComposingSelector;
    private WordComposingPanelSizeControl panelSizeControl;
    public ClauseType GameMode { get; private set; } = ClauseType.ThreeWord;
    public static bool ClauseComplete { get; private set; } = false;
    #endregion

    public override void Initialize()
    {
        gameName = GameName.WordComposing;
        var selector = FindObjectOfType<WordComposingSelector>();
        cardSelector = selector;
        wordComposingSelector = selector;
        reseter = FindObjectOfType<WordComposingReseter>();
        panelSizeControl = FindObjectOfType<WordComposingPanelSizeControl>();

        base.Initialize();

        CategoriesPanels.ObserveEveryValueChanged((temp) => temp)
            .Subscribe(_ => reseter.UpdatePanelList(CategoriesPanels))
            .AddTo(this);

        categorySelectorPanel.GetComponent<ElementsManagment>().Initialize();

        Signals.LastWordSelected.AddListener(() => ClauseComplete = true);
        Signals.RemoveFirstWord.AddListener(() => GameMode = ClauseType.TwoWord);
        Signals.ReturnAllWordInClause.AddListener(() => GameMode = ClauseType.ThreeWord);
        Signals.RemoveSecondRankWord.AddListener(() => 
        {
            if (GetVisibleSecondRankCardCount() == 0)
                GameMode = ClauseType.OneWord;
        });
        Signals.ReturnSecondRankCard.AddListener(() => GameMode = ClauseType.TwoWord);
        Signals.ReturnToMainMenuEvent.AddListener(ResetGame);
        mainMenu.AddEditableElement(this);
        BinBtn();
    }

    private void Start()
    {
        //gridControl.CalculateCategoryGrid();
        //CalculateCategoryPanelRect();

        //foreach (var panel in CategoriesPanels)
        //    panel.GetComponent<WordComposingCardPanel>().UpdateGrid();

        //reseter.Initialize(GameMode);
    }

    private int GetVisibleSecondRankCardCount()
    {
        int count = 2;
        if (!Cards[1].GetComponent<EditableElement>().Visible) --count;
        if (!Cards[2].GetComponent<EditableElement>().Visible) --count;
        return count;
    }

    protected override void ConfigurateCategories()
    {
        ConfigurateRankCategories();
        base.ConfigurateCategories();
    }

    private void ConfigurateRankCategories()
    {
        CategoryData data = categoryStorage.WordComposingRanks["default_4_category28"];
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
                GameObject cardObj = AddRankCardInMenu((i == 0) ? firstRankParent : secondRankParent, "default_4_category28", key);
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

    private GameObject AddRankCardInMenu(GameObject _categoryPanel, string _categoryKey, string _cardKey)
    {
        var cardData = cardStorage.cards[_cardKey];
        GameObject cardObj;

        var parent = _categoryPanel.transform.Find("Mask/Content");
        if (parent == null) parent = _categoryPanel.transform;
        cardObj = Instantiate(rankCardPref, parent);

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardBase>();
        initializer.Initialize(gameName, _categoryKey, _cardKey, cardData);
        return cardObj;
    }
    
    public void ConfigurateElement(MenuMode _mode)
    {
        if (_mode == MenuMode.Play)
        {
            panelSizeControl.ConfigurateForPlay(Cards[0], Cards[1], Cards[2], CategoriesPanels);
            reseter.SetMode(GameMode);
            wordComposingSelector.ShowPlayClauseBtn();
            resetBtn.gameObject.SetActive(true);
            gridControl.SetRowConstraint(2);
            gridControl.CalculateCategoryGrid();
            CalculateCategoryPanelRect();
            foreach (var panel in CategoriesPanels)
                panel.GetComponent<WordComposingCardPanel>().UpdateGrid();
        }
        else
        {
            panelSizeControl.ConfigurateForCustomize(Cards[0], Cards[1], Cards[2], CategoriesPanels);
            reseter.SetMode(ClauseType.OneWord);
            wordComposingSelector.HidePlayClauseBtn();
            resetBtn.gameObject.SetActive(false);
            gridControl.SetRowConstraint(3);
            gridControl.CalculateCategoryGrid();
            CalculateCategoryPanelRect();
            foreach (var panel in CategoriesPanels)
                panel.GetComponent<WordComposingCardPanel>().UpdateGrid(3);
        }
    }

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
        CalculateCategoryPanelRect();
        ConfigurateCards(data, categoryPanel, _categoryKey);

        var closeBtn = categoryPanel.transform.Find("CloseButton").GetComponent<Button>();
        closeBtn.onClick.AddListener(() =>
        {
            categoryPanel.SetActive(false);
            categorySelectorPanel.SetActive(true);
            if (ClauseComplete)
            {
                ClauseComplete = false;
                Signals.RemoveWordFromClause.Invoke();
            }
        });

        categoryPanel.GetComponent<ElementsManagment>()?.Initialize(_categoryKey);
    }

    protected override void CalculateCategoryPanelRect()
    {
        gridControl.CalculateCategoryContentSize(UIInstruments.GetVisibleElements(categoryParent));
        categoriesMover.CalculateMinPos();
    }

    protected override void CalculateCardPanelRect(GameObject _panel)
    {
        _panel.GetComponent<WordComposingCardPanel>().UpdateGrid();
    }

    private void BinBtn()
    {
        resetBtn.onClick.AddListener(() =>
        {
            Signals.ResetWordComposingMenu.Invoke();
            ClauseComplete = false;
        });
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var panel = CategoriesPanels[_categoryIndex];

        CategoryCards[_categoryIndex].GetSelectBtn().GetComponent<SelectBtnControl>().SetBtnEvents(() =>
        {
            if (!ClauseComplete)
            {
                HidePanels();
                categorySelectorPanel.SetActive(false);
                panel.SetActive(true);
                Signals.WordComposingCardSelectEvent.Invoke();
            }

            if (MainMenuUIControl.Mode == MenuMode.CustomizeMenu)
                Signals.WordcomposingCardSetting.Invoke();
        });
    }

    private void BindRankCardBtn(GameObject _card, int _rank)
    {
        var btn = _card.GetComponent<CardBase>().GetSelectBtn();

        if (_rank == 1)
            btn.onClick.AddListener(() =>
            {
                if (MainMenuUIControl.Mode == MenuMode.Play)
                {
                    firstRankParent.SetActive(false);
                    secondRankPanel.SetActive(true);
                }
            });
        else if (_rank == 2)
            btn.onClick.AddListener(() =>
            {
                if (MainMenuUIControl.Mode == MenuMode.Play)
                {
                    secondRankPanel.SetActive(false);
                    categorySelectorPanel.SetActive(true);
                    Signals.WordComposingCategoriesSelectEvent.Invoke();
                }
           });
    }

    public override void ResetGame()
    {
        Signals.ResetWordComposingMenu.Invoke();
        ClauseComplete = false;
        playPanel.SetActive(false);
        Signals.StopPlayAudioEvent.Invoke();
    }
}
