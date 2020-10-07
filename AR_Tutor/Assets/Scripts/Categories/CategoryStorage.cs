using UnityEngine;
using System.Collections.Generic;

public class CategoryStorage : MonoBehaviour
{
    #region Variables
    [SerializeField] private CategoriesSO startCategoryPack;
    private SaveSystem saveSystem;
    private PatientDataManager patientDataManager;
    public Dictionary<string, CategoryData> Categories = new Dictionary<string, CategoryData>();
    public Dictionary<string, CategoryData> VariantCategories = new Dictionary<string, CategoryData>();
    public Dictionary<string, CategoryData> ButtonsCategories = new Dictionary<string, CategoryData>();
    public Dictionary<string, CategoryData> WordBookCategories = new Dictionary<string, CategoryData>();
    public Dictionary<string, CategoryData> WordComposingCategories = new Dictionary<string, CategoryData>();
    #endregion

    #region Initialize
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
        {
            string key = null;
            switch ((GameName)startCategoryPack.categoryDatas[i].game)
            {
                case GameName.Variant:
                    key = $"default_0_category{i}";
                    break;
                case GameName.Buttons:
                    key = $"default_1_category{i}";
                    break;
                case GameName.WordBook:
                    key = $"default_2_category{i}";
                    break;
                case GameName.WordComposing:
                    key = $"default_3_category{i}";
                    break;
                default:
                    break;
            }
            AddCategory(key, startCategoryPack.categoryDatas[i]);
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
            Debug.Log(patientDataManager.PatientData.CardsVisible.Count);
            var category = new CategoryData(
                categories.games[i],
                categories.titles[i],
                Sprite.Create(
                    img,
                    imgRect,
                    Vector2.zero),
                clip,
                patientDataManager.PatientData.CategoriesVisible[index],
                (categories.cardKeys == null)? new List<string>(): categories.cardKeys[i],
                new List<bool>(),
                //(index < 0) ? new List<bool>() : patientDataManager.PatientData.CardsVisible[index],
                true);

            AddCategory(categories.keys[i], category);
        }
    }
    #endregion

    private void AddCategory(string _categoryKey, CategoryData _data)
    {
        if (!Categories.ContainsKey(_categoryKey))
            Categories.Add(_categoryKey, _data);

        switch ((GameName)_data.game)
        {
            case GameName.Variant:
                VariantCategories.Add(_categoryKey, _data);
                break;
            case GameName.Buttons:
                ButtonsCategories.Add(_categoryKey, _data);
                break;
            case GameName.WordBook:
                WordBookCategories.Add(_categoryKey, _data);
                break;
            case GameName.WordComposing:
                WordComposingCategories.Add(_categoryKey, _data);
                break;
            default:
                break;
        }
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

    public void AddCategoryToBase(CategoryData _data, string _categoryKey, string _imageAddress, string _clipAddress)
    {
        if (Categories.ContainsKey(_categoryKey)) return;

        AddCategory(_categoryKey, _data);

        saveSystem.SaveCustomCategoryFromLocal(_data, _categoryKey, _imageAddress, _clipAddress);
    }
}
