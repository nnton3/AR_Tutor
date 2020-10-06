using System;
using System.Collections;
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
public struct PatientGameData
{
    public List<string> CategoriesKeys;
    public List<int> Games;
    public List<bool> CategoriesVisible;
    public List<List<string>> CardKeys;
    public List<List<bool>> CardsVisible;

    public PatientGameData(
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
