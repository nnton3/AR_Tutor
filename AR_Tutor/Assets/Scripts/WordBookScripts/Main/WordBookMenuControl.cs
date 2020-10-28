using UnityEngine;
using UnityEngine.UI;

public class WordBookMenuControl : GameMenu
{
    private WordBookCardSelector wordbookSelector;

    public override void Initialize()
    {
        gameName = GameName.WordBook;
        var selector = FindObjectOfType<WordBookCardSelector>();
        cardSelector = selector;
        wordbookSelector = selector;
        base.Initialize();
        CategoriesPanels[0].SetActive(true);
        wordbookSelector.SetCurrentCategory(CategoryCards[0].categoryKey, CategoriesPanels[0]);
    }

    protected override void AddNewCategory(string _categoryKey)
    {
        base.AddNewCategory(_categoryKey);
        UIInstruments.GetSizeForVerticalGroup(
            categoryParent.GetComponent<VerticalLayoutGroup>(),
            CategoryCards.Count,
            categoryCardPref.GetComponent<RectTransform>().sizeDelta.y * categoryCardPref.GetComponent<RectTransform>().localScale.y);
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var btn = CategoryCards[_categoryIndex].GetSelectBtn();
        var panel = CategoriesPanels[_categoryIndex];
        var categoryKey = CategoryCards[_categoryIndex].categoryKey;

        btn.onClick.AddListener(() =>
        {
            HidePanels();
            wordbookSelector.SetCurrentCategory(categoryKey, panel);
            panel.SetActive(true);
        });
    }
}
