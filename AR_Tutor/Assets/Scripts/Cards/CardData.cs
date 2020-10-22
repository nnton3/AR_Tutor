using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CardData
{
    public string Title, TitleForm;
    public bool IsCustom;
    public Sprite img1, img2, img3;
    public AudioClip audioClip1, audioClip2;

    public CardData(string _title,
        string _titleForm,
        Sprite _img1 = null, Sprite _img2 = null, Sprite _img3 = null,
        AudioClip _audioClip1 = null, AudioClip _audioClip2 = null,
        bool _isCustom = false)
    {
        Title = _title; TitleForm = _titleForm;
        img1 = _img1;
        img2 = _img2;
        img3 = _img3;
        audioClip1 = _audioClip1;
        audioClip2 = _audioClip2;
        IsCustom = _isCustom;
    }
}

public struct CardSaveData
{
    public List<string> keys, image1Addres, image2Addres, image3Addres, audio1Addres, audio2Addres, cardTitles, cardTitleForms;

    public CardSaveData(
        List<string> _cardTitles = null,
        List<string> _cardTitleForms = null,
        List<string> _keys = null,
        List<string> _image1Addres = null,
        List<string> _image2Addres = null,
        List<string> _image3Addres = null,
        List<string> _audio1Addres = null,
        List<string> _audio2Addres = null)
    {
        if (_cardTitles == null) cardTitles = new List<string>();
        else cardTitles = _cardTitles;

        if (_cardTitleForms == null) cardTitleForms = new List<string>();
        else cardTitleForms = _cardTitleForms;

        if (_keys == null) keys = new List<string>();
        else keys = _keys;

        if (_image1Addres == null) image1Addres = new List<string>();
        else image1Addres = _image1Addres;

        if (_image2Addres == null) image2Addres = new List<string>();
        else image2Addres = _image2Addres;

        if (_image3Addres == null) image3Addres = new List<string>();
        else image3Addres = _image3Addres;

        if (_audio1Addres == null) audio1Addres = new List<string>();
        else audio1Addres = _audio1Addres;

        if (_audio2Addres == null) audio2Addres = new List<string>();
        else audio2Addres = _audio2Addres;
    }
}
