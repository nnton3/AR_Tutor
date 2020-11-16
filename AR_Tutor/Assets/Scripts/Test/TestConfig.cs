using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New test config", menuName = "Configs/TestConfig")]
public class TestConfig : ScriptableObject
{
    public int index = 0;
    public List<CardData> Cards = new List<CardData>();

    public void DeleteElement(int _index)
    {
        if (_index > -1 && _index < Cards.Count - 1) Cards.RemoveAt(_index);
    }
}
