using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New category pack", menuName = "Configs/CategoryPack")]
public class CategoriesSO : ScriptableObject
{
    public List<CategoryData> categoryDatas;

    public void DeleteElement(int _index)
    {
        if (_index > -1 && _index < categoryDatas.Count - 1)
            categoryDatas.RemoveAt(_index);
    }

    public void DeleteCardFromCategory(int _categoryIndex, int _cardIndex)
    {
        var targetCategory = categoryDatas[_categoryIndex];
        targetCategory.cardKeys.Remove(targetCategory.cardKeys[_cardIndex]);
        targetCategory.cardsVisible.RemoveRange(targetCategory.cardsVisible.Count - 1, 1);
    }
}
