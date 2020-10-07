using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    private enum SelectionMethod { AddNew, FromLibrary}
    private enum AddedObj { Card, Category}

    #region Variables
    private MenuTransitionController transitionController;
    private VariantGameMenu variantMenu;
    private PatientDataManager patientDataManager;
    private CardStorage cardStorage;
    private CategoryStorage categoryStorage;
    private CardLibraryUIControl cardLibraryControl;
    private CategoryLibraryUIControl categoryLibraryControl;
    private CardCreator cardCreator;

    public GameName gameName { get; private set; }
    private string categoryKey;
    private bool methodSelected;
    private SelectionMethod method;
    private Coroutine routine;

    [SerializeField] private GameObject methodSelectorPanel, createCardPanel, createCategoryPanel, cardLibrary, categoryLibrary;
    [SerializeField] private Button selectCardFromLibraryBtn, createNewCardBtn;
    private GameObject createPanel, libraryPanel;
    private AddedObj currentAddedObj;
    #endregion

    private void Awake()
    {
        transitionController = FindObjectOfType<MenuTransitionController>();
        variantMenu = FindObjectOfType<VariantGameMenu>();
        patientDataManager = FindObjectOfType<PatientDataManager>();
        cardStorage = FindObjectOfType<CardStorage>();
        categoryLibraryControl = FindObjectOfType<CategoryLibraryUIControl>();
        cardLibraryControl = FindObjectOfType<CardLibraryUIControl>();
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

    #region Add content routine
    private void StartSelectMethodRoutine()
    {
        routine = StartCoroutine(SelectMethodRoutine());
        transitionController.AddEventToReturnBtn(() =>
        {
            StopCoroutine(routine);
            methodSelected = false;
        });
    }

    private IEnumerator SelectMethodRoutine()
    {
        while (!methodSelected)
            yield return null;

        switch (method)
        {
            case SelectionMethod.AddNew:
                OpenCreateCardPanel();
                break;
            case SelectionMethod.FromLibrary:
                if (currentAddedObj == AddedObj.Card)
                    OpenCardLibrary();
                else
                    OpenCategoryLibrary();
                break;
            default:
                break;
        }
    }

    private void OpenCreateCardPanel()
    {
        methodSelected = false;
        transitionController.ActivatePanel(new GameObject[] { createPanel });
        transitionController.AddEventToReturnBtn(() =>
        {
            cardCreator.ResetFields();
            StartSelectMethodRoutine();
        });
    }

    private void OpenCardLibrary()
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

    private void OpenCategoryLibrary()
    {
        categoryLibraryControl.BindCardsForSelect();
        methodSelected = false;
        transitionController.ActivatePanel(new GameObject[] { libraryPanel });
        transitionController.AddEventToReturnBtn(() =>
        {
            categoryLibraryControl.ClearBtnsEvents();
            StartSelectMethodRoutine();
        });
    }

    #region Add category in game
    public void SelectAddMethod(GameName _game)
    {
        gameName = _game;
        createPanel = createCategoryPanel;
        libraryPanel = categoryLibrary;
        currentAddedObj = AddedObj.Category;
        categoryLibraryControl.FillLibrary(_game);
        transitionController.ActivatePanel(new GameObject[] { methodSelectorPanel });
        StartSelectMethodRoutine();
    }

    public void AddCategory(string _categoryKey)
    {
        Signals.AddCategoryEvent.Invoke(_categoryKey);
    }

    public void AddCategoryFromlibrary(string _categoryKey)
    {
        if (CategoryIsValid(_categoryKey))
        {
            Debug.Log("add category");
            AddCategory(_categoryKey);
            transitionController.ReturnToBack(2);
        }
        else
        {
            Debug.Log("category exist, select new");
            categoryLibraryControl.BindCardsForSelect();
        }
    }

    private bool CategoryIsValid(string _categoryKey)
    {
        return !patientDataManager.CategoryExist(gameName, _categoryKey);
    }
    #endregion

    #region Add card in category
    public void SelectAddMethod(GameName game, string _categoryKey)
    {
        gameName = game;
        categoryKey = _categoryKey;
        createPanel = createCardPanel;
        libraryPanel = cardLibrary;
        currentAddedObj = AddedObj.Card;
        transitionController.ActivatePanel(new GameObject[] { methodSelectorPanel });
        StartSelectMethodRoutine();
    }

    public void AddCard(string _cardKey)
    {
        Signals.AddCardEvent.Invoke(categoryKey, _cardKey);
    }

    public void AddCardFromLibrary(string _cardKey)
    {
        if (CardIsValid(categoryKey, _cardKey))
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

    private bool CardIsValid(string _categoryKey, string _cardKey)
    {
        return !patientDataManager.CardExists(gameName, _categoryKey, _cardKey);
    }
    #endregion
    #endregion

    #region Set new image
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
    #endregion
}
