﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMenu : MonoBehaviour
{
    #region Variables
    protected GameName gameName;
    protected PatientDataManager patientDataManager;
    protected CategoryStorage categoryStorage;
    protected CardStorage cardStorage;
    protected MainMenuUIControl mainMenu;
    protected CategoryManager categoryManager;
    protected IManageCards cardSelector;

    [SerializeField] protected Transform categoryParent, panelParent;
    [SerializeField] protected GameObject categoryCardPref, categoryPanelPref, addCardBtnPref, cardPref, customCardPref;
    protected List<CategoryInitializer> CategoryCards = new List<CategoryInitializer>();
    protected List<GameObject> CategoriesPanels = new List<GameObject>(), Cards = new List<GameObject>();
    #endregion

    public virtual void Initialize()
    {
        patientDataManager = FindObjectOfType<PatientDataManager>();
        categoryManager = FindObjectOfType<CategoryManager>();
        categoryStorage = FindObjectOfType<CategoryStorage>();
        cardStorage = FindObjectOfType<CardStorage>();
        mainMenu = FindObjectOfType<MainMenuUIControl>();

        ConfigurateCategories();
        cardSelector.Initialize(Cards);

        Signals.AddCategoryEvent.AddListener(AddNewCategory);
        Signals.AddCardEvent.AddListener(AddNewCard);
        Signals.DeleteCategoryFromGame.AddListener(DeleteCategory);
        Signals.DeleteCardFromCategory.AddListener(DeleteCard);
    }

    #region Configurate
    protected void ConfigurateCategories()
    {
        foreach (var categoryKey in patientDataManager.PatientData.CategoriesKeys)
            AddNewCategory(categoryKey);

        CreateAddCategoryBtn();
    }

    protected virtual void ConfigurateCards(CategoryData _categoryData, GameObject _categoryPanel, string _categoryKey)
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
    #endregion

    #region Add content
    protected virtual void AddNewCategory(string _categoryKey)
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
    }

    public virtual void AddNewCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var targetCard = CategoryCards.Find((categoryObj) => categoryObj.GetComponent<CategoryInitializer>().categoryKey == _categoryKey);
        var index = CategoryCards.IndexOf(targetCard);
        GameObject cardObj = AddCardInMenu(CategoriesPanels[index], _categoryKey, _key);

        InitializeEditableElement(cardObj);
        cardSelector.AddCard(cardObj);
    }

    protected virtual GameObject AddCardInMenu(GameObject _categoryPanel, string _categoryKey, string _cardKey)
    {
        var cardData = cardStorage.cards[_cardKey];
        GameObject cardObj;

        var parent = _categoryPanel.transform.Find("Mask/Content");
        cardObj = (!cardData.IsCustom) ? Instantiate(cardPref, parent) : Instantiate(customCardPref, parent);

        Cards.Add(cardObj);
        var initializer = cardObj.GetComponent<CardBase>();
        initializer.Initialize(GameName.Variant, _categoryKey, _cardKey, cardData);
        return cardObj;
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

    protected virtual void DeleteCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        foreach (var card in Cards)
            if (card.GetComponent<CardBase>().Key == _key)
            {
                mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
                Cards.Remove(card);
                //cardSelector.RemoveCard(card);
                Destroy(card);
            }
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

    #region Add btn
    protected void CreateAddCardBtn(GameObject _categoryPanel, string _categoryKey)
    {
        CreateAddBtn(_categoryPanel.transform.Find("Mask/Content"), () => categoryManager.SelectAddMethod(GameName.Variant, _categoryKey), _categoryKey);
    }

    protected void CreateAddCategoryBtn()
    {
        CreateAddBtn(categoryParent, () => categoryManager.SelectAddMethod(GameName.Variant));
    }

    protected void CreateAddBtn(Transform _parent, UnityAction _action, string _categoryKey = null)
    {
        var instance = Instantiate(addCardBtnPref, _parent);
        var editableElem = instance.GetComponent<EditableElement>();
        mainMenu.AddEditableElement(editableElem);
        var initialiser = instance.GetComponent<CardBase>();
        initialiser.Initialize(GameName.Variant, _categoryKey, null, new CardData());
        var btn = initialiser.GetSelectBtn();
        btn.onClick.AddListener(() => _action());
    }
    #endregion

    protected virtual void BindCategoryBtn(int _categoryIndex) { }

    #region Helpful
    protected bool IsCategoryForThisGame(string _categoryKey)
    {
        var game = categoryStorage.Categories[_categoryKey].game;
        return (GameName)game == gameName;
    }

    protected void InitializeEditableElement(GameObject _obj, bool visible = true)
    {
        var editableElem = _obj.GetComponent<EditableElement>();
        editableElem.Visible = visible;
        editableElem.ConfigurateElement(mainMenu.Mode);
        mainMenu.AddEditableElement(editableElem);
    }

    protected virtual void HidePanels()
    {
        foreach (var panel in CategoriesPanels)
            panel.SetActive(false);
    }
    #endregion
}
