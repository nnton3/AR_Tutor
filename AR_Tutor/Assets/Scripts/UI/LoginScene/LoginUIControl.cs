using UnityEngine;
using UnityEngine.UI;

public class LoginUIControl : MonoBehaviour
{
    #region Variables
    private enum AuthType { SignIn, SignUp}

    private AuthType authType;
    private MenuTransitionController transitionController;
    [SerializeField]
    private Button
        skipHelloScreenBtn,
        selectSignInBtn,
        selectSignUpBtn,
        signBtn,
        addUserBtn,
        createPatientDataBtn,
        addPatientDataBtn;
    [SerializeField] private Text loadStatus;
    [SerializeField]
    private InputField
        emailField,
        passwordField,
        patientLoginFieldForCreate,
        patientLoginFieldForAdd,
        patientNameField,
        patientAgeField;
    [SerializeField]
    private GameObject
        helloPanel,
        authTypePanel,
        loginPanel,
        addUserPanel,
        patientSelector,
        patientSelectorContent,
        patientCardPref,
        backBtn;

    private GameManager loginMenuControl;
    private AuthUser authentication;
    private UserData config;

    public bool HelloPanelSkiped { get; private set; } = false;

    public bool AuthTypePanelActiveSelf
    {
        get => authTypePanel.activeSelf;
        set => authTypePanel.SetActive(value);
    }
    public bool AddUserPanelActiveSelf
    {
        get => addUserPanel.activeSelf;
        set => addUserPanel.SetActive(value);
    }
    public bool PatientSelectorActiveSelf
    {
        get => patientSelector.activeSelf;
        set => patientSelector.SetActive(value);
    }
    public bool LoginPanelActiveSelf
    {
        get => loginPanel.activeSelf;
        set => loginPanel.SetActive(value);
    }
    #endregion

    private void Awake()
    {
        Initialize();

        BindBtns();
        BindFields();

        transitionController.ActivatePanel(helloPanel);
    }

    private void Initialize()
    {
        loginMenuControl = FindObjectOfType<GameManager>();
        authentication = FindObjectOfType<AuthUser>();
        transitionController = FindObjectOfType<MenuTransitionController>();
    }

    private void BindBtns()
    {
        if (skipHelloScreenBtn != null) skipHelloScreenBtn.onClick.AddListener(() => transitionController.ActivatePanel(authTypePanel));
        if (selectSignInBtn != null) selectSignInBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignIn));
        if (selectSignUpBtn != null) selectSignUpBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignUp));
        if (addUserBtn != null) addUserBtn.onClick.AddListener(() =>
        {
            if (addUserPanel != null)
                addUserPanel.SetActive(true);
        });
        if (createPatientDataBtn != null) createPatientDataBtn.onClick.AddListener(() => loginMenuControl.CreatePatient());
        if (addPatientDataBtn != null) addPatientDataBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.AddPatient()));
    }

    private void BindFields()
    {
        if (emailField != null) emailField.onValueChanged.AddListener((value) => loginMenuControl.Email = value);
        if (passwordField != null) passwordField.onValueChanged.AddListener((value) => loginMenuControl.Password = value);
        if (patientLoginFieldForCreate != null) patientLoginFieldForCreate.onValueChanged.AddListener((value) => loginMenuControl.PatientLogin = value);
        if (patientLoginFieldForAdd != null) patientLoginFieldForAdd.onValueChanged.AddListener((value) => loginMenuControl.PatientLogin = value);
        if (patientNameField != null) patientNameField.onValueChanged.AddListener((value) => loginMenuControl.PatientName = value);
        if (patientAgeField != null) patientAgeField.onValueChanged.AddListener((value) => loginMenuControl.PatientAge = value);
    }

    public void AddPatientCardInSelector(PatientData data, string patient)
    {
        if (patientSelectorContent == null) return;
        if (patientCardPref == null) return;

        var card = Instantiate(patientCardPref, patientSelectorContent.transform);
        card.GetComponent<UserCardUI>().Initialize(data.PatientName, data.PatientAge);
        card.GetComponent<Button>().onClick.AddListener(() =>
        {
            loginMenuControl.SelectPatient(patient);
        });
    }

    private void OpenAuthPanel(AuthType _authType)
    {
        signBtn.onClick.RemoveAllListeners();
        var btnText = signBtn.GetComponentInChildren<Text>();
        switch (_authType)
        {
            case AuthType.SignIn:
                signBtn.onClick.AddListener(() => authentication.SignIn(loginMenuControl.Email, loginMenuControl.Password));
                btnText.text = "войти";
                break;
            case AuthType.SignUp:
                signBtn.onClick.AddListener(() => authentication.CreateUser(loginMenuControl.Email, loginMenuControl.Password));
                btnText.text = "регистрация";
                break;
            default:
                break;
        }
        transitionController.ActivatePanel(loginPanel);
        transitionController.SwitchBackBtnRender(true);
        transitionController.AddEventToReturnBtn(() =>
        {
            transitionController.SwitchBackBtnRender(false);
        });
    }
}
