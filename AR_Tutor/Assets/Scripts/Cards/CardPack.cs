using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New cardpack", menuName = "Configs/CardPack")]
public class CardPack : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();

    public void GetIndex(string _name)
    {
        foreach (var card in cards)
            if (card.Title == _name) Debug.Log(cards.IndexOf(card));
    }

    public void AddElement(int _index)
    {
        if (_index > -1 && _index < cards.Count - 1)
            cards.Insert(_index, new CardData());
    }

    public void DeleteElement(int _index)
    {
        if (_index > -1 && _index < cards.Count - 1)
            cards.RemoveAt(_index);
    }
}
