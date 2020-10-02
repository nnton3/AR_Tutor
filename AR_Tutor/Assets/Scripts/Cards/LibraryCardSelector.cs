using UnityEngine;

public class LibraryCardSelector : MonoBehaviour
{
    private CategoryManager categoryManager;

    public void Initialize()
    {
        categoryManager = FindObjectOfType<CategoryManager>();

        Signals.SelectCardFromLibrary.AddListener(SelectedCardIsValid);
    }

    private void SelectedCardIsValid(string cardKey)
    {
        categoryManager.AddCardFromLibrary(cardKey);
    }
}
