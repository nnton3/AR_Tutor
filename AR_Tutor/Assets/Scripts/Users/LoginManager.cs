using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class LoginManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private string
    email,
    password;

    private DataBaseControl database;
    private LoginUIControl uiControl;
    private AuthUser auth;
    private UserSaveSystem saveSystem;

    public string Email { get => email; set => email = value; }
    public string Password { get => password; set => password = value; }

    [SerializeField] private UserData userData = new UserData(new List<string>());
    [SerializeField] private List<PatientData> patients = new List<PatientData>();
    [SerializeField] private List<string> patientsIdentifiers = new List<string>();

    [SerializeField] private string selectedPatient;
    #endregion

    private void Awake()
    {
        database = FindObjectOfType<DataBaseControl>();
        uiControl = FindObjectOfType<LoginUIControl>();
        auth = FindObjectOfType<AuthUser>();
        saveSystem = FindObjectOfType<UserSaveSystem>();

        if (HasEnter())
        {
            auth.SetUserID(PlayerPrefs.GetString("lastUser"));
            ConfigurateUserData(auth.UserID);
        }
    }

    public bool HasEnter()
    {
        return PlayerPrefs.HasKey("lastUser");
    }

    #region Login
    public IEnumerator EnterRoutine()
    {
        if (Email == "" || Password == "")
            Signals.ShowNotification.Invoke("Некорректный адрес эектронной почты или пароль");
        else
        {
            yield return StartCoroutine(auth.SignInRoutine(Email, Password));

            if (auth.NewUser != null)
            {
                PlayerPrefs.SetString("lastUser", auth.UserID);
                ConfigurateUserData(auth.UserID);
            }

            uiControl.PatientSelectorActiveSelf = true;
        }
    }

    public void ConfigurateUserData(string _userID)
    {
        if (_userID == null) return;
        if (saveSystem.HasUserData(_userID))
        {
            userData = saveSystem.LoadUserData(_userID);
            if (userData.patients != null)
                foreach (var patient in userData.patients)
                {
                    var data = saveSystem.LoadPatientsFromLocal(patient);
        
                    if (!string.IsNullOrWhiteSpace(data.PatientName))
                        uiControl.AddPatientCardInSelector(data, patient);
                }
        }
    }

    public IEnumerator RegistryRoutine()
    {
        var task = auth.CreateUser(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (auth.NewUser != null)
        {
            PlayerPrefs.SetString("lastUser", auth.UserID);
            ConfigurateUserData(auth.UserID);
        }

        uiControl.PatientSelectorActiveSelf = true;
    }

    #region User
    public void AddPatientToUser(string patient)
    {
        if (!userData.patients.Contains(patient))
            userData.patients.Add(patient);

        saveSystem.SaveUserData(auth.UserID, userData);
    }
    #endregion

    #region Patient
    public IEnumerator AddPatient(string _patientLogin)
    {
        uiControl.AddUserPanelActiveSelf = false;
        yield return StartCoroutine(saveSystem.LoadPatientFromCloudRoutine(_patientLogin));

        uiControl.AddPatientCardInSelector(saveSystem.LoadedPatient, _patientLogin);
        AddPatientToUser(_patientLogin);
    }

    public void SelectPatient(string _patinetLogin)
    {
        var selectPatientData = FindObjectOfType<SelectedPatient>();
        if (selectPatientData == null)
        {
            var obj = Instantiate(new GameObject("SelectedPatientData"));
            selectPatientData = obj.AddComponent<SelectedPatient>();
        }
        selectPatientData.SetUserLogin(email);
        selectPatientData.SetSelectedPatientLogin(_patinetLogin);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    #endregion
    #endregion
}
