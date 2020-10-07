using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PatientData
{
    public string PatientName;
    public string PatientAge;

    public PatientData(string patientName, string patientAge)
    {
        PatientName = patientName;
        PatientAge = patientAge;
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
