using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

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

    private int failedSigning;
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
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            Signals.ShowNotification.Invoke("Ошибка! Не удалось войти в учетную запись, проверьте правильность ввода эл.почты и пароля");
        else
        {
            yield return StartCoroutine(auth.SignInRoutine(Email, Password));

            if (auth.NewUser != null)
            {
                PlayerPrefs.SetString("lastUser", auth.UserID);
                ConfigurateUserData(auth.UserID);
                uiControl.PatientSelectorActiveSelf = true;
            }
            else
            {
                failedSigning++;
                if (failedSigning > 2)
                {
                    Signals.ResetPasswordEvent.Invoke();
                    failedSigning = 0;
                }
            }
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
        if (!EmailIsValid())
            Signals.ShowNotification.Invoke("Ошибка! Электронная почта указана неверно");
        else if (PasswordIsValid())
            Signals.ShowNotification.Invoke("Ошибка! Пароль должен содержать не менее 6 символов и включать в себя английские буквы и цифры.");
        else
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
    }

    private bool EmailIsValid()
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            email = Regex.Replace(
                email, @"(@)(.+)$",
                DomainMapper,
                RegexOptions.None, 
                TimeSpan.FromMilliseconds(200));

            string DomainMapper(Match match)
            {
                var idn = new IdnMapping();
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, 
                TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    private bool PasswordIsValid()
    {
        if (string.IsNullOrEmpty(password)) return false;
        if (password.Length < 6) return false;
        if (!Regex.IsMatch(password, @"[0-9a-zA-Z]{8}")) return false;

        return true;
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
        if (PatientDataIsValid(saveSystem.LoadedPatient))
        {
            uiControl.AddPatientCardInSelector(saveSystem.LoadedPatient, _patientLogin);
            AddPatientToUser(_patientLogin);
        }
        else Signals.ShowNotification.Invoke("Ошибка! Пациент не был загружен");
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

    private bool PatientDataIsValid(PatientData _data)
    {
        if (string.IsNullOrEmpty(_data.PatientName)) return false;
        if (string.IsNullOrEmpty(_data.PatientAge)) return false;

        return true;
    }
    #endregion
    #endregion
}
