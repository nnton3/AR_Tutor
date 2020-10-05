using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CategoryStorage : MonoBehaviour
{

    private SaveSystem saveSystem;
    public Dictionary<string, CategoryData> VariantCategories = new Dictionary<string, CategoryData>();

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
    }

    private void FillStorage()
    {
        AddDefaulCategories();
        AddCustomCategories();
    }

    private void AddDefaulCategories()
    {
            
    }

    private void AddCustomCategories()
    {
        var categories = saveSystem.GetCustomCategoryData();

        for (int i = 0; i < categories.keys.Count; i++)
        {
            var img = saveSystem.LoadImage(categories.imgAddresses[i]);
            Rect imgRect = new Rect(0, 0, img.width, img.height);
            var size = (img.width < img.height) ? img.width : img.height;
            imgRect = new Rect(0, 0, size, size);

            var clip = saveSystem.LoadAudio(categories.clipAddresses[i]);

            var category = new CategoryData(
                (GameName)categories.games[i],
                categories.titles[i],
                Sprite.Create(
                    img,
                    imgRect,
                    Vector2.zero),
                clip,
                categories.visibles[i],
                categories.cardKeys[i]
                );
        }
    }

    private void AddCustomCategory(string key, CategoryData categoryData)
    {
        switch (categoryData.game)
        {
            case GameName.Variant:
                if (!VariantCategories.ContainsKey(key))
                    VariantCategories.Add(key, categoryData);
                break;
            case GameName.Buttons:
                break;
            case GameName.WordBook:
                break;
            case GameName.WordComposing:
                break;
            default:
                break;
        }
    }
}
