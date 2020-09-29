using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Button testDelete;
    private CardSaveData cardSaveData = new CardSaveData(null, null, null, null);

    public void Initialize()
    {
        testDelete.onClick.AddListener(DeleteData);
        cardSaveData = LoadCustomCardsFromLocal();
    }

    #region Patient
    public void SavePatientDataFromLocal(PatientGameData? data, string login)
    {
        var json = JsonUtility.ToJson(data.Value);
        PlayerPrefs.SetString(login, json);
    }

    public PatientGameData? LoadPatientDataFromLocal(string patientLogin)
    {
        if (!PlayerPrefs.HasKey(patientLogin))
        {
            Debug.Log("U haven't patient data in local storage");
            return null;
        }
        else
            return JsonUtility.FromJson<PatientGameData>(PlayerPrefs.GetString(patientLogin));
    }
    #endregion

    #region Cards
    public void SaveCustomCardFromLocal(CardData cardData, string key, string imageKey)
    {
        cardSaveData.cardTitles.Add(cardData.Title);
        cardSaveData.cardTitleForms.Add(cardData.TitleForm);
        cardSaveData.keys.Add(key);
        cardSaveData.imageAddres.Add(imageKey);

        UpdateCustomCardLocal();
    }

    private CardSaveData LoadCustomCardsFromLocal()
    {
        if (!PlayerPrefs.HasKey("Custom_cards")) return new CardSaveData(null, null, null, null);
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
    private void SaveAudio(AudioClip clip)
    {
        ES3.Save<AudioClip>("TestAudio", clip);
    }

    private AudioClip LoadAudio(string key)
    {
        if (ES3.FileExists(key))
            return ES3.Load<AudioClip>(key);
        else return null;
    }
    #endregion

    private void DeleteData()
    {
        Debug.Log("clear data");
        PlayerPrefs.DeleteAll();
    }
}
