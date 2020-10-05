using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CategoryData
{
    public GameName game;
    public string title;
    public Sprite img;
    public AudioClip clip;
    public bool visible;

    public CategoryData(GameName _game, string _title, Sprite _img, AudioClip _clip, bool _visible)
    {
        game = _game;
        title = _title;
        img = _img;
        clip = _clip;
        visible = _visible;
    }
}

public struct CategorySaveData
{
    public List<int> games;
    public List<string> titles, keys, imgAddresses, clipAddresses;
    public List<bool> visibles;

    public CategorySaveData(
        List<int> _games = null,
        List<string> _titles = null, 
        List<string> _keys = null,
        List<string>  _imgAddresses = null, 
        List<string> _clipAddresses = null,
        List<bool> _visibles = null)
    {
        if (_games == null) games = new List<int>();
        else games = _games;

        if (_titles == null) titles = new List<string>();
        else titles = _titles;

        if (_keys == null) keys = new List<string>();
        else keys = _keys;

        if (_imgAddresses == null) imgAddresses = new List<string>();
        else imgAddresses = _imgAddresses;

        if (_clipAddresses == null) clipAddresses = new List<string>();
        else clipAddresses = _clipAddresses;

        if (_visibles == null) visibles = new List<bool>();
        else visibles = _visibles;
    }
}
