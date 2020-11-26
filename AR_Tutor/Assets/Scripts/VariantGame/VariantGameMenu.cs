using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantGameMenu : GameMenu
{
    #region Variables
    private VariantCardSelector variantSelector;
    private HorizontalContentMover contentMover;

    [SerializeField] private CardHierarchyManager categoriesHierarchyManager;
    [SerializeField] private VariantGridCalculater gridControl;
    [SerializeField] private GameObject categorySelector, modeSelector;
    [SerializeField] private Button[] gameModes = new Button[] { };
    [SerializeField] private Button startGameBtn;
    #endregion

    public override void Initialize()
    {
        gameName = GameName.Variant;
        var selector = FindObjectOfType<VariantCardSelector>();
        contentMover = GetComponent<HorizontalContentMover>();
        cardSelector = selector;
        variantSelector = selector;

        categoriesHierarchyManager.Initialize();
        categorySelector.GetComponent<VariantElementsManagment>().Initialize();

        base.Initialize();

        BindBtns();
        HidePanels();

        Signals.ReturnToMainMenuEvent.AddListener(ResetGame);
    }

    #region SwitchVisible
    protected override void SwitchCategoryVisible(string _categoryKey, bool _isVisible)
    {
        var target = CategoryCards.Find((card) => card.CategoryKey == _categoryKey);
        if (target == null) return;

        if (_isVisible) categoriesHierarchyManager.SetFirst(target.transform);
        else categoriesHierarchyManager.SetLast(target.transform);
    }

    protected override void SwitchCardVisible(string _categoryKey, string _cardKey, bool _isVisible)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;
        var target = Cards.Find((card) => card.GetComponent<CardBase>().Key == _cardKey);
        if (target == null) return;

        var categoryIndex = CategoryCards.IndexOf(CategoryCards.Find((card) => card.CategoryKey == _categoryKey));
        if (_isVisible) CategoriesPanels[categoryIndex].GetComponent<CardHierarchyManager>().SetFirst(target.transform);
        else CategoriesPanels[categoryIndex].GetComponent<CardHierarchyManager>().SetLast(target.transform);
    }
    #endregion

    #region Delete content
    public override void DeleteCategory(string _categoryKey)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var target = CategoryCards.Find((category) => category.CategoryKey == _categoryKey);
        if (target != null)
        {
            mainMenu.DeleteEditableElement(target.GetComponent<EditableElement>());
            CategoryCards.Remove(target);

            categoriesHierarchyManager.DeleteCard(target.transform);
        }
    }

    protected override void DeleteCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var targetCards = Cards.FindAll((card) => card.GetComponent<CardBase>().Key == _key);

        foreach (var card in targetCards)
        {
            mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
            Cards.Remove(card);
            cardSelector.RemoveCard(card);

            var categoryIndex = CategoryCards.IndexOf(CategoryCards.Find((elem) => elem.CategoryKey == _categoryKey));
            CategoriesPanels[categoryIndex].GetComponent<CardHierarchyManager>().DeleteCard(card.transform);
        }
    }
    #endregion

    #region Create add btn
    protected override void CreateAddCategoryBtn()
    {
        CreateAddBtn(categorySelector.transform, () => categoryManager.SelectAddMethod(GameName.Variant));
    }

    protected override void CreateAddCardBtn(GameObject _categoryPanel, string _categoryKey)
    {
        CreateAddBtn(_categoryPanel.transform, () => categoryManager.SelectAddMethod(GameName.Variant, _categoryKey), _categoryKey);
    }

    protected override void CreateAddBtn(Transform _hierarchyManager, UnityAction _action, string _categoryKey = null)
    {
        var instance = Instantiate(addCardBtnPref);
        _hierarchyManager.GetComponent<CardHierarchyManager>().AddCardToEnd(instance.transform);
        var editableElem = instance.GetComponent<EditableElement>();
        mainMenu.AddEditableElement(editableElem);
        var initialiser = instance.GetComponent<CardBase>();
        initialiser.Initialize(GameName.Variant, _categoryKey, null, new CardData());
        var btn = initialiser.GetSelectBtn();
        btn.onClick.AddListener(() => _action());
    }
    #endregion

    private void BindBtns()
    {
        for (int i = 0; i < gameModes.Length; i++)
        {
            var btn = gameModes[i];
            var maxCardCount = 2 + 2 * i;

            btn.onClick.AddListener(() =>
            {
                modeSelector.SetActive(false);
                categorySelector.SetActive(true);
                variantSelector.SetMaxCard(maxCardCount);
                Signals.VariantCategoriesSelectEvent.Invoke();
            });
        }
    }

    protected override void BindCategoryBtn(int index)
    {
        var btn = CategoryCards[index].GetSelectBtn();
        var panel = CategoriesPanels[index];

        btn.onClick.AddListener(() =>
        {
            panel.SetActive(true);
            categorySelector.SetActive(false);
            if (MainMenuUIControl.Mode == MenuMode.Play)
            {
                startGameBtn.gameObject.SetActive(true);
                variantSelector.SetTargetPanel(panel);
                Signals.VariantCardsSelectEvent.Invoke();
            }
            else Signals.VarianCardSetting.Invoke();
        });
    }

    protected override void InitializePanel(GameObject _panel, string _categoryKey)
    {
        _panel.GetComponent<CardHierarchyManager>()?.Initialize();
        _panel.GetComponent<VariantElementsManagment>()?.Initialize(_categoryKey);
        mainMenu.AddEditableElement(_panel.GetComponent<IEditableElement>());
    }

    #region Helpfull
    protected override void InitializeCategoryPanel(GameObject categoryPanel)
    {
        var panelInit = categoryPanel.GetComponent<VariantCategoryPanelControl>();
        if (panelInit == null) return;

        panelInit.Initialize(() =>
        {
            startGameBtn.gameObject.SetActive(false);
            categoryPanel.SetActive(false);
            categorySelector.SetActive(true);
        });
    }

    public override void ResetGame()
    {
        modeSelector.SetActive(false);
        startGameBtn.gameObject.SetActive(false);
        variantSelector.Reset();
        HidePanels();

        Signals.StopPlayAudioEvent.Invoke();
    }

    public override void HidePanels()
    {
        base.HidePanels();
        categorySelector.gameObject.SetActive(false);
    }

    protected override void AddCategoryToBeginning(GameObject _target)
    { categoriesHierarchyManager.AddCardToBeginning(_target.transform); }

    protected override void AddCategoryToEnd(GameObject _target)
    { categoriesHierarchyManager.AddCardToEnd(_target.transform); }

    protected override void AddCardToBeginning(GameObject _target, GameObject _panel)
    { _panel.GetComponent<CardHierarchyManager>().AddCardToBeginning(_target.transform); }

    protected override void AddCardInEnd(GameObject _target, GameObject _panel)
    { _panel.GetComponent<CardHierarchyManager>().AddCardToEnd(_target.transform); }
    #endregion
}
