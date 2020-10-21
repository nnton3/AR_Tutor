using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantGameMenu : MonoBehaviour
{
    #region Variables
    private PatientDataManager patientDataManager;
    private CardStorage cardStorage;
    private CategoryStorage categoryStorage;
    private CategoryManager categoryManager;
    private MainMenuUIControl mainMenu;
    private VariantCardSelector cardSelector;
    private MenuTransitionController transitionController;

    [SerializeField] private GameObject categoryCardPref, categoryPanelPref, cardPref, customCardPref, addCardBtnPref, categoriesSelector, VariantGameParent;
    [SerializeField] private List<CategoryInitializer> CategoryCards = new List<CategoryInitializer>();
    [SerializeField] private List<GameObject> CategoriesPanels = new List<GameObject>(), Cards = new List<GameObject>();
    [SerializeField] private Button[] gameModes = new Button[] { };
    #endregion

    public void Initialize()
    {
        cardStorage = FindObjectOfType<CardStorage>();
        categoryManager = FindObjectOfType<CategoryManager>();
        mainMenu = FindObjectOfType<MainMenuUIControl>();
        cardSelector = FindObjectOfType<VariantCardSelector>();
        transitionController = FindObjectOfType<MenuTransitionController>();
        categoryStorage = FindObjectOfType<CategoryStorage>();
        patientDataManager = FindObjectOfType<PatientDataManager>();

        ConfigurateCategories();
        
        BindBtns();
        HidePanels();

        Signals.AddCategoryEvent.AddListener(AddNewCategory);
        Signals.AddCardEvent.AddListener(AddNewCard);
        Signals.DeleteCategoryFromGame.AddListener(DeleteCategory);
        Signals.DeleteCardFromCategory.AddListener(DeleteCard);
        cardSelector.Initialize(Cards);
    }

    private void ConfigurateCategories()
    {
        foreach (var categoryKey in patientDataManager.PatientData.CategoriesKeys)
            AddNewCategory(categoryKey);

        CreateAddCategoryBtn();
    }

    private void ConfigurateCards(CategoryData _categoryData, GameObject _categoryPanel, string _categoryKey)
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

                GameObject cardObj = AddCardInMenu(_categoryPanel, _categoryKey, key);

                bool visible = true;
                if (_categoryData.cardsVisible != null)
                    if (_categoryData.cardsVisible.Count - 1 >= i)
                        visible = _categoryData.cardsVisible[i];

                InitializeEditableElement(cardObj, visible);
            }

        CreateAddCardBtn(_categoryPanel, _categoryKey);
    }

    private void AddNewCategory(string _categoryKey)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        CategoryData data = new CategoryData();
        if (!categoryStorage.VariantCategories.ContainsKey(_categoryKey)) return;
        data = categoryStorage.VariantCategories[_categoryKey];

        var obj = Instantiate(categoryCardPref, categoriesSelector.transform.Find("Content"));
        var categoryPanel = Instantiate(categoryPanelPref, VariantGameParent.transform);
        CategoriesPanels.Add(categoryPanel);
        categoryPanel.SetActive(false);

        var initializer = obj.GetComponent<CategoryInitializer>();
        initializer.Initialize(GameName.Variant, _categoryKey, data);
        CategoryCards.Add(initializer);

        InitializeEditableElement(obj, data.visible);

        BindCategoryBtn(CategoryCards.Count - 1);

        ConfigurateCards(data, categoryPanel, _categoryKey);
    }

    public void AddNewCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var targetCard = CategoryCards.Find((categoryObj) => categoryObj.GetComponent<CategoryInitializer>().categoryKey == _categoryKey);
        var index = CategoryCards.IndexOf(targetCard);
        GameObject cardObj = AddCardInMenu(CategoriesPanels[index], _categoryKey, _key);

        InitializeEditableElement(cardObj);

        cardSelector.AddCard(cardObj);
    }

    private GameObject AddCardInMenu(GameObject _categoryPanel, string _categoryKey, string _cardKey)
    {
        var cardData = cardStorage.cards[_cardKey];
        GameObject cardObj;

        cardObj = (!cardData.IsCustom) ? 
            Instantiate(cardPref, _categoryPanel.transform.Find("Content")) : Instantiate(customCardPref, _categoryPanel.transform.Find("Content"));

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardBase>();
        initializer.Initialize(GameName.Variant, _categoryKey, _cardKey, cardData);
        return cardObj;
    }

    private bool IsCategoryForThisGame(string _categoryKey)
    {
        var game = categoryStorage.Categories[_categoryKey].game;
        return (GameName)game == GameName.Variant;
    }

    private void InitializeEditableElement(GameObject _obj, bool visible = true)
    {
        var editableElem = _obj.GetComponent<EditableElement>();
        editableElem.Visible = visible;
        editableElem.ConfigurateElement(mainMenu.Mode);
        mainMenu.AddEditableElement(editableElem);
    }

    #region Add btn
    private void CreateAddCardBtn(GameObject _categoryPanel, string _categoryKey)
    {
        CreateAddBtn(_categoryPanel, () => categoryManager.SelectAddMethod(GameName.Variant, _categoryKey), _categoryKey);
    }

    private void CreateAddCategoryBtn()
    {
        CreateAddBtn(categoriesSelector, () => categoryManager.SelectAddMethod(GameName.Variant));
    }

    private void CreateAddBtn(GameObject _parent, UnityAction _action, string _categoryKey = null)
    {
        var instance = Instantiate(addCardBtnPref, _parent.transform.Find("Content"));
        var editableElem = instance.GetComponent<EditableElement>();
        mainMenu.AddEditableElement(editableElem);
        var initialiser = instance.GetComponent<CardBase>();
        initialiser.Initialize(GameName.Variant, _categoryKey, null, new CardData());
        var btn = initialiser.GetSelectBtn();
        btn.onClick.AddListener(() => _action());
    }
    #endregion

    #region Update Image
    public void UpdateCategoryImage(string _categoryKey, Sprite _categoryImg)
    {
        var targetInstance = CategoryCards.Find((card) => card.categoryKey == _categoryKey);
        targetInstance.UpdateImg(_categoryImg);
    }

    public void UpdateCardImg(string _cardKey, Sprite _cardImg)
    {
        var validCards = Cards.FindAll((card) => card.GetComponent<CardBase>().Key == _cardKey);
        foreach (var item in validCards)
            item.GetComponent<CardBase>().UpdateImg(_cardImg);
    }
    #endregion

    #region Delete content
    public void DeleteCategory(string _categoryKey)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var target = CategoryCards.Find((category) => category.categoryKey == _categoryKey);
        if (target != null)
        {
            mainMenu.DeleteEditableElement(target.GetComponent<EditableElement>());
            CategoryCards.Remove(target);
            Destroy(target.gameObject);
        }
    }

    private void DeleteCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        foreach (var card in Cards)
            if (card.GetComponent<CardBase>().Key == _key)
            {
                mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
                Cards.Remove(card);
                cardSelector.RemoveCard(card);
                Destroy(card);
            }
    }
    #endregion

    private void HidePanels()
    {
        foreach (var panel in CategoriesPanels)
            panel.SetActive(false);

        categoriesSelector.SetActive(false);
    }

    private void BindBtns()
    {
        /// Bind mode btns
        for (int i = 0; i < gameModes.Length; i++)
        {
            var btn = gameModes[i];
            var maxCardCount = 2 + 2 * i;

            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(categoriesSelector);
                cardSelector.SetMaxCard(maxCardCount);
            });
        }
    }

    private void BindCategoryBtn(int i)
    {
        var btn = CategoryCards[i].GetSelectBtn();
        var panel = CategoriesPanels[i];

        btn.onClick.AddListener(() =>
        {
            transitionController.ActivatePanel(panel);
            transitionController.AddEventToReturnBtn(() =>
            {
                cardSelector.UnselectAll();
            });
        });
    }
}
