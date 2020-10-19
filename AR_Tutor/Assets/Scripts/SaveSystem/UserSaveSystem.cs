using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserSaveSystem : MonoBehaviour
{
    DataBaseControl database;

    private void Awake()
    {
        database = FindObjectOfType<DataBaseControl>();
    }

    #region Database
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
            yield return StartCoroutine(LoadPatientRoutine(identifier));
    }

    private IEnumerator LoadPatientRoutine(string identifier)
    {
        yield return StartCoroutine(database.ReadPatientDataRoutine(identifier));
        var patientData = new PatientData(database.patientData.PatientName, database.patientData.PatientAge);
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
        if (!PlayerPrefs.HasKey(_userID)) return new UserData();

        var json = PlayerPrefs.GetString(_userID);
        return JsonUtility.FromJson<UserData>(json);
    }

    public void SavePatientsFromLocal(string _login, PatientSaveData _saveData)
    {
        var json = JsonUtility.ToJson(_saveData);
        PlayerPrefs.SetString(_login, json);
    }

    public PatientSaveData LoadPatientsFromLocal(string _login)
    {
        if (!PlayerPrefs.HasKey(_login)) return new PatientSaveData();

        var json = PlayerPrefs.GetString(_login);
        return JsonUtility.FromJson<PatientSaveData>(json);
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
}
