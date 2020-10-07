using UnityEngine;

public class LibraryCardSelector : MonoBehaviour
{
    private CategoryManager categoryManager;

    public void Initialize()
    {
        categoryManager = FindObjectOfType<CategoryManager>();

        Signals.SelectCardFromLibrary.AddListener(SelectedCard);
    }

    private void SelectedCard(string cardKey)
    {
        categoryManager.AddCardFromLibrary(cardKey);
    }
}
