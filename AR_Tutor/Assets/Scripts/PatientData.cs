using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PatientData
{
    public string PatientName;
    public string PatientAge;
    public Sprite img;

    public PatientData(string _patientName, string _patientAge, Sprite _img = null)
    {
        PatientName = _patientName;
        PatientAge = _patientAge;
        img = _img;
    }
}

[Serializable]
public struct PatientSaveData
{
    public string PatientName;
    public string PatientAge;
    public string imgAddress;

    public PatientSaveData(string _patientName, string _patientAge, string _imgAddress)
    {
        PatientName = _patientName;
        PatientAge = _patientAge;
        imgAddress = _imgAddress;
    }
}

[Serializable]
public struct PatientCategoryData
{
    public string CategoryKeys;
    public bool CategoryVisible;
    public List<string> CardKeys;
    public List<bool> CardsVisible;
}

[Serializable]
public struct PatientSaveGameData
{
    public List<string> CategoriesKeys;
    public List<int> Games;
    public List<bool> CategoriesVisible;
    public List<List<string>> CardKeys;
    public List<List<bool>> CardsVisible;

    public PatientSaveGameData(
        List<string> _categoriesKeys, 
        List<int> _games,
        List<bool> _categoriesVisible, 
        List<List<string>> _cardKeys,
        List<List<bool>> _cardsVisible)
    {
        CategoriesKeys = _categoriesKeys;
        Games = _games;
        CategoriesVisible = _categoriesVisible;
        CardKeys = _cardKeys;
        CardsVisible = _cardsVisible;
    }
}
