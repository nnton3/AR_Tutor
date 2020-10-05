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
        AddDefaulStorage();
        AddCustomStorage();
    }

    private void AddDefaulStorage()
    {
            
    }

    private void AddCustomStorage()
    {
        //var categories = saveSystem
    }
}
