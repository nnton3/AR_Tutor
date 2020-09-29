using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardData
{
    public string Title, TitleForm;
    public bool IsCustom;
    public Sprite img;
    public AudioClip audioClip;

    public CardData(string _title,
        string _titleForm,
        Sprite _img,
        AudioClip _audioClip,
        bool _isCustom = false)
    {
        Title = _title; TitleForm = _titleForm;
        img = _img;
        audioClip = _audioClip;
        IsCustom = _isCustom;
    }
}

public struct CardSaveData
{
    public List<string> keys, imageAddres, audioAddres, cardTitles, cardTitleForms;

    public CardSaveData(
        List<string> _cardTitles,
        List<string> _cardTitleForms,
        List<string> _keys,
        List<string> _imageAddres, 
        List<string> _audioAddres)
    {
        if (_cardTitles == null) cardTitles = new List<string>();
        else cardTitles = _cardTitles;

        if (_cardTitleForms == null) cardTitleForms = new List<string>();
        else cardTitleForms = _cardTitleForms;

        if (_keys == null) keys = new List<string>();
        else keys = _keys;

        if (_imageAddres == null) imageAddres = new List<string>();
        else imageAddres = _imageAddres;

        if (_audioAddres == null) audioAddres = new List<string>();
        else audioAddres = _audioAddres;
    }
}
