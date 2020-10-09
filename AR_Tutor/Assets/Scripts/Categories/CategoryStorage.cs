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
            string key = "";
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

            CategoryData categoryData = startCategoryPack.categoryDatas[i];
            if (patientDataManager.PatientData.CategoriesKeys.Contains(key))
            {
                var index = patientDataManager.PatientData.CategoriesKeys.IndexOf(key);

                if (patientDataManager.PatientData.CategoriesVisible != null)
                    if (patientDataManager.PatientData.CategoriesVisible.Count - 1 >= index)
                        categoryData.visible = patientDataManager.PatientData.CategoriesVisible[index];

                if (index >= 0)
                {
                    if (patientDataManager.PatientData.CardKeys != null)
                        if (patientDataManager.PatientData.CardKeys.Count - 1 >= index)
                            categoryData.cardKeys = patientDataManager.PatientData.CardKeys[index];

                    if (patientDataManager.PatientData.CardsVisible != null)
                        if (patientDataManager.PatientData.CardsVisible.Count - 1 >= index)
                            categoryData.cardsVisible = patientDataManager.PatientData.CardsVisible[index];
                }
            }

            AddCategory(key, categoryData);
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

            List<string> cardKeys = new List<string>();
            List<bool> cardsVisible = new List<bool>();
            /// if u have card keys in custom categories
            if (index >= 0)
            {
                if (patientDataManager.PatientData.CardKeys != null)
                    if (patientDataManager.PatientData.CardKeys.Count >= index - 1)
                        cardKeys = patientDataManager.PatientData.CardKeys[index];

                if (patientDataManager.PatientData.CardsVisible != null)
                    if (patientDataManager.PatientData.CardsVisible.Count >= index - 1)
                        cardsVisible = patientDataManager.PatientData.CardsVisible[index];
            }

            var category = new CategoryData(
                categories.games[i],
                categories.titles[i],
                Sprite.Create(
                    img,
                    imgRect,
                    Vector2.zero),
                clip,
                patientDataManager.PatientData.CategoriesVisible[index],
                cardKeys,
                cardsVisible,
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
        var index = categoryData.keys.IndexOf(categoryData.keys.Find((key) => key == _categoryKey));
        if (index >= 0)
            saveSystem.SaveImage(_categoryImg.texture, categoryData.imgAddresses[index]);

        SwapImg(Categories);

        switch ((GameName)categoryData.games[index])
        {
            case GameName.Variant:
                SwapImg(VariantCategories);
                break;
            case GameName.Buttons:
                SwapImg(ButtonsCategories);
                break;
            case GameName.WordBook:
                SwapImg(WordBookCategories);
                break;
            case GameName.WordComposing:
                SwapImg(WordComposingCategories);
                break;
            default:
                break;
        }

        void SwapImg(Dictionary<string, CategoryData> targetGame)
        {
            var data = targetGame[_categoryKey];
            data.img = _categoryImg;
            targetGame[_categoryKey] = data;
        }
    }

    public void AddCategoryToBase(CategoryData _data, string _categoryKey, string _imageAddress, string _clipAddress)
    {
        if (Categories.ContainsKey(_categoryKey)) return;

        AddCategory(_categoryKey, _data);

        saveSystem.SaveCustomCategoryFromLocal(_data, _categoryKey, _imageAddress, _clipAddress);
    }
}
