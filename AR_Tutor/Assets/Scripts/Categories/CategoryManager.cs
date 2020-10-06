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
    private CardStorage cardStorage;
    private CategoryStorage categoryStorage;
    private LibraryUIControl cardLibraryControl;
    private CardCreator cardCreator;

    private GameName gameName;
    private string categoryKey;
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
        cardStorage = FindObjectOfType<CardStorage>();
        cardLibraryControl = FindObjectOfType<LibraryUIControl>();
        cardCreator = FindObjectOfType<CardCreator>();

        Signals.SetImgForCardEvent.AddListener(SetUpNewImgForCard);
        Signals.SetImgForCategoryEvent.AddListener(SetUpImgForCategory);
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

    #region Add category in game
    public void SelectAddCategoryMethod(GameName _game)
    {
        Debug.Log("Select method to add category");
    }
    #endregion

    #region Add card in category
    public void SelectAddMethod(GameName game, string _categoryKey)
    {
        gameName = game;
        categoryKey = _categoryKey;
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
        cardLibraryControl.BindCardsForSelect();
        methodSelected = false;
        transitionController.ActivatePanel(new GameObject[] { libraryPanel });
        transitionController.AddEventToReturnBtn(() =>
        {
            cardLibraryControl.ClearBtnsEvents();
            StartSelectMethodRoutine();
        });
    }

    public void AddCard(string _cardKey)
    {
        Signals.AddCardEvent.Invoke(categoryKey, _cardKey);
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
            cardLibraryControl.BindCardsForSelect();
        }
    }

    private bool CardIsValid(string _key)
    {
        return !patientDataManager.CardExists(gameName, categoryKey, _key);
    }
    #endregion

    /// <summary>
    /// Установить для карточки новую картинку
    /// </summary>
    /// <param name="_game"></param>
    /// <param name="_categoryKey"></param>
    /// <param name="_cardKey"></param>
    public void SetUpNewImgForCard(GameName _game, string _categoryKey , string _cardKey)
    {
        gameName = _game;
        categoryKey = _categoryKey;

        var cardData = cardStorage.cards[_cardKey];

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
                    SetUpImgToBaseCard(_categoryKey, _cardKey, sprite);
            }
        });
    }

    private void SetUpImgForCategory(GameName _game, string _categoryKey)
    {
        gameName = _game;
        categoryKey = _categoryKey;

        var categoryData = categoryStorage.Categories[_categoryKey];

        Sprite sprite = null;

        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false);

                var size = (texture.width < texture.height) ? texture.width : texture.height;
                sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.zero);

                if (categoryData.IsCustom)
                    UpdateCategoryImage(_categoryKey, sprite);
                else
                    SetUpImgToBaseCategory(_game, _categoryKey, sprite);
            }
        });
    }

    private void UpdateCardImage(string _cardKey, Sprite _cardImg)
    {
        /// Обновить картинку для карточки в библиотеке и сохранить изменения локально
        cardStorage.UpdateCustomCardImage(_cardKey, _cardImg);

        /// Обновить картинку для карточки во всех меню
        cardLibraryControl.UpdateCardImg(_cardKey, _cardImg);
        variantMenu.UpdateCardImg(_cardKey, _cardImg);
    }

    private void SetUpImgToBaseCard(string _categoryKey, string _cardKey, Sprite sprite)
    {
        cardCreator.CreateCard(_cardKey, sprite);
        Signals.DeleteCardFromCategory.Invoke(categoryKey, _cardKey);
    }

    private void UpdateCategoryImage(string _categoryKey, Sprite _categoryImg)
    {
        categoryStorage.UpdateCustomCategoryImg(_categoryKey, _categoryImg);

        /// TODO 
        /// Добавить обновление библиотеки разделов
        /// Добавить обновление во всех играх, где используется
    }

    private void SetUpImgToBaseCategory(GameName _game, string _categoryKey, Sprite _categoryImg)
    {
        /// TODO 
        /// Доделать замену старого раздела на кастомный с новой картинкой
    }
}
