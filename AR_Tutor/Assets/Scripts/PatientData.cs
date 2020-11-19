using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PatientData
{
    public string PatientName;
    public string PatientGender;
    public Sprite img;
    public AudioClip nameClip;

    public PatientData(string _patientName, string _patientGender, Sprite _img = null, AudioClip _nameClip = null)
    {
        PatientName = _patientName;
        PatientGender = _patientGender;
        img = _img;
        nameClip = _nameClip;
    }
}

[Serializable]
public struct PatientSaveData
{
    public string PatientName;
    public string PatientGender;
    public string imgAddress;
    public string clipAddress;

    public PatientSaveData(string _patientName, string _patientGender, string _imgAddress, string _clipAddress)
    {
        PatientName = _patientName;
        PatientGender = _patientGender;
        imgAddress = _imgAddress;
        clipAddress = _clipAddress;
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
