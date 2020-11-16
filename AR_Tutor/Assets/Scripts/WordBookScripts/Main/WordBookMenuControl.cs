using UnityEngine;
using UnityEngine.UI;

public class WordBookMenuControl : GameMenu
{
    [SerializeField] private GameObject audioBtn, cardContent, mainPanel, animalsBtn;
    private VerticalContentMover categoryContentMover;
    private WordBookCardSelector wordbookSelector;

    public override void Initialize()
    {
        gameName = GameName.WordBook;
        categoryContentMover = GetComponent<VerticalContentMover>();
        var selector = FindObjectOfType<WordBookCardSelector>();
        cardSelector = selector;
        wordbookSelector = selector;
        base.Initialize();

        if (CategoriesPanels.Count > 0)
        {
            CategoriesPanels[0].SetActive(true);
            wordbookSelector.SetCurrentCategory(CategoryCards[0].CategoryKey, CategoriesPanels[0]);
        }

        BindBtn();
    }

    protected override void CalculateCategoryPanelRect()
    {
        UIInstruments.GetSizeForVerticalGroup(
            categoryParent.GetComponent<VerticalLayoutGroup>(),
            GetVisibleCategoriesCount(),
            categoryCardPref.GetComponent<RectTransform>().sizeDelta.y * categoryCardPref.GetComponent<RectTransform>().localScale.y);
        categoryContentMover.CalculateMinPos();
    }

    protected override void CalculateCardPanelRect(GameObject _panel)
    {
        var contentPanel = _panel.transform.Find("Mask/Content");

        var visibleCategoryCount = 0;
        foreach (Transform card in contentPanel.transform)
            if (card.gameObject.activeSelf) visibleCategoryCount++;

        UIInstruments.GetSizeForVerticalGrid(contentPanel.GetComponent<GridLayoutGroup>(), visibleCategoryCount);
    }

    private void BindBtn()
    {
        returnToMenuBtn.onClick.AddListener(() =>
        {
            wordbookSelector.CloseContent();
            mainPanel.SetActive(false);
            animalsBtn.SetActive(false);
            returnToMenuBtn.gameObject.SetActive(false);
        });
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
                audioBtn.SetActive(true);
            }
        });
    }
}
