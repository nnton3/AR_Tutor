using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameName { Variant, Buttons, WordBook, WordComposing}

public class PatientDataManager : MonoBehaviour
{
    private SaveSystem saveSystem;
    private PatientGameData? patientData;
    [SerializeField] private PatientGameData testData;

    [SerializeField] private string login;

    public PatientGameData? GetPatientData()
    {
        return patientData;
    }

    public string GetPatientLogin()
    {
        return login;
    }

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        FindObjectOfType<CategoryManager>().AddCardEvent.AddListener(AddCardToCategory);
        patientData = new PatientGameData(null);
        var loadData = saveSystem.LoadPatientDataFromLocal(login);

        if (loadData != null) patientData = loadData;

        testData = patientData.Value;
    }

    public void UpdatePatientData()
    {
        saveSystem.SavePatientDataFromLocal(patientData, login);
    }

    #region Category management
    public void HideCategory(GameName game, int index)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                    patientData.Value.VariantGameConfig[index].visible = false;
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void ShowCategory(GameName game, int index)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                    patientData.Value.VariantGameConfig[index].visible = true;
                UpdatePatientData();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Card management
    public void HideCard(GameName game, int categoryIndex, string cardIndex)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                {
                    int index = patientData.Value.VariantGameConfig[categoryIndex].cardKeys.IndexOf(cardIndex);
                    patientData.Value.VariantGameConfig[categoryIndex].cardVisibleValue[index] = false;
                }
                    
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void ShowCard(GameName game, int categoryIndex, string cardIndex)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                {
                    int index = patientData.Value.VariantGameConfig[categoryIndex].cardKeys.IndexOf(cardIndex);
                    patientData.Value.VariantGameConfig[categoryIndex].cardVisibleValue[index] = true;
                }

                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public bool CardExists(GameName game, int categoryIndex, string cardIndex)
    {
        if (patientData.Value.VariantGameConfig[categoryIndex].cardKeys.Contains(cardIndex))
            return true;

        return false;
    }

    public void AddCardToCategory(GameName game, int _categoryIndex, string _cardKey)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Contains(_cardKey)) return;

                patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Add(_cardKey);
                patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue.Add(true);
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void DeleteCardFromCategory(GameName game, int _categoryIndex, string _cardKey)
    {
        switch (game)
        {
            case GameName.Variant:
                patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Remove(_cardKey);
                patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue.Remove(true);
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void SetImageToCard()
    {

    }
    #endregion
}
