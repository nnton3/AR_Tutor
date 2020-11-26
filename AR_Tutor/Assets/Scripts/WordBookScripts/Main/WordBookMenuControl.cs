using UnityEngine;

public class WordBookMenuControl : GameMenu, IEditableElement
{
    [SerializeField] private GameObject cardContent, mainPanel, animalsBtn;
    [SerializeField] private WordbookGridCalculater gridControl;
    [SerializeField] private VerticalContentMover categoryContentMover;
    private WordBookCardSelector wordbookSelector;

    public override void Initialize()
    {
        gameName = GameName.WordBook;
        var selector = FindObjectOfType<WordBookCardSelector>();
        cardSelector = selector;
        wordbookSelector = selector;
        base.Initialize();

        if (CategoriesPanels.Count > 0)
        {
            CategoriesPanels[0].SetActive(true);
            wordbookSelector.SetCurrentCategory(CategoryCards[0].CategoryKey, CategoriesPanels[0]);
        }

        categoryContentMover.GetComponent<ElementsManagment>().Initialize();

        mainMenu.AddEditableElement(this);

        Signals.ReturnToMainMenuEvent.AddListener(ReturnToMenuHandler);
    }

    private void Start()
    {
        gridControl.CalculateCategoryGrid();
        categoryContentMover.UpdateMoveStep(gridControl.PanelHeight);
        CalculateCategoryPanelRect();

        foreach (var panel in CategoriesPanels)
        {
            panel.GetComponent<WordbookCardPanelControl>().CalculateGrid();
            panel.GetComponent<WordbookCardPanelControl>().UpdateGrid();
        }
    }

    protected override void CalculateCategoryPanelRect()
    {
        gridControl.CalculateContentSize(UIInstruments.GetVisibleElements(categoryParent));
        categoryContentMover.CalculateMinPos();
    }

    protected override void CalculateCardPanelRect(GameObject _panel)
    {
        _panel.GetComponent<WordbookCardPanelControl>().UpdateGrid();
    }

    public override void ResetGame()
    {
        mainPanel.SetActive(false);
        animalsBtn.SetActive(false);
        cardContent.SetActive(false);
        HidePanels();
    }

    private void ReturnToMenuHandler()
    {
        wordbookSelector.CloseContent();
        mainPanel.SetActive(false);
        animalsBtn.SetActive(false);
        Signals.StopPlayAudioEvent.Invoke();
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var btn = CategoryCards[_categoryIndex].GetSelectBtn();
        var panel = CategoriesPanels[_categoryIndex];
        var categoryKey = CategoryCards[_categoryIndex].CategoryKey;

        btn.onClick.AddListener(() =>
        {
            HidePanels();
            cardContent.SetActive(false);
            wordbookSelector.SetCurrentCategory(categoryKey, panel);
            panel.SetActive(true);

            if (categoryKey == "default_2_category10" ||
                categoryKey == "default_2_category11" ||
                categoryKey == "default_2_category12")
            {
                animalsBtn.SetActive(true);
            }
        });
    }

    public void ConfigurateElement(MenuMode _mode)
    {
        gridControl.CalculateContentSize(UIInstruments.GetVisibleElements(categoryParent));
        foreach (var panel in CategoriesPanels)
            CalculateCardPanelRect(panel);
    }
}
