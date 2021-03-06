﻿using System;
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
        if (FindObjectOfType<SelectedPatient>())
        {
            patientLogin = FindObjectOfType<SelectedPatient>()?.PatientLogin;
            userLogin = FindObjectOfType<SelectedPatient>()?.UserLogin;
        }

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

        Signals.AddCategoryEvent.AddListener(AddCategoryToGame);
        Signals.AddCardEvent.AddListener(AddCardToCategory);
        Signals.DeleteCategoryFromGame.AddListener(RemoveCategoryFromGame);
        Signals.DeleteCardFromCategory.AddListener(RemoveCardFromCategory);
        Signals.SwitchCardVisibleEvent.AddListener(SwitchCardVisible);
        Signals.SwitchCategoryVisibleEvent.AddListener(SwitchCategoryVisible);
        testData = PatientData;
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
        Debug.Log("Start removed category");
        var index = GetCategoryIndex(_categoryKey);
        if (index < 0) return;

        PatientData.CategoriesKeys.Remove(_categoryKey);
        if (PatientData.CategoriesVisible.Count - 1 >= index)
            PatientData.CategoriesVisible.RemoveAt(index);
        Debug.Log("Category removed from game");
        UpdatePatientData();
    }
    #endregion

    #region Card management
    public void SwitchCardVisible(string _categoryKey, string _cardKey, bool _visible)
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

    public void AddCardToCategory(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return;
        if (PatientData.CardKeys[categoryIndex].Contains(_cardKey)) return;

        PatientData.CardKeys[categoryIndex].Add(_cardKey);
        PatientData.CardsVisible[categoryIndex].Add(true);
        Debug.Log("Card added and save in patient data");
        UpdatePatientData();
    }

    public void RemoveCardFromCategory(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return;
        var cardIndex = GetCardIndex(_categoryKey, _cardKey);
        if (cardIndex < 0) return;

        PatientData.CardKeys[categoryIndex].Remove(_cardKey);
        if (PatientData.CardsVisible[categoryIndex].Count - 1 >= cardIndex)
            PatientData.CardsVisible[categoryIndex][cardIndex] = false;
        Debug.Log("Card removed from category");
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
