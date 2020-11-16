using UnityEngine;
using UnityEngine.UI;

public class VariantGameMenu : GameMenu
{
    #region Variables
    private VariantCardSelector variantSelector;
    private HorizontalContentMover contentMover;

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
        base.Initialize();
        
        BindBtns();
        HidePanels();
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

    protected override void CalculateCategoryPanelRect()
    {
        UIInstruments.GetSizeForHorizontalGrid(categoryParent.GetComponent<GridLayoutGroup>(), GetVisibleCategoriesCount());
        contentMover.CalculateMinPos();
    }

    protected override void CalculateCardPanelRect(GameObject _panel)
    {
        var contentPanel = _panel.transform.Find("Mask/Content");

        var visibleCategoryCount = 0;
        foreach(Transform child in contentPanel.transform)
            if (child.gameObject.activeSelf) visibleCategoryCount++;

        UIInstruments.GetSizeForHorizontalGrid(
            contentPanel.GetComponent<GridLayoutGroup>(),
            visibleCategoryCount);

        _panel.GetComponent<HorizontalContentMover>().CalculateMinPos();
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

        returnToMenuBtn.onClick.AddListener(ResetGame);
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
                categorySelector.SetActive(false);
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

    protected override void ResetGame()
    {
        modeSelector.SetActive(false);
        categorySelector.SetActive(false);
        startGameBtn.gameObject.SetActive(false);
        variantSelector.Reset();
        returnToMenuBtn.gameObject.SetActive(false);

        foreach (var panel in CategoriesPanels)
            panel.SetActive(false);
    }
}
