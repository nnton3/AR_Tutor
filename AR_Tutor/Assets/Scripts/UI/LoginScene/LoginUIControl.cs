using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIControl : MonoBehaviour
{
    #region Variables
    private enum AuthType { SignIn, SignUp}

    private AuthType authType;
    private MenuTransitionController transitionController;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField]
    private Button
        skipHelloScreenBtn,
        selectSignInBtn,
        selectSignUpBtn,
        signBtn,
        addPatientBtn,
        addPatientFromCloudBtn;
    [SerializeField] private Text loadStatus;
    [SerializeField]
    private InputField
        emailField,
        passwordField,
        patientLoginFieldForAdd;
    [SerializeField]
    private GameObject
        helloPanel,
        authTypePanel,
        loginPanel,
        addPatientPanel,
        patientSelector,
        patientSelectorContent,
        patientCardPref,
        backBtn;

    private LoginManager loginMenuControl;
    private AuthUser authentication;
    private UserData config;
    private Queue<GameObject> patientsInstances = new Queue<GameObject>();

    public bool HelloPanelSkiped { get; private set; } = false;

    public bool AuthTypePanelActiveSelf
    {
        get => authTypePanel.activeSelf;
        set => authTypePanel.SetActive(value);
    }
    public bool AddUserPanelActiveSelf
    {
        get => addPatientPanel.activeSelf;
        set => addPatientPanel.SetActive(value);
    }
    public bool PatientSelectorActiveSelf
    {
        get => patientSelector.activeSelf;
        set
        {
            if (value)
                transitionController.ActivatePanel(patientSelector);
        }
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
        loginMenuControl = FindObjectOfType<LoginManager>();
        authentication = FindObjectOfType<AuthUser>();
        transitionController = FindObjectOfType<MenuTransitionController>();
    }

    private void BindBtns()
    {
        if (skipHelloScreenBtn != null) skipHelloScreenBtn.onClick.AddListener(() =>
        {
            if (!loginMenuControl.HasEnter())
                transitionController.ActivatePanel(authTypePanel);
            else
                transitionController.ActivatePanel(patientSelector);
        });
        if (selectSignInBtn != null) selectSignInBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignIn));
        if (selectSignUpBtn != null) selectSignUpBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignUp));
        if (addPatientBtn != null) addPatientBtn.onClick.AddListener(() => transitionController.ActivatePanel(addPatientPanel));
        if (addPatientFromCloudBtn != null) addPatientFromCloudBtn.onClick.AddListener(
            () => StartCoroutine(loginMenuControl.AddPatient(patientLoginFieldForAdd.text))
            );
    }

    private void BindFields()
    {
        if (emailField != null) emailField.onValueChanged.AddListener((value) => loginMenuControl.Email = value);
        if (passwordField != null) passwordField.onValueChanged.AddListener((value) => loginMenuControl.Password = value);
    }

    public void AddPatientCardInSelector(PatientData data, string patient)
    {
        if (patientSelectorContent == null) return;
        if (patientCardPref == null) return;

        var card = Instantiate(patientCardPref, patientSelectorContent.transform);
        card.GetComponent<UserCardUI>().Initialize(data, patient);
        card.GetComponent<Button>().onClick.AddListener(() =>
        {
            loginMenuControl.SelectPatient(patient);
        });
        patientsInstances.Enqueue(card);

        UIInstruments.GetSizeForHorizontalGroup(
            layoutGroup,
            patientsInstances.Count,
            card.GetComponent<RectTransform>().sizeDelta.x * card.GetComponent<RectTransform>().localScale.x,
            addPatientBtn.GetComponent<RectTransform>().sizeDelta.x * addPatientBtn.GetComponent<RectTransform>().localScale.x);
    }

    private void OpenAuthPanel(AuthType _authType)
    {
        signBtn.onClick.RemoveAllListeners();
        var btnText = signBtn.GetComponentInChildren<Text>();
        switch (_authType)
        {
            case AuthType.SignIn:
                signBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.EnterRoutine()));
                btnText.text = "войти";
                break;
            case AuthType.SignUp:
                signBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.RegistryRoutine()));
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
