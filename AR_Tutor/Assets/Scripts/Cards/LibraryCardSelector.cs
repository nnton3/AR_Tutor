using UnityEngine;
using UnityEngine.Events;

public class UnityStringEvent : UnityEvent<string> { }

public class LibraryCardSelector : MonoBehaviour
{
    public UnityStringEvent CardSelectedEvent = new UnityStringEvent();
    private LibraryUIControl libraryUI;
    private CategoryManager categoryManager;
    private MenuTransitionController transitionController;

    public void Initialize()
    {
        libraryUI = FindObjectOfType<LibraryUIControl>();
        categoryManager = FindObjectOfType<CategoryManager>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        CardSelectedEvent.AddListener(SelectedCardIsValid);
    }

    private void SelectedCardIsValid(string cardKey)
    {
        if (categoryManager.CardIsValid(cardKey))
        {
            Debug.Log("Category contain this card");
            libraryUI.BindCardsForSelect();
        }
        else
        {
            categoryManager.AddCard(cardKey);
            transitionController.ReturnToBack(2);
        }
    }
}
