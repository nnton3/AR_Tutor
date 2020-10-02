using System.Collections;
using UnityEngine;
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
    private Coroutine routine;

    [SerializeField] private GameObject addMethodSeletor, createCardPanel, libraryPanel;
    [SerializeField] private Button selectCardFromLibraryBtn, createNewCardBtn;
    #endregion

    private void Awake()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        variantMenu = FindObjectOfType<VariantGameMenu>();
        patientDataManager = FindObjectOfType<PatientDataManager>();
        storage = FindObjectOfType<CardStorage>();
        libraryControl = FindObjectOfType<LibraryUIControl>();
        cardCreator = FindObjectOfType<CardCreator>();

        Signals.SetImageEvent.AddListener(SetUpNewImage);
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
        transitionController.ActivatePanel(new GameObject[] { addMethodSeletor });
        StartSelectMethodRoutine();
    }

    private void StartSelectMethodRoutine()
    {
        routine = StartCoroutine(AddCardToCategoryRoutine());
        transitionController.AddEventToReturnBtn(() =>
        {
            StopCoroutine(routine);
            methodSelected = false;
        });
    }

    private IEnumerator AddCardToCategoryRoutine()
    {
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
        methodSelected = false;
        transitionController.ActivatePanel(new GameObject[] { createCardPanel });
        transitionController.AddEventToReturnBtn(() =>
        {
            cardCreator.ResetFields();
            StartSelectMethodRoutine();
        });
    }

    private void OpenLibrary()
    {
        libraryControl.BindCardsForSelect();
        methodSelected = false;
        transitionController.ActivatePanel(new GameObject[] { libraryPanel });
        transitionController.AddEventToReturnBtn(() =>
        {
            libraryControl.ClearBtnsEvents();
            StartSelectMethodRoutine();
        });
    }

    public void AddCard(string _cardKey)
    {
        Signals.AddCardEvent.Invoke(gameName, categoryIndex, _cardKey);
    }

    public void AddCardFromLibrary(string _cardKey)
    {
        if (CardIsValid(_cardKey))
        {
            Debug.Log("add card");
            AddCard(_cardKey);
            transitionController.ReturnToBack(2);
        }
        else
        {
            Debug.Log("card exist, select new");
            libraryControl.BindCardsForSelect();
        }
    }

    private bool CardIsValid(string _key)
    {
        return !patientDataManager.CardExists(gameName, categoryIndex, _key);
    }
    #endregion

    /// <summary>
    /// Установить для карточки новую картинку
    /// </summary>
    /// <param name="_game"></param>
    /// <param name="_categoryIndex"></param>
    /// <param name="_cardKey"></param>
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

                var size = (texture.width < texture.height) ? texture.width : texture.height;
                sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);

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
        Signals.DeleteCardFromCategory.Invoke(gameName, categoryIndex, _cardKey);
    }

    private void UpdateCardImage(string _cardKey, Sprite _cardImg)
    {
        /// Обновить картинку для карточки в библиотеке и сохранить изменения локально
        storage.UpdateCustomCardImage(_cardKey, _cardImg);

        /// Обновить картинку для карточки во всех меню
        libraryControl.UpdateCardImg(_cardKey, _cardImg);
        variantMenu.UpdateCardImg(_cardKey, _cardImg);
    }
}
