using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseControl : MonoBehaviour
{
    private FirebaseDatabase database = null;
    private DataSnapshot snapshot = null;
    private string userID;
    public UserData userData { get; private set; }
    public PatientData patientData { get; private set; }

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ar-tutor.firebaseio.com/");

        database = FirebaseDatabase.DefaultInstance;
        //database.App.Options.DatabaseUrl = new System.Uri("https://ar-tutor.firebaseio.com/");
    }

    #region User data
    public void WriteUserData(string userId, UserData config)
    {
        database.GetReference("users").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(config));
    }

    public IEnumerator ReadUserDataRoutine(string userId)
    {
        var readTask = database.GetReference("users")
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("bind snapshot");
                        snapshot = task.Result;
                    }
                });

        yield return new WaitUntil(() => readTask.IsCompleted);

        userData = new UserData(new List<string>());
        if (snapshot != null)
        {
            if (snapshot.HasChild(userId))
            {
                Debug.Log("load users from cloud");
                var json = snapshot.Child(userId).GetRawJsonValue();
                userData = JsonUtility.FromJson<UserData>(json);
            }
        }
        else Debug.Log("user not load and create as new");
    }
    #endregion

    #region Patient data
    public void WritePatientData(string login, PatientData data)
    {
        database.GetReference("patients").Child(login).SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }

    public IEnumerator ReadPatientDataRoutine(string login)
    {
        var readTask = database.GetReference("patients")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                    snapshot = task.Result;
            });

        yield return new WaitUntil(() => readTask.IsCompleted);

        if (snapshot.HasChild(login))
        {
            var json = snapshot.Child(login).GetRawJsonValue();
            patientData = JsonUtility.FromJson<PatientData>(json);
        }
        else patientData = new PatientData();
    }
    #endregion
}
