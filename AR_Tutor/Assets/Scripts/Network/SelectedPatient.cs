using UnityEngine;

public class SelectedPatient : MonoBehaviour
{
    public string PatientLogin { get; private set; } = null;
    public string UserLogin { get; private set; } = null;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void SetUserLogin(string _mail)
    {
        var lastCharIndex = _mail.IndexOf("@");
        UserLogin = _mail.Remove(lastCharIndex);
    }

    public void SetSelectedPatientLogin(string _login)
    {
        PatientLogin = _login;
    }
}
