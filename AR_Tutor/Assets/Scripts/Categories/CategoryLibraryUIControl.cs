using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CategoryLibraryUIControl : MonoBehaviour
{
    [SerializeField] private GameObject libraryCategoryPref, libraryPanel;
    [SerializeField] private GridLayoutGroup grid;
    private CategoryStorage categoryStorage;
    private List<LibraryCategoryInitializer> cards = new List<LibraryCategoryInitializer>();

    public void Initialize()
    {
        categoryStorage = FindObjectOfType<CategoryStorage>();
        FindObjectOfType<LibraryCategorySelector>().Initialize();

        Signals.AddCategoryEvent.AddListener(AddCategory);
    }

    public void FillLibrary(GameName game)
    {
        switch (game)
        {
            case GameName.Variant:
                FillLibrary(categoryStorage.VariantCategories);
                break;
            case GameName.Buttons:
                FillLibrary(categoryStorage.ButtonsCategories);
                break;
            case GameName.WordBook:
                FillLibrary(categoryStorage.WordBookCategories);
                break;
            case GameName.WordComposing:
                FillLibrary(categoryStorage.WordComposingCategories);
                break;
            default:
                break;
        }
    }

    private void FillLibrary(Dictionary<string, CategoryData> _targetCategories)
    {
        Clearlibrary();

        foreach (var key in _targetCategories.Keys)
        {
            if (!_targetCategories[key].IsCustom) continue;
            CreateCard(_targetCategories[key], key);
        }
    }

    private void CreateCard(CategoryData _data, string _categoryKey)
    {
        var instance = Instantiate(libraryCategoryPref, libraryPanel.transform);
        var initializer = instance.GetComponent<LibraryCategoryInitializer>();
        initializer.Initialize(_categoryKey, _data);
        cards.Add(initializer);
        UIInstruments.GetSizeForVerticalGrid(grid, cards.Count);
    }

    private void CreateCard(string _categoryKey)
    {
        var data = categoryStorage.Categories[_categoryKey];
        CreateCard(data, _categoryKey);
    }

    public void BindCardsForSelect()
    {
        Debug.Log("Bind cards");
        foreach (var card in cards)
            card.EnableSelectable();
    }

    public void ClearBtnsEvents()
    {
        Clearlibrary();
    }

    public void UpdateCardImg(string _categoryKey, Sprite _cardImg)
    {
        var target = cards.Find((card) => card.categoryKey == _categoryKey);
        target.UpdateImg(_cardImg);
    }

    private void Clearlibrary()
    {
        if (cards.Count == 0) return;
        foreach (var card in cards)
            Destroy(card.gameObject);

        cards.Clear();
    }

    private void AddCategory(string _categoryKey)
    {
        if (cards.Find((card) => card.categoryKey == _categoryKey))
        {
            Debug.Log("Library contain this card");
            return;
        }
        else CreateCard(_categoryKey);
    }
}
