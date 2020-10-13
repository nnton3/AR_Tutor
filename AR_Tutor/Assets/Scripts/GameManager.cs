using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private string
    email,
    password,
    patientLogin,
    patientName,
    patientAge;

    private DataBaseControl database;
    private LoginUIControl uiControl;
    private AuthUser auth;

    public string Email { get => email; set => email = value; }
    public string Password { get => password; set => password = value; }
    public string PatientLogin { get => patientLogin; set => patientLogin = value; }
    public string PatientName { get => patientName; set => patientName = value; }
    public string PatientAge { get => patientAge; set => patientAge = value; }

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
    }

    private void Start()
    {
        StartCoroutine(MenuProgressRoutine());
    }

    #region Login
    private IEnumerator MenuProgressRoutine()
    {
        while (!auth.IsSignIn)
            yield return null;

        uiControl.LoginPanelActiveSelf = false;

        yield return StartCoroutine(database.ReadUserDataRoutine(auth.NewUser.UserId)); // Load user data

        if (database.userData.patients.Count > 0)                       // If have patients
        {
            userData = database.userData;
            patientsIdentifiers = database.userData.patients;           // Save list of patients
            yield return StartCoroutine(LoadPatientsFromDatabase());    // Load and display patients data
        }

        uiControl.PatientSelectorActiveSelf = true;
    }

    #region User
    public void UpdateUserData(string patient)
    {
        if (!userData.patients.Contains(patient))
            userData.patients.Add(PatientLogin);

        database.WriteUserData(auth.NewUser.UserId, userData);
    }
    #endregion

    #region Patient
    public IEnumerator AddPatient()
    {
        uiControl.AddUserPanelActiveSelf = false;
        yield return StartCoroutine(LoadPatientRoutine(PatientLogin));

        UpdateUserData(PatientLogin);
    }

    public void CreatePatient()
    {
        uiControl.AddUserPanelActiveSelf = false;
        var data = new PatientData(patientName, patientAge);
        uiControl.AddPatientCardInSelector(data, PatientLogin);
        SavePatientInDatabase(data);
        UpdateUserData(PatientLogin);
    }

    private void SavePatientInDatabase(PatientData data)
    {
        database.WritePatientData(PatientLogin, data);
    }

    private IEnumerator LoadPatientsFromDatabase()
    {
        foreach (var identifier in patientsIdentifiers)
            yield return StartCoroutine(LoadPatientRoutine(identifier));
    }

    private IEnumerator LoadPatientRoutine(string identifier)
    {
        yield return StartCoroutine(database.ReadPatientDataRoutine(identifier));
        var patientData = new PatientData(database.patientData.PatientName, database.patientData.PatientAge);
        patients.Add(patientData);
        if (!patientsIdentifiers.Contains(identifier))
            patientsIdentifiers.Add(identifier);

        uiControl.AddPatientCardInSelector(patientData, identifier);
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
