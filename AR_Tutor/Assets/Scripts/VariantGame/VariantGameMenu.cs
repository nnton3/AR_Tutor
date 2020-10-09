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
        {
            CategoryData data = new CategoryData();
            if (!categoryStorage.VariantCategories.ContainsKey(categoryKey)) continue;
            data = categoryStorage.VariantCategories[categoryKey];

            var obj = Instantiate(categoryCardPref, categoriesSelector.transform);
            var categoryPanel = Instantiate(categoryPanelPref, VariantGameParent.transform);
            CategoriesPanels.Add(categoryPanel);

            var initializer = obj.GetComponent<CategoryInitializer>();
            initializer.Initialize(GameName.Variant, categoryKey, data);
            CategoryCards.Add(initializer);

            ConfigurateCards(data, categoryPanel, categoryKey);

            var editableElem = obj.GetComponent<EditableElement>();
            editableElem.Visible = (data.visible);
            Debug.Log(data.visible);
            editableElem.ConfigurateElement(mainMenu.Mode);
            mainMenu.AddEditableElement(editableElem);
        }

        CreateAddCategoryBtn();
    }

    private void ConfigurateCards(CategoryData _categoryData, GameObject _categoryPanel, string _categoryKey)
    {
        if (cardStorage == null) return;
        string debug = "";
        if (_categoryData.cardKeys != null)
            if (_categoryData.cardKeys.Count > 0)
                foreach (var key in _categoryData.cardKeys)
                {
                    debug += $"Try to add card {key}\n";
                    if (!cardStorage.cards.ContainsKey(key))
                    {
                        Debug.Log($"Key {key} not found in card storage");
                        continue;
                    }

                    GameObject cardObj = AddCardInMenu(_categoryPanel, _categoryKey, key);
                    var editableElem = cardObj.GetComponent<EditableElement>();

                    int index = -1;
                    bool visible = true;
                    index = _categoryData.cardKeys.IndexOf(key);
                    if (index >= 0)
                        if (_categoryData.cardsVisible != null)
                            if (_categoryData.cardsVisible.Count - 1 >= index)
                                visible = _categoryData.cardsVisible[index];
                
                    editableElem.Visible = visible;
                    editableElem.ConfigurateElement(mainMenu.Mode);
                    mainMenu.AddEditableElement(editableElem);
                }

        CreateAddCardBtn(_categoryPanel, _categoryKey);
    }

    private void AddNewCategory(string _categoryKey)
    {
        var game = categoryStorage.Categories[_categoryKey].game;
        if ((GameName)game != GameName.Variant) return;

        CategoryData data = new CategoryData();
        if (!categoryStorage.VariantCategories.ContainsKey(_categoryKey)) return;
        data = categoryStorage.VariantCategories[_categoryKey];

        var obj = Instantiate(categoryCardPref, categoriesSelector.transform);
        var categoryPanel = Instantiate(categoryPanelPref, VariantGameParent.transform);
        CategoriesPanels.Add(categoryPanel);
        categoryPanel.SetActive(false);

        var initializer = obj.GetComponent<CategoryInitializer>();
        initializer.Initialize(GameName.Variant, _categoryKey, data);
        CategoryCards.Add(initializer);

        var editableElem = obj.GetComponent<EditableElement>();
        editableElem.Visible = (data.visible);
        editableElem.ConfigurateElement(mainMenu.Mode);
        mainMenu.AddEditableElement(editableElem);

        BindCategoryBtn(CategoryCards.Count - 1);

        CreateAddCardBtn(categoryPanel, _categoryKey);
    }

    public void AddNewCard(string _categoryKey, string _key)
    {
        var game = categoryStorage.Categories[_categoryKey].game;
        if ((GameName)game != GameName.Variant) return;

        var targetCard = CategoryCards.Find((categoryObj) => categoryObj.GetComponent<CategoryInitializer>().categoryKey == _categoryKey);
        var index = CategoryCards.IndexOf(targetCard);
        GameObject cardObj = AddCardInMenu(CategoriesPanels[index], _categoryKey, _key);
        
        var editableElem = cardObj.GetComponent<EditableElement>();
        editableElem.ConfigurateElement(mainMenu.Mode);
        mainMenu.AddEditableElement(editableElem);
        cardSelector.AddCard(cardObj);
    }

    private GameObject AddCardInMenu(GameObject _categoryPanel, string _categoryKey, string _cardKey)
    {
        var cardData = cardStorage.cards[_cardKey];
        GameObject cardObj;

        cardObj = (!cardData.IsCustom) ? 
            Instantiate(cardPref, _categoryPanel.transform) : Instantiate(customCardPref, _categoryPanel.transform);

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardBase>();
        initializer.Initialize(GameName.Variant, _categoryKey, _cardKey, cardData);
        return cardObj;
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
        var instance = Instantiate(addCardBtnPref, _parent.transform);
        var editableElem = instance.GetComponent<EditableElement>();
        mainMenu.AddEditableElement(editableElem);
        var init = instance.GetComponent<CardBase>();
        init.Initialize(GameName.Variant, _categoryKey, null, new CardData());
        var btn = init.GetSelectBtn();
        btn.onClick.AddListener(() => _action());
    }
    #endregion

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

        Debug.Log("Variant game updated");
    }

    public void DeleteCategory(GameName _game, string _categoryKey)
    {
        var game = categoryStorage.Categories[_categoryKey].game;
        if ((GameName)game != GameName.Variant) return;

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
        var game = categoryStorage.Categories[_categoryKey].game;
        if ((GameName)game != GameName.Variant) return;

        foreach (var card in Cards)
            if (card.GetComponent<CardBase>().Key == _key)
            {
                mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
                Cards.Remove(card);
                cardSelector.RemoveCard(card);
                Destroy(card);
            }
    }

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
                transitionController.ActivatePanel(new GameObject[] { categoriesSelector });
                cardSelector.SetMaxCard(maxCardCount);
            });
        }

        /// Bind categories btns
        for (int i = 0; i < CategoryCards.Count; i++)
            BindCategoryBtn(i);
    }

    private void BindCategoryBtn(int i)
    {
        var btn = CategoryCards[i].GetSelectBtn();
        var panel = CategoriesPanels[i];

        btn.onClick.AddListener(() =>
        {
            transitionController.ActivatePanel(new GameObject[] { panel });
            transitionController.AddEventToReturnBtn(() => cardSelector.UnselectAll());
        });
    }
}
