using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBookMenuControl : GameMenu
{
    public override void Initialize()
    {
        gameName = GameName.WordBook;
        cardSelector = FindObjectOfType<WordBookCardSelector>();
        base.Initialize();
    }

    protected override void BindCategoryBtn(int _categoryIndex)
    {
        var btn = CategoryCards[_categoryIndex].GetSelectBtn();
        var panel = CategoriesPanels[_categoryIndex];

        btn.onClick.AddListener(() =>
        {
            HidePanels();
            panel.SetActive(true);
        });
    }
}
