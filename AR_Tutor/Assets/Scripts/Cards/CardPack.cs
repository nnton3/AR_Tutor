using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New cardpack", menuName = "Configs/CardPack")]
public class CardPack : ScriptableObject
{
    public CardData[] cards = new CardData[] { };
}
