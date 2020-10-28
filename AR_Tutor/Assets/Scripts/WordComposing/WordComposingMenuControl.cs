using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordComposingMenuControl : GameMenu
{
    public override void Initialize()
    {
        gameName = GameName.WordComposing;
        cardSelector = FindObjectOfType<WordComposingSelector>();
        base.Initialize();
    }
}
