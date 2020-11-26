using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    private PatientDataManager patientDataManager;
    [SerializeField] private Button testDelete;
    private CategorySaveData categorySaveData = new CategorySaveData(null, null, null, null, null, null);
    private CardSaveData cardSaveData = new CardSaveData();

    public void Initialize()
    {
        patientDataManager = FindObjectOfType<PatientDataManager>();

        testDelete.onClick.AddListener(DeleteData);
        cardSaveData = LoadCustomCardsFromLocal();
        categorySaveData = LoadCustomCategoriesFromLocal();
    }

    #region Patient
    public void SavePatientDataFromLocal(PatientSaveGameData data, string _login)
    {
        ES3.Save(_login, data);

        //var json = JsonUtility.ToJson(data);
        //PlayerPrefs.SetString(_login, json);
    }

    public PatientSaveGameData? LoadPatientDataFromLocal(string _login)
    {
        if (ES3.KeyExists(_login))
            return ES3.Load<PatientSaveGameData>(_login);
        else return null;
        //if (!PlayerPrefs.HasKey(_login))
        //{
        //    Debug.Log("U haven't category data in local storage");
        //    return null;
        //}
        //else
        //    return JsonUtility.FromJson<PatientSaveGameData>(PlayerPrefs.GetString(_login));
    }
    #endregion

    #region Categories
    public void SaveCustomCategoryFromLocal(CategoryData _categoryData, string _categoryKey, string _imageKey, string _audioKey)
    {
        categorySaveData.games.Add(_categoryData.game);
        categorySaveData.titles.Add(_categoryData.title);
        categorySaveData.imgAddresses.Add(_imageKey);
        categorySaveData.clipAddresses.Add(_audioKey);
        categorySaveData.keys.Add(_categoryKey);
        categorySaveData.isCustom.Add(true);

        SaveImage(_categoryData.img.texture, _imageKey);
        SaveAudio(_categoryData.clip, _audioKey);

        UpdateCustomCategoryLocal();
    }

    public CategorySaveData LoadCustomCategoriesFromLocal()
    {
        string key = $"{patientDataManager.GetUserLogin()}_Custom_categories";
        if (!PlayerPrefs.HasKey(key))
            return new CategorySaveData(null, null, null, null, null, null);
        else
            return JsonUtility.FromJson<CategorySaveData>(PlayerPrefs.GetString(key));
    }

    private void UpdateCustomCategoryLocal()
    {
        var json = JsonUtility.ToJson(categorySaveData);
        PlayerPrefs.SetString($"{patientDataManager.GetUserLogin()}_Custom_categories", json);
    }

    public CategorySaveData GetCustomCategoryData()
    {
        return categorySaveData;
    }
    #endregion

    #region Cards
    public void SaveCustomCardFromLocal(CardData cardData, string key, string _image1Key, string _image2Key, string _image3Key, string _audio1Key, string _audio2Key)
    {
        cardSaveData.cardTitles.Add(cardData.Title);
        cardSaveData.cardTitleForms.Add(cardData.TitleForm);
        cardSaveData.keys.Add(key);
        cardSaveData.image1Addres.Add(_image1Key);
        cardSaveData.image2Addres.Add(_image2Key);
        cardSaveData.image3Addres.Add(_image3Key);
        cardSaveData.audio1Addres.Add(_audio1Key);
        cardSaveData.audio2Addres.Add(_audio2Key);

        SaveImage(cardData.img1.texture, _image1Key);
        SaveImage(cardData.img2.texture, _image2Key);
        SaveImage(cardData.img3.texture, _image3Key);
        SaveAudio(cardData.audioClip1, _audio1Key);
        SaveAudio(cardData.audioClip2, _audio2Key);

        UpdateCustomCardLocal();
    }

    private CardSaveData LoadCustomCardsFromLocal()
    {
        if (!PlayerPrefs.HasKey("Custom_cards")) return new CardSaveData(null, null, null, null,null);
        else return JsonUtility.FromJson<CardSaveData>(PlayerPrefs.GetString("Custom_cards"));
    }

    private void UpdateCustomCardLocal()
    {
        var json = JsonUtility.ToJson(cardSaveData);
        PlayerPrefs.SetString("Custom_cards", json);
    }

    public CardSaveData GetCustomCardsData()
    {
        return cardSaveData;
    }
    #endregion

    #region Image
    public void SaveImage(Texture2D texture, string imageKey)
    {
        if (texture == null) return;
        ES3.SaveImage(texture, $"{imageKey}.png");
    }

    public Texture2D LoadImage(string key)
    {
        if (ES3.FileExists($"{key}.png"))
            return ES3.LoadImage($"{key}.png");
        else
        {
            Debug.Log("Image not found");
            return null;
        }
    }
    #endregion

    #region Audio
    public void SaveAudio(AudioClip _clip, string _key)
    {
        if (_clip == null) return;
        SaveWav.Save(_key, _clip);
    }

    public AudioClip LoadAudio(string _key)
    {
        if (ES3.FileExists($"{_key}.wav"))
            return ES3.LoadAudio($"{Application.persistentDataPath}/{_key}.wav", AudioType.WAV);
        else
        {
            Debug.Log("Image not found");
            return null;
        }
    }
    #endregion

    private void DeleteData()
    {
        Debug.Log("clear data");
        ES3.DeleteFile();
        PlayerPrefs.DeleteAll();
    }
}
