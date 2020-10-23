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
    public string img1Name, img2Name, img3Name;
    public string audio1Name, audio2Name;

    public CardStorageData(string _title, string _titleForm, string _img1Name, string _img2Name, string _img3Name, string _audio1Name, string _audio2Name)
    {
        title = _title;
        titleForm = _titleForm;
        img1Name = _img1Name;
        img2Name = _img2Name;
        img3Name = _img3Name;
        audio1Name = _audio1Name;
        audio2Name = _audio2Name;
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
            yield return cloudStorage.DownloadSprite(storageData.Value.img1Name, 1);
            yield return cloudStorage.DownloadSprite(storageData.Value.img1Name, 2);
            yield return cloudStorage.DownloadSprite(storageData.Value.img1Name, 3);
            yield return cloudStorage.DownloadAudio(storageData.Value.audio1Name, 1);
            yield return cloudStorage.DownloadAudio(storageData.Value.audio1Name, 2);

            loadedCard = new CardData(
                storageData.Value.title,
                storageData.Value.titleForm,
                cloudStorage.Img1, cloudStorage.Img2, cloudStorage.Img3,
                cloudStorage.Clip1, cloudStorage.Clip2,
                true);

            var patientDataManager = FindObjectOfType<PatientDataManager>();
            var saveSystem = FindObjectOfType<SaveSystem>();
            var storage = FindObjectOfType<CardStorage>();
            var categoryManager = FindObjectOfType<CategoryManager>();

            var cardKey = $"{patientDataManager.GetUserLogin()}{loadedCard.Title}{saveSystem.GetCustomCardsData().keys.Count}";
            var image1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}image1";
            var image2Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}image2";
            var image3Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}image3";
            var audio1Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}audio1";
            var audio2Key = $"{patientDataManager.GetUserLogin()}{saveSystem.GetCustomCardsData().keys.Count}audio2";

            saveSystem.SaveCustomCardFromLocal(loadedCard, cardKey, image1Key, image2Key, image3Key, audio1Key, audio2Key);

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
                    snapshot.Child("testCard").Child("image1").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("image2").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("image3").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("clip1").GetValue(false).ToString(),
                    snapshot.Child("testCard").Child("clip2").GetValue(false).ToString());
            }
        }
    }
}
