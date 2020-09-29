using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VariantCategoryData
{
    //public Sprite icon;
    public bool visible;
    public string categoryName;
    public List<string> cardKeys;
    public List<bool> cardVisibleValue;
}

[CreateAssetMenu(fileName = "New Variant game config", menuName = "Configs/VariantGameConfig")]
public class VariantGameConfig : ScriptableObject
{
    public VariantCategoryData[] Categories = new VariantCategoryData[] { };
}
