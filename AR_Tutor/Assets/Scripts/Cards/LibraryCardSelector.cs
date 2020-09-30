using UnityEngine;
using UnityEngine.Events;

public class UnityStringEvent : UnityEvent<string> { }

public class LibraryCardSelector : MonoBehaviour
{
    public UnityStringEvent CardSelectedEvent = new UnityStringEvent();
    private LibraryUIControl libraryUI;
    private CategoryManager categoryManager;

    public void Initialize()
    {
        libraryUI = FindObjectOfType<LibraryUIControl>();
        categoryManager = FindObjectOfType<CategoryManager>();

        CardSelectedEvent.AddListener(SelectedCardIsValid);
    }

    private void SelectedCardIsValid(string cardKey)
    {
        categoryManager.AddCardFromLibrary(cardKey);
    }
}
