using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardBtnInitializer : CardBase
{
    public override void Initialize(GameName _game, string _categoryKey, string _cardKey, CardData _data)
    {
        game = _game;
        categoryKey = _categoryKey;
        title.text = "Add new card";
    }
}
