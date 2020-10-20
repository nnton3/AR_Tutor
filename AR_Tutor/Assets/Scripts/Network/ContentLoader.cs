using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardStorageData
{
    public string title;
    public string titleForm;
    public string imageName;
    public string audioName;

    public CardStorageData(string _title, string _titleForm, string _imageName, string _audioName)
    {
        title = _title;
        titleForm = _titleForm;
        imageName = _imageName;
        audioName = _audioName;
    }
}

public class ContentLoader : MonoBehaviour
{
    private CloudStorage cloudStorage;
    private FirebaseDatabase database = null;
    private DataSnapshot snapshot = null;
    [SerializeField] private CardStorageData? storageData;
    [SerializeField] private CardData loadedCard;

    private void Awake()
    {
        cloudStorage = FindObjectOfType<CloudStorage>();

        database = FirebaseDatabase.DefaultInstance;
        database.App.Options.DatabaseUrl = new System.Uri("https://ar-tutor.firebaseio.com/");
    }

    public IEnumerator LoadCardFromCloud(string _cardKey)
    {
        yield return StartCoroutine(LoadCardConfigFromCloud(_cardKey));

        if (storageData != null)
        {
            yield return cloudStorage.DownloadSprite(storageData.Value.imageName);
            yield return cloudStorage.DownloadAudio(storageData.Value.audioName);

            loadedCard = new CardData(
                storageData.Value.title,
                storageData.Value.titleForm,
                cloudStorage.LastLoadedSprite,
                cloudStorage.LastLoadedClip, 
                true);

            var patientDataManager = FindObjectOfType<PatientDataManager>();
            var saveSystem = FindObjectOfType<SaveSystem>();
            var storage = FindObjectOfType<CardStorage>();
            var categoryManager = FindObjectOfType<CategoryManager>();

            var cardKey = $"{patientDataManager.GetUserLogin()}{loadedCard.Title}{saveSystem.GetCustomCardsData().keys.Count}";
            var image1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}image1";
            var audio1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}audio1";

            saveSystem.SaveCustomCardFromLocal(loadedCard, cardKey, image1Key, audio1Key);

            storage.AddNewCardToBase(loadedCard, cardKey);
            categoryManager.AddCard(cardKey);
            Signals.CardLoadEnd.Invoke(true);
        }
        else
            Signals.CardLoadEnd.Invoke(false);
    }

    private IEnumerator LoadCardConfigFromCloud(string _cardKey)
    {
        var readTask = database.GetReference("cards")
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                        snapshot = task.Result;
                });

        yield return new WaitUntil(() => readTask.IsCompleted);

        if (snapshot != null)
        {
            if (snapshot.HasChild(_cardKey))
            {
                storageData = new CardStorageData(
                    snapshot.Child("testCard").Child("title").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("titleForm").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("imageName").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("audioName").GetValue(false).ToString());
            }
        }
    }
}
