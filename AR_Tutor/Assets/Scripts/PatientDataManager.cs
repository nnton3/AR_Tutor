using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameName { Variant = 0, Buttons = 1, WordBook = 2, WordComposing = 3, WordComposingRanks = 4}

public class PatientDataManager : MonoBehaviour
{
    #region Variables
    private SaveSystem saveSystem;
    public PatientSaveGameData PatientData { get; private set; }
    [SerializeField] private PatientSaveGameData testData;
    [SerializeField] private string userLogin;
    [SerializeField] private string patientLogin;
    [SerializeField] private CategoriesSO defaultCategoryPack;
    [SerializeField] private CardPack defaultCardPack;
    #endregion

    public string GetUserLogin()
    {
        return userLogin;
    }

    public string GetPatientLogin()
    {
        return patientLogin;
    }

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();

        var selectedPatient = FindObjectOfType<SelectedPatient>();
        if (selectedPatient!= null)
        {
            patientLogin = selectedPatient.PatientLogin;
            userLogin = selectedPatient.UserLogin;
        }

        InitData();

        Signals.AddCategoryEvent.AddListener(AddCategoryToGame);
        Signals.AddCardEvent.AddListener(AddCardToCategory);
        Signals.DeleteCategoryFromGame.AddListener(RemoveCategoryFromGame);
        Signals.DeleteCardFromCategory.AddListener(RemoveCardFromCategory);
        Signals.SwitchCardVisibleEvent.AddListener(SwitchCardVisible);
        Signals.SwitchCategoryVisibleEvent.AddListener(SwitchCategoryVisible);
        Signals.SetIndexForCategory.AddListener(SetIndexForCategory);
        Signals.SetIndexForCard.AddListener(SetIndexForCard);
        testData = PatientData;
    }

    private void InitData()
    {
        PatientData = new PatientSaveGameData(null, null, null, null, null);
        var loadData = saveSystem.LoadPatientDataFromLocal(patientLogin);

        if (loadData != null)
        {
            Debug.Log("have data");
            PatientData = loadData.Value;
            if (PatientData.CardKeys == null)
            {
                Debug.Log("null");
                var temp = PatientData;
                temp.CardKeys = new List<List<string>>();
                temp.CardKeys.Add(new List<string>());
                PatientData = temp;
            }
            if (PatientData.CardsVisible == null)
            {
                Debug.Log("null");
                var temp = PatientData;
                temp.CardsVisible = new List<List<bool>>();
                temp.CardsVisible.Add(new List<bool>());
                PatientData = temp;
            }
        }
        else
        {
            Debug.Log("null data");
            FillPatientDataByDefault();
        }
    }

    private void FillPatientDataByDefault()
    {
        PatientData = new PatientSaveGameData(
            new List<string>(),
            new List<int>(),
            new List<bool>(),
            new List<List<string>>(),
            new List<List<bool>>());

        for (int i = 0; i < defaultCategoryPack.categoryDatas.Count; i++)
        {
            string categoryKey = $"default_{defaultCategoryPack.categoryDatas[i].game.ToString()}_category{i}";

            PatientData.CategoriesKeys.Add(categoryKey);
            PatientData.CategoriesVisible.Add(defaultCategoryPack.categoryDatas[i].visible);
            PatientData.Games.Add(defaultCategoryPack.categoryDatas[i].game);
            PatientData.CardKeys.Add(new List<string>(defaultCategoryPack.categoryDatas[i].cardKeys));
            PatientData.CardsVisible.Add(new List<bool>(defaultCategoryPack.categoryDatas[i].cardsVisible));
        }
    }

    public void UpdatePatientData()
    {
        saveSystem.SavePatientDataFromLocal(PatientData, patientLogin);
    }

    #region Category management
    public void SwitchCategoryVisible(string _categoryKey, bool _visible)
    {
        var index = GetCategoryIndex(_categoryKey);
        if (index < 0) return;
        PatientData.CategoriesVisible[index] = _visible;

        UpdatePatientData();
    }

    public bool CategoryExist(GameName gameName, string _categoryKey)
    {
        var index = GetCategoryIndex(_categoryKey);

        if (index < 0) return false;
        if (PatientData.Games[index] != (int)gameName) return false;

        return true;
    }

    private void AddCategoryToGame(string _categoryKey)
    {
        if (PatientData.CategoriesKeys.Contains(_categoryKey)) return;
        
        PatientData.CategoriesKeys.Add(_categoryKey);
        PatientData.Games.Add(FindObjectOfType<CategoryStorage>().Categories[_categoryKey].game);
        PatientData.CategoriesVisible.Add(true);
        PatientData.CardKeys.Add(new List<string>());
        PatientData.CardsVisible.Add(new List<bool>());
        Debug.Log("Category added from game");
        UpdatePatientData();
    }

    private void RemoveCategoryFromGame(string _categoryKey)
    {
        var index = GetCategoryIndex(_categoryKey);
        if (index < 0) return;

        PatientData.CategoriesKeys.Remove(_categoryKey);
        if (PatientData.CategoriesVisible.Count - 1 >= index)
            PatientData.CategoriesVisible.RemoveAt(index);

        Debug.Log("Category removed from game");
        UpdatePatientData();
    }

    private void SetIndexForCategory(string _categoryKey, int _index)
    {
        var currentIndex = GetCategoryIndex(_categoryKey);
        var game = PatientData.Games[currentIndex];
        var categoryVisible = PatientData.CategoriesVisible[currentIndex];
        var cards = PatientData.CardKeys[currentIndex];
        var cardsVisible = PatientData.CardsVisible[currentIndex];

        PatientData.CategoriesKeys.RemoveAt(currentIndex);
        PatientData.Games.RemoveAt(currentIndex);
        PatientData.CategoriesVisible.RemoveAt(currentIndex);
        PatientData.CardKeys.RemoveAt(currentIndex);
        PatientData.CardsVisible.RemoveAt(currentIndex);

        PatientData.CategoriesKeys.Insert(_index, _categoryKey);
        PatientData.Games.Insert(_index, game);
        PatientData.CategoriesVisible.Insert(_index, categoryVisible);
        PatientData.CardKeys.Insert(_index, cards);
        PatientData.CardsVisible.Insert(_index, cardsVisible);
        
        UpdatePatientData();
    }
    #endregion

    #region Card management
    private void SwitchCardVisible(string _categoryKey, string _cardKey, bool _visible)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        var cardIndex = GetCardIndex(_categoryKey, _cardKey);
        if (categoryIndex < 0) return;
        if (cardIndex < 0) return;

        var data = PatientData;
        data.CardsVisible[categoryIndex][cardIndex] = _visible;
        PatientData = data;
        UpdatePatientData();
    }

    public bool CardExists(GameName game, string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        var carIndex = GetCardIndex(_categoryKey, _cardKey);

        if (carIndex < 0) return false;
        if (PatientData.Games[categoryIndex] != (int)game) return false;

        return true;
    }

    private void AddCardToCategory(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return;
        if (PatientData.CardKeys[categoryIndex].Contains(_cardKey)) return;

        PatientData.CardKeys[categoryIndex].Add(_cardKey);
        PatientData.CardsVisible[categoryIndex].Add(true);
        Debug.Log("Card added and save in patient data");
        UpdatePatientData();
    }

    private void RemoveCardFromCategory(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return;
        var cardIndex = GetCardIndex(_categoryKey, _cardKey);
        if (cardIndex < 0) return;

        PatientData.CardKeys[categoryIndex].RemoveAt(cardIndex);
        if (PatientData.CardsVisible[categoryIndex].Count - 1 >= cardIndex)
            PatientData.CardsVisible[categoryIndex].RemoveAt(cardIndex);

        Debug.Log("Card removed from category");
        UpdatePatientData();
    }

    private void SetIndexForCard(string _categoryKey, string _cardKey, int _index)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        var currentCardIndex = GetCardIndex(_categoryKey, _cardKey);
        var card = PatientData.CardKeys[categoryIndex][currentCardIndex];
        var cardVisible = PatientData.CardsVisible[categoryIndex][currentCardIndex];

        PatientData.CardKeys[categoryIndex].RemoveAt(currentCardIndex);
        PatientData.CardsVisible[categoryIndex].RemoveAt(currentCardIndex);

        PatientData.CardKeys[categoryIndex].Insert(_index, card);
        PatientData.CardsVisible[categoryIndex].Insert(_index, cardVisible);

        UpdatePatientData();
    }
    #endregion

    public int GetCategoryIndex(string _categoryKey)
    {
        var targetCategory = PatientData.CategoriesKeys.Find((key) => key == _categoryKey);
        if (targetCategory == null) return -1;
        return PatientData.CategoriesKeys.IndexOf(targetCategory);
    } 

    public int GetCardIndex(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return -1;
        var targetCard = PatientData.CardKeys[categoryIndex].Find((key) => key == _cardKey);
        if (targetCard == null) return -1;
        return PatientData.CardKeys[categoryIndex].IndexOf(targetCard);
    }
}
