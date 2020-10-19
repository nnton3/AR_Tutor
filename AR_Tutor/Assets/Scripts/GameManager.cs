using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

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
    private UserSaveSystem saveSystem;

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
        saveSystem = FindObjectOfType<UserSaveSystem>();
    }

    #region Login
    //private IEnumerator MenuProgressRoutine()
    //{
    //    while (!auth.IsSignIn)
    //        yield return null;

    //    uiControl.LoginPanelActiveSelf = false;

    //    yield return StartCoroutine(database.ReadUserDataRoutine(auth.NewUser.UserId)); // Load user data

    //    if (database.userData.patients.Count > 0)                       // If have patients
    //    {
    //        userData = database.userData;
    //        patientsIdentifiers = database.userData.patients;           // Save list of patients
    //        yield return StartCoroutine(LoadPatientsFromDatabase());    // Load and display patients data
    //    }

    //    uiControl.PatientSelectorActiveSelf = true;
    //}
    public IEnumerator EnterRoutine()
    {
        if (Email == "" || Password == "")
            Debug.Log("Email or password are not correct");
        else
        {
            yield return StartCoroutine(auth.SignInRoutine(Email, Password));

            if (auth.NewUser != null)
            {
                var userData = saveSystem.LoadUserData(auth.NewUser.UserId);
                if (userData.patients != null)
                    foreach (var patient in userData.patients)
                    {
                        var saveData = saveSystem.LoadPatientsFromLocal(patient);

                        Sprite targetSprite = null;
                        var texture = saveSystem.LoadImage(saveData.imgAddress);

                        if (texture != null)
                            targetSprite = Sprite.Create(
                                texture,
                                new Rect(0, 0, texture.width, texture.width),
                                Vector2.zero);

                        var patientData = new PatientData(saveData.PatientName, saveData.PatientAge, targetSprite);
                        uiControl.AddPatientCardInSelector(patientData, PatientLogin);
                    }
            }

        }
    }

    public void TryToRegistry()
    {

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
        yield return null;//StartCoroutine(LoadPatientRoutine(PatientLogin));

        UpdateUserData(PatientLogin);
    }

    public void CreatePatient()
    {
        uiControl.AddUserPanelActiveSelf = false;
        var data = new PatientData(patientName, patientAge);
        uiControl.AddPatientCardInSelector(data, PatientLogin);
        saveSystem.SavePatientInDatabase(PatientLogin, data);
        UpdateUserData(PatientLogin);
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
