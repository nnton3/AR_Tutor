using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    private enum SelectionMethod { AddNew, FromLibrary}

    #region Variables
    private MenuTransitionController transitionController;
    private VariantGameMenu variantMenu;
    private PatientDataManager patientDataManager;
    private CardStorage storage;
    private LibraryUIControl libraryControl;
    private CardCreator cardCreator;

    private GameName gameName;
    private int categoryIndex;
    private bool methodSelected;
    private SelectionMethod method;

    [SerializeField] private GameObject addMethodSeletor, createCardPanel, libraryPanel;
    [SerializeField] private Button selectCardFromLibraryBtn, createNewCardBtn;

    [HideInInspector] public AddCardEventUnityEvent AddCardEvent = new AddCardEventUnityEvent();
    #endregion

    private void Awake()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        variantMenu = FindObjectOfType<VariantGameMenu>();
        patientDataManager = FindObjectOfType<PatientDataManager>();
        storage = FindObjectOfType<CardStorage>();
        libraryControl = FindObjectOfType<LibraryUIControl>();
        cardCreator = FindObjectOfType<CardCreator>();

        BindBtn();
    }

    private void BindBtn()
    {
        selectCardFromLibraryBtn.onClick.AddListener(() =>
        {
            method = SelectionMethod.FromLibrary;
            methodSelected = true;
        });

        createNewCardBtn.onClick.AddListener(() =>
        {
            method = SelectionMethod.AddNew;
            methodSelected = true;
        });
    }

    #region Add card in category
    public void SelectAddMethod(GameName game, int _categoryIndex)
    {
        gameName = game;
        categoryIndex = _categoryIndex;
        StartCoroutine(AddCardToCategoryRoutine());
    }

    private IEnumerator AddCardToCategoryRoutine()
    {
        transitionController.ActivatePanel(new GameObject[] { addMethodSeletor });
        transitionController.AddEventToReturnBtn(() => StopCoroutine(AddCardToCategoryRoutine()));

        while (!methodSelected)
            yield return null;

        switch (method)
        {
            case SelectionMethod.AddNew:
                OpenCreateCardPanel();
                break;
            case SelectionMethod.FromLibrary:
                OpenLibrary();
                break;
            default:
                break;
        }
    }

    private void OpenCreateCardPanel()
    {
        transitionController.ActivatePanel(new GameObject[] { createCardPanel });
    }

    private void OpenLibrary()
    {
        libraryControl.BindCardsForSelect();
        transitionController.ActivatePanel(new GameObject[] { libraryPanel });
        transitionController.AddEventToReturnBtn(() => libraryControl.ClearBtnsEvents());
    }

    public void AddCard(string _cardKey)
    {
        AddCardEvent.Invoke(gameName, categoryIndex, _cardKey);
        methodSelected = false;
    }

    private void UpdateCardImage(string _cardKey, Sprite _cardImg)
    {
        storage.UpdateCustomCardImage(_cardKey, _cardImg);
        UpdateCardImgInMenu(_cardKey, _cardImg);
    }

    private void UpdateCardImgInMenu(string _cardKey, Sprite _cardImg)
    {
        variantMenu.UpdateCardImg(_cardKey, _cardImg);
        libraryControl.UpdateCardImg(_cardKey, _cardImg);
    }

    public bool CardIsValid(string _key)
    {
        return patientDataManager.CardExists(gameName, categoryIndex, _key);
    }
    #endregion

    public void SetUpNewImage(GameName _game,int _categoryIndex , string _cardKey)
    {
        gameName = _game;
        categoryIndex = _categoryIndex;

        var cardData = storage.cards[_cardKey];

        Sprite sprite = null;

        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false);

                if (texture.width < texture.height)
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.width), Vector2.zero);
                else
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.height, texture.height), Vector2.zero);

                if (cardData.IsCustom)
                    UpdateCardImage(_cardKey, sprite);
                else
                    SetUpImgToBaseCard(_game, _categoryIndex, _cardKey, sprite);
            }
        });
    }

    private void SetUpImgToBaseCard(GameName _game, int _categoryIndex, string _cardKey, Sprite sprite)
    {
        cardCreator.CreateCard(_cardKey, sprite);
        patientDataManager.DeleteCardFromCategory(gameName, categoryIndex, _cardKey);
        /// Delete old card from menu
        switch (_game)
        {
            case GameName.Variant:
                variantMenu.DeleteCard(_categoryIndex, _cardKey);
                break;
            default:
                break;
        }
    }

    private void OnDestroy() { AddCardEvent.RemoveAllListeners(); }
}

public class AddCardEventUnityEvent: UnityEvent<GameName, int, string> { }
