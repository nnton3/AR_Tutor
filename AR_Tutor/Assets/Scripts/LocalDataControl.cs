using UnityEngine;
using System.Collections;

public class LocalDataControl : MonoBehaviour
{
    public void SaveLocalData(UserData config)
    {
        var jsonFormat = JsonUtility.ToJson(config);

        PlayerPrefs.SetString("config", jsonFormat);
    }

    public UserData LoadLocalData()
    {
        var jsonFormat = PlayerPrefs.GetString("config");

        if (jsonFormat != null)
            return JsonUtility.FromJson<UserData>(jsonFormat);
        else return new UserData();
    }
}
