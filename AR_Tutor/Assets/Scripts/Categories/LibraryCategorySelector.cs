using UnityEngine;

public class LibraryCategorySelector : MonoBehaviour
{
    private CategoryManager categoryManager;

    public void Initialize()
    {
        categoryManager = FindObjectOfType<CategoryManager>();

        Signals.SelectCategoryFromLibrary.AddListener(SelectCategory);
    }

    private void SelectCategory(string _categoryKey)
    {
        categoryManager.AddCategoryFromlibrary(_categoryKey);
    }
}
