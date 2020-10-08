using UnityEngine;
using System.Collections;
using System;

public class CustomCardInitializer : CardBase
{
    private VariantGameMenu variantMenu;

    public override void Initialize(GameName _game, string _categoryKey, string cardKey, CardData data)
    {
        base.Initialize(_game, _categoryKey, cardKey, data);
        variantMenu = FindObjectOfType<VariantGameMenu>();
    }

    protected override void SwitchVisible()
    {
        Debug.Log("deleted");
        Signals.DeleteCardFromCategory.Invoke(categoryKey, Key);
        DeleteFromMenu();
    }

    private void DeleteFromMenu()
    {
        Signals.DeleteCardFromCategory.Invoke(categoryKey, Key);
    }
}
