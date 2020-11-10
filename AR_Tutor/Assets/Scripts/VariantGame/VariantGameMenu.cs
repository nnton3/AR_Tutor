using UnityEngine;
using UnityEngine.UI;

public class VariantGameMenu : GameMenu
{
    #region Variables
    private MenuTransitionController transitionController;
    private VariantCardSelector variantSelector;
    private ContentMover contentMover;

    [SerializeField] private GameObject categorySelector, modeSelector;
    [SerializeField] private Button[] gameModes = new Button[] { };
    [SerializeField] private Button startGameBtn;
    #endregion

    public override void Initialize()
    {
        gameName = GameName.Variant;
        transitionController = FindObjectOfType<MenuTransitionController>();
        VariantCardSelector selector = FindObjectOfType<VariantCardSelector>();
        contentMover = GetComponent<ContentMover>();
        cardSelector = selector;
        variantSelector = selector;
        base.Initialize();
        
        BindBtns();
        HidePanels();
    }

    protected override void AddNewCategory(string _categoryKey)
    {
        base.AddNewCategory(_categoryKey);
        UIInstruments.GetSizeForHorizontalGrid(categoryParent.GetComponent<GridLayoutGroup>(), CategoryCards.Count);
        contentMover.CalculateMinPos();
    }

    protected override void HidePanels()
    {
        base.HidePanels();
        categorySelector.gameObject.SetActive(false);
    }

    protected override GameObject AddCardInMenu(GameObject _categoryPanel, string _categoryKey, string _cardKey)
    {
        return base.AddCardInMenu(_categoryPanel, _categoryKey, _cardKey);
    }

    protected override void CalculateCardPanelRect(GameObject _panel)
    {
        
        UIInstruments.GetSizeForHorizontalGrid(
            _panel.transform.Find("Mask/Content").GetComponent<GridLayoutGroup>(),
            _panel.transform.Find("Mask/Content").childCount);

        _panel.GetComponent<ContentMover>().CalculateMinPos();
    }

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
            if (MainMenuUIControl.Mode == MenuMode.Play)
            {
                variantSelector.SetTargetPanel(panel);
                startGameBtn.gameObject.SetActive(true);
            }
        });
    }

    protected override void InitializeCategoryPanel(GameObject categoryPanel)
    {
        var panelInit = categoryPanel.GetComponent<VariantCategoryPanelControl>();
        if (panelInit == null) return;

        panelInit.Initialize(() =>
        {
            startGameBtn.gameObject.SetActive(false);
            categoryPanel.SetActive(false);
            categorySelector.SetActive(true);
            variantSelector.UnselectAll();
        });
    }
}
