using UnityEngine;
using System.Collections;
using System;

public class CustomCardInitializer : CardBase
{
    private VariantGameMenu variantMenu;

    public override void Initialize(GameName _game, int _categoryIndex, string cardKey, CardData data)
    {
        base.Initialize(_game, _categoryIndex, cardKey, data);
        variantMenu = FindObjectOfType<VariantGameMenu>();
    }

    protected override void SwitchVisible()
    {
        Signals.DeleteCardFromCategory.Invoke(game, categoryIndex, Key);
        DeleteFromMenu();
    }

    private void DeleteFromMenu()
    {
        Signals.DeleteCardFromCategory.Invoke(game, categoryIndex, Key);
    }
}
