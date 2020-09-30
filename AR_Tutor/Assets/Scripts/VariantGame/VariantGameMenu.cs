using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantGameMenu : MonoBehaviour, IGameMenu
{
    #region Variables
    private MainMenuUIControl uiControl;
    private CardStorage cardStorage;
    private CategoryManager categoryManager;
    private MainMenuUIControl mainMenu;
    private VariantCardSelector cardSelector;
    private MenuTransitionController transitionController;

    private VariantCategoryData[] config;
    [SerializeField] private GameObject categoryCardPref, categoryPanelPref, cardPref, customCardPref, addCardBtnPref, categoriesSelector, VariantGameParent;
    [SerializeField] private List<Button> CategoriesBtns = new List<Button>();
    [SerializeField] private List<GameObject> CategoriesPanels = new List<GameObject>(), Cards = new List<GameObject>();
    [SerializeField] private Button[] gameModes = new Button[] { };
    #endregion

    public void Initialize()
    {
        uiControl = GetComponent<MainMenuUIControl>();
        cardStorage = FindObjectOfType<CardStorage>();
        categoryManager = FindObjectOfType<CategoryManager>();
        mainMenu = FindObjectOfType<MainMenuUIControl>();
        cardSelector = FindObjectOfType<VariantCardSelector>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        categoryManager.AddCardEvent.AddListener(AddNewCard);

        if (FindObjectOfType<PatientDataManager>().GetPatientData() != null)
        {
            config = FindObjectOfType<PatientDataManager>().GetPatientData().Value.VariantGameConfig;

            if (config != null)
                ConfigurateCategories();
        }

        BindBtns();
        HidePanels();

        cardSelector.Initialize(Cards);
    }

    private void ConfigurateCategories()
    {
        for (int i = 0; i < config.Length; i++)
        {
            var category = Instantiate(categoryCardPref, categoriesSelector.transform);
            var initializer = category.GetComponent<CategoryInitializer>();
            initializer.Initialize(GameName.Variant, i, config[i].categoryName);
            CategoriesBtns.Add(initializer.GetSelectBtn());

            var categoryPanel = Instantiate(categoryPanelPref, VariantGameParent.transform);
            CategoriesPanels.Add(categoryPanel);

            ConfigurateCards(config[i], categoryPanel, i);

            category.GetComponent<EditableElement>().visible = (config[i].visible);
        }
    }

    private void ConfigurateCards(VariantCategoryData categoryData, GameObject categoryPanel, int categoryIndex)
    {
        if (cardStorage == null) return;

        if (categoryData.cardKeys.Count > 0)
            foreach (var key in categoryData.cardKeys)
            {
                GameObject cardObj = AddCardInMenu(categoryPanel, categoryIndex, key);
                var editableElem = cardObj.GetComponent<EditableElement>();
                editableElem.visible = categoryData.cardVisibleValue[categoryData.cardKeys.IndexOf(key)];
                mainMenu.AddEditableElement(editableElem);
            }

        CreateAddCardBtn(categoryPanel, categoryIndex);
    }

    private void CreateAddCardBtn(GameObject categoryPanel, int categoryIndex)
    {
        var addCardBtnInstance = Instantiate(addCardBtnPref, categoryPanel.transform);
        var init = addCardBtnInstance.GetComponent<CardInitializer>();
        init.Initialize(GameName.Variant, categoryIndex, null, new CardData());
        var btn = init.GetSelectBtn();
        btn.onClick.AddListener(() =>
        {
            categoryManager.SelectAddMethod(GameName.Variant, categoryIndex);
        });
    }

    private GameObject AddCardInMenu(GameObject categoryPanel, int categoryIndex, string key)
    {
        var cardData = cardStorage.cards[key];
        GameObject cardObj;

        cardObj = (!cardData.IsCustom) ? 
            Instantiate(cardPref, categoryPanel.transform) : Instantiate(customCardPref, categoryPanel.transform);

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardInitializer>();
        initializer.Initialize(GameName.Variant, categoryIndex, key, cardData);
        var cardBtn = initializer.GetSelectBtn();
        cardBtn.onClick.AddListener(() => Debug.Log(cardData.Title));
        return cardObj;
    }

    public void AddNewCard(GameName _game, int _categoryIndex, string _key)
    {
        if (_game != GameName.Variant) return;

        GameObject cardObj = AddCardInMenu(CategoriesPanels[_categoryIndex], _categoryIndex, _key);
        
        var editableElem = cardObj.GetComponent<EditableElement>();
        editableElem.visible = true;
        editableElem.ConfigurateElement(MenuMode.GameSelection);
        mainMenu.AddEditableElement(editableElem);
        cardSelector.AddCard(cardObj);
    }

    public void UpdateCardImg(string _cardKey, Sprite _cardImg)
    {
        var validCards = Cards.FindAll((card) => card.GetComponent<CardInitializer>().key == _cardKey);
        foreach (var item in validCards)
            item.GetComponent<CardInitializer>().UpdateImg(_cardImg);

        Debug.Log("Variant game updated");
    }

    public void DeleteCard(int _categoryIndex, string _key)
    {
         foreach (var card in Cards)
            if (card.GetComponent<CardInitializer>().key == _key)
            {
                mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
                Cards.Remove(card);
                cardSelector.RemoveCard(card);
                Destroy(card.gameObject);
                return;
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
        uiControl.BindPanelBtns(gameModes, new GameObject[] { categoriesSelector, categoriesSelector, categoriesSelector });
        uiControl.BindPanelBtns(
            CategoriesBtns.ToArray(),
            CategoriesPanels.ToArray(),
            () => transitionController.AddEventToReturnBtn(() => cardSelector.UnselectAll()));

        gameModes[0].onClick.AddListener(() => cardSelector.SetMaxCard(2));
        gameModes[1].onClick.AddListener(() => cardSelector.SetMaxCard(4));
        gameModes[2].onClick.AddListener(() => cardSelector.SetMaxCard(6));
    }
}
