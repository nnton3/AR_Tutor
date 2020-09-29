using UnityEngine;
using System.Collections;
using System;

public class CustomCardInitializer : CardInitializer
{
    private MainMenuUIControl mainMenu;
    private VariantGameMenu variantMenu;

    public override void Initialize(GameName _game, int _categoryIndex, string cardKey, CardData data)
    {
        base.Initialize(_game, _categoryIndex, cardKey, data);
        variantMenu = FindObjectOfType<VariantGameMenu>();
        mainMenu = FindObjectOfType<MainMenuUIControl>();
    }

    protected override void HideCard()
    {
        patientManager.DeleteCardFromCategory(game, categoryIndex, key);
        DeleteFromMenu();
    }

    private void DeleteFromMenu()
    {
        switch (game)
        {
            case GameName.Variant:
                variantMenu.DeleteCard(categoryIndex, key);
                break;
            default:
                break;
        }
    }
}
