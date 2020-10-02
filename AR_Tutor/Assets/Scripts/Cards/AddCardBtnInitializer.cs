using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardBtnInitializer : CardBase
{
    public override void Initialize(GameName _game, int _categoryIndex, string _cardKey, CardData _data)
    {
        game = _game;
        categoryIndex = _categoryIndex;
        title.text = "Add new card";
    }
}
