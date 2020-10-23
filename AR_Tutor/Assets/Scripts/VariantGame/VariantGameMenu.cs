using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VariantGameMenu : GameMenu
{
    #region Variables
    private MenuTransitionController transitionController;
    private VariantCardSelector variantSelector;

    [SerializeField] private GameObject categorySelector;
    [SerializeField] private Button[] gameModes = new Button[] { };
    [SerializeField] private Button startGameBtn;
    #endregion

    public override void Initialize()
    {
        gameName = GameName.Variant;
        VariantCardSelector selector = FindObjectOfType<VariantCardSelector>();
        cardSelector = selector;
        variantSelector = selector;
        transitionController = FindObjectOfType<MenuTransitionController>();
        base.Initialize();
        
        BindBtns();
        HidePanels();
    }

    #region Add content
    public override void AddNewCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var targetCard = CategoryCards.Find((categoryObj) => categoryObj.GetComponent<CategoryInitializer>().categoryKey == _categoryKey);
        var index = CategoryCards.IndexOf(targetCard);
        GameObject cardObj = AddCardInMenu(CategoriesPanels[index], _categoryKey, _key);

        InitializeEditableElement(cardObj);
        cardSelector.AddCard(cardObj);
    }
    #endregion

    #region Delete content
    protected override void DeleteCard(string _categoryKey, string _key)
    {
        if (!IsCategoryForThisGame(_categoryKey)) return;

        var targetCards = Cards.FindAll((card) => card.GetComponent<CardBase>().Key == _key);

        foreach (var card in targetCards)
        {
            mainMenu.DeleteEditableElement(card.GetComponent<EditableElement>());
            Cards.Remove(card);
            cardSelector.RemoveCard(card);
            Destroy(card);
        }
    }
    #endregion

    protected override void HidePanels()
    {
        base.HidePanels();
        categorySelector.gameObject.SetActive(false);
    }

    private void BindBtns()
    {
        for (int i = 0; i < gameModes.Length; i++)
        {
            var btn = gameModes[i];
            var maxCardCount = 2 + 2 * i;

            btn.onClick.AddListener(() =>
            {
                transitionController.ActivatePanel(categorySelector);
                variantSelector.SetMaxCard(maxCardCount);
            });
        }
    }

    protected override void BindCategoryBtn(int index)
    {
        var btn = CategoryCards[index].GetSelectBtn();
        var panel = CategoriesPanels[index];

        btn.onClick.AddListener(() =>
        {
            transitionController.ActivatePanel(panel);
            startGameBtn.gameObject.SetActive(true);

            transitionController.AddEventToReturnBtn(() =>
            {
                startGameBtn.gameObject.SetActive(false);
                variantSelector.UnselectAll();
            });
        });
    }
}
