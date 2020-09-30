﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Button testDelete;
    private CardSaveData cardSaveData = new CardSaveData(null, null, null, null, null);

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
    public void SaveCustomCardFromLocal(CardData cardData, string key, string imageKey, string audioKey)
    {
        cardSaveData.cardTitles.Add(cardData.Title);
        cardSaveData.cardTitleForms.Add(cardData.TitleForm);
        cardSaveData.keys.Add(key);
        cardSaveData.imageAddres.Add(imageKey);
        cardSaveData.audioAddres.Add(audioKey);

        UpdateCustomCardLocal();
    }

    private CardSaveData LoadCustomCardsFromLocal()
    {
        if (!PlayerPrefs.HasKey("Custom_cards")) return new CardSaveData(null, null, null, null, null);
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
    public void SaveAudio(AudioClip clip, string _key)
    {
        SaveWav.Save(_key, clip);
    }

    public AudioClip LoadAudio(string _key)
    {
        return ES3.LoadAudio($"{Application.persistentDataPath}/{_key}.wav", AudioType.WAV);
    }
    #endregion

    private void DeleteData()
    {
        Debug.Log("clear data");
        PlayerPrefs.DeleteAll();
    }
}