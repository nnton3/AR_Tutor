using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New category pack", menuName = "Configs/CategoryPack")]
public class CategoriesSO : ScriptableObject
{
    public CategoryData[] categoryDatas;
}
