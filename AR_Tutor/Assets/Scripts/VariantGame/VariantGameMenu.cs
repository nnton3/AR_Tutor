using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantGameMenu : MonoBehaviour, IGameMenu
{
    #region Variables
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
        cardStorage = FindObjectOfType<CardStorage>();
        categoryManager = FindObjectOfType<CategoryManager>();
        mainMenu = FindObjectOfType<MainMenuUIControl>();
        cardSelector = FindObjectOfType<VariantCardSelector>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        var patientManager = FindObjectOfType<PatientDataManager>();
        if (patientManager.GetPatientData() != null)
        {
            config = patientManager.GetPatientData().Value.VariantGameConfig;

            if (config != null)
                ConfigurateCategories();
        }

        BindBtns();
        HidePanels();
        
        Signals.AddCardEvent.AddListener(AddNewCard);
        Signals.DeleteCardFromCategory.AddListener(DeleteCard);
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

            category.GetComponent<EditableElement>().Visible = (config[i].visible);
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
                editableElem.Visible = categoryData.cardVisibleValue[categoryData.cardKeys.IndexOf(key)];
                editableElem.ConfigurateElement(mainMenu.Mode);
                mainMenu.AddEditableElement(editableElem);
            }

        CreateAddCardBtn(categoryPanel, categoryIndex);
    }

    public void AddNewCard(GameName _game, int _categoryIndex, string _key)
    {
        if (_game != GameName.Variant) return;

        GameObject cardObj = AddCardInMenu(CategoriesPanels[_categoryIndex], _categoryIndex, _key);
        
        var editableElem = cardObj.GetComponent<EditableElement>();
        editableElem.ConfigurateElement(MenuMode.GameSelection);
        mainMenu.AddEditableElement(editableElem);
        cardSelector.AddCard(cardObj);
    }

    private GameObject AddCardInMenu(GameObject categoryPanel, int categoryIndex, string key)
    {
        var cardData = cardStorage.cards[key];
        GameObject cardObj;

        cardObj = (!cardData.IsCustom) ? 
            Instantiate(cardPref, categoryPanel.transform) : Instantiate(customCardPref, categoryPanel.transform);

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardBase>();
        initializer.Initialize(GameName.Variant, categoryIndex, key, cardData);
        return cardObj;
    }

    private void CreateAddCardBtn(GameObject categoryPanel, int categoryIndex)
    {
        var instance = Instantiate(addCardBtnPref, categoryPanel.transform);
        var editableElem = instance.GetComponent<EditableElement>();
        mainMenu.AddEditableElement(editableElem);
        var init = instance.GetComponent<CardBase>();
        init.Initialize(GameName.Variant, categoryIndex, null, new CardData());
        var btn = init.GetSelectBtn();
        btn.onClick.AddListener(() =>
        {
            categoryManager.SelectAddMethod(GameName.Variant, categoryIndex);
        });
    }

    public void UpdateCardImg(string _cardKey, Sprite _cardImg)
    {
        var validCards = Cards.FindAll((card) => card.GetComponent<CardBase>().Key == _cardKey);
        foreach (var item in validCards)
            item.GetComponent<CardBase>().UpdateImg(_cardImg);

        Debug.Log("Variant game updated");
    }

    public void DeleteCard(GameName _game, int _categoryIndex, string _key)
    {
        if (_game != GameName.Variant) return;

         foreach (var card in Cards)
            if (card.GetComponent<CardBase>().Key == _key)
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
        for (int i = 0; i < CategoriesBtns.Count; i++)
        {
            var btn = CategoriesBtns[i];
            var panel = CategoriesPanels[i];

            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(new GameObject[] { panel });
                transitionController.AddEventToReturnBtn(() => cardSelector.UnselectAll());
            });
        }
    }
}
