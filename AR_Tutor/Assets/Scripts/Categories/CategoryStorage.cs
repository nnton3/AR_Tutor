using UnityEngine;
using System.Collections.Generic;

public class CategoryStorage : MonoBehaviour
{
    [SerializeField] private CategoryPack startCategoryPack;
    private SaveSystem saveSystem;
    private PatientDataManager patientDataManager;
    public Dictionary<string, CategoryData> Categories = new Dictionary<string, CategoryData>();
    public Dictionary<string, CategoryData> VariantCategories = new Dictionary<string, CategoryData>();

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        patientDataManager = FindObjectOfType<PatientDataManager>();

        FillStorage();
    }

    private void FillStorage()
    {
        AddDefaulCategories();
        AddCustomCategories();
    }

    private void AddDefaulCategories()
    {
        for (int i = 0; i < startCategoryPack.categoryDatas.Length; i++)
            switch (startCategoryPack.categoryDatas[i].game)
            {
                case GameName.Variant:
                    VariantCategories.Add($"defaul_variant_category{i}", startCategoryPack.categoryDatas[i]);
                    AddCategory($"defaul_variant_category{i}", startCategoryPack.categoryDatas[i]);
                    break;
                case GameName.Buttons:
                    AddCategory($"defaul_buttons_category{i}", startCategoryPack.categoryDatas[i]);
                    break;
                case GameName.WordBook:
                    AddCategory($"defaul_wordbook_category{i}", startCategoryPack.categoryDatas[i]);
                    break;
                case GameName.WordComposing:
                    AddCategory($"defaul_wordcomposing_category{i}", startCategoryPack.categoryDatas[i]);
                    break;
                default:
                    break;
            }
    }

    private void AddCustomCategories()
    {
        var categories = saveSystem.GetCustomCategoryData();

        if (categories.keys == null) return;

        for (int i = 0; i < categories.keys.Count; i++)
        {
            var img = saveSystem.LoadImage(categories.imgAddresses[i]);
            Rect imgRect = new Rect(0, 0, img.width, img.height);
            var size = (img.width < img.height) ? img.width : img.height;
            imgRect = new Rect(0, 0, size, size);

            var clip = saveSystem.LoadAudio(categories.clipAddresses[i]);

            var index = patientDataManager.GetCategoryIndex(categories.keys[i]);

            var category = new CategoryData(
                (GameName)categories.games[i],
                categories.titles[i],
                Sprite.Create(
                    img,
                    imgRect,
                    Vector2.zero),
                clip,
                patientDataManager.PatientData.CategoriesVisible[index],
                categories.cardKeys[i],
                patientDataManager.PatientData.CardsVisible[index],
                true);

            AddCategory(categories.keys[i], category);
        }
    }

    private void AddCategory(string key, CategoryData categoryData)
    {
        if (!Categories.ContainsKey(key))
            Categories.Add(key, categoryData);
    }

    public void UpdateCustomCategoryImg(string _categoryKey, Sprite _categoryImg)
    {
        var categoryData = saveSystem.GetCustomCategoryData();
        var address = categoryData.keys.IndexOf(categoryData.keys.Find((key) => key == _categoryKey));
        saveSystem.SaveImage(_categoryImg.texture, categoryData.imgAddresses[address]);

        var data = Categories[_categoryKey];
        data.img = _categoryImg;
        Categories[_categoryKey] = data;
    }
}
