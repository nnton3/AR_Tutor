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

    protected override void AddNewCategory(string _categoryKey)
    {
        base.AddNewCategory(_categoryKey);
        UIInstruments.GetSizeForGrid(categoryParent.GetComponent<GridLayoutGroup>(), CategoryCards.Count);
    }

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
            if (mainMenu.Mode == MenuMode.GameSelection)
                startGameBtn.gameObject.SetActive(true);

            transitionController.AddEventToReturnBtn(() =>
            {
                startGameBtn.gameObject.SetActive(false);
                variantSelector.UnselectAll();
            });
        });
    }
}
