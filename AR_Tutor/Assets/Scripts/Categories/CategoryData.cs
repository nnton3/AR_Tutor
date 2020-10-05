using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New cardpack", menuName = "Configs/CategoryPack")]
public class CategoryPack: ScriptableObject
{
    public CategoryData[] categoryDatas = new CategoryData[] { };
}

[System.Serializable]
public struct CategoryData
{
    public GameName game;
    public string title;
    public Sprite img;
    public AudioClip clip;
    public bool visible;
    public List<string> cardKeys;

    public CategoryData(GameName _game, string _title, Sprite _img, AudioClip _clip, bool _visible, List<string> _cardKeys)
    {
        game = _game;
        title = _title;
        img = _img;
        clip = _clip;
        visible = _visible;
        cardKeys = _cardKeys;
    }
}

public struct CategorySaveData
{
    public List<int> games;
    public List<string> titles, keys, imgAddresses, clipAddresses;
    public List<List<string>> cardKeys;
    public List<bool> visibles;

    public CategorySaveData(
        List<int> _games = null,
        List<string> _titles = null,
        List<string> _keys = null,
        List<string> _imgAddresses = null,
        List<string> _clipAddresses = null,
        List<List<string>> _cardKeys = null,
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

        if (_cardKeys == null) cardKeys = new List<List<string>>();
        else cardKeys = _cardKeys;

        if (_visibles == null) visibles = new List<bool>();
        else visibles = _visibles;
    }
}
