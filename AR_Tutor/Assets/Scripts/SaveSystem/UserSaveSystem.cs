using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class UserSaveSystem : MonoBehaviour
{
    private DataBaseControl database; 
    public PatientData LoadedPatient { get; private set; }

    private void Awake()
    {
        database = FindObjectOfType<DataBaseControl>();
    }

    #region Database
    public void SaveUserToCloud(string _userID, UserData _data)
    {
        database.WriteUserData(_userID, _data);
    }

    public void SavePatientInDatabase(string _patientLogin, PatientData data)
    {
        StartCoroutine(CheckInternetConnection((value) =>
        {
            if (value)
                database.WritePatientData(_patientLogin, data);
        }));
        database.WritePatientData(_patientLogin, data);
    }

    private IEnumerator LoadPatientsFromDatabase(List<string> _patientsIdentifiers)
    {
        foreach (var identifier in _patientsIdentifiers)
            yield return StartCoroutine(LoadPatientFromCloudRoutine(identifier));
    }

    public IEnumerator LoadPatientFromCloudRoutine(string identifier)
    {
        yield return StartCoroutine(CheckInternetConnection());

        if (hasConnection)
        {
            yield return StartCoroutine(database.ReadPatientDataRoutine(identifier));
            LoadedPatient = new PatientData(database.patientData.PatientName, database.patientData.PatientAge);
            SavePatientsFromLocal(identifier, LoadedPatient, "");
        }
        else Signals.ShowNotification.Invoke("Отсутствует подключение");
    }
    #endregion

    #region Local
    public void SaveUserData(string _userID, UserData _data)
    {
        var json = JsonUtility.ToJson(_data);
        PlayerPrefs.SetString(_userID, json);
    }

    public UserData LoadUserData(string _userID)
    {
        var json = PlayerPrefs.GetString(_userID);
        return JsonUtility.FromJson<UserData>(json);
    }

    public bool HasUserData(string _userID)
    {
        return PlayerPrefs.HasKey(_userID);
    }

    public void SavePatientsFromLocal(string _login, PatientData _data, string _imageKey)
    {
        var data = new PatientSaveData(
            _data.PatientName,
            _data.PatientAge,
            _imageKey);

        if (_data.img != null)
            SaveImage(_data.img.texture, _imageKey);

        var json = JsonUtility.ToJson(_data);
        PlayerPrefs.SetString(_login, json);
    }

    public PatientData LoadPatientsFromLocal(string _login)
    {
        if (!PlayerPrefs.HasKey(_login)) return new PatientData();

        var json = PlayerPrefs.GetString(_login);
        var savedData = JsonUtility.FromJson<PatientSaveData>(json);

        Sprite targetSprite = null;
        var texture = LoadImage(savedData.imgAddress);

        if (texture != null)
            targetSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.width),
                Vector2.zero);

        return new PatientData(savedData.PatientName, savedData.PatientAge, targetSprite);
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

    private IEnumerator CheckInternetConnection(Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest("http://google.com");
        yield return www.SendWebRequest();
        if (www.error != null)
            action(false);
        else
            action(true);
    }

    private bool hasConnection;
    private IEnumerator CheckInternetConnection()
    {
        UnityWebRequest www = new UnityWebRequest("http://google.com");
        yield return www.SendWebRequest();
        if (www.error != null)
            hasConnection = false;
        else
            hasConnection = true;
    }
}
