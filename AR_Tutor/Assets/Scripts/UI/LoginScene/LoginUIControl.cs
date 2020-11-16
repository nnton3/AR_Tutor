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
    [SerializeField]
    private Button
        skipHelloScreenBtn,
        selectSignInBtn,
        selectSignUpBtn,
        signBtn,
        openCreatePatientPanelBtn,
        openLoadPatientPanelBtn,
        loadPatientBtn,
        resetYesBtn,
        resetNoBtn,
        openPatientDatasPanelBtn;
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
        createPatientPanel,
        loadPatientPanel,
        patientSelector,
        patientsDataPanel,
        resetPasswordPanel;

    private LoginManager loginMenuControl;
    private AuthUser authentication;
    private UserData config;

    public bool AuthTypePanelActiveSelf
    {
        get => authTypePanel.activeSelf;
        set => authTypePanel.SetActive(value);
    }
    public bool AddUserPanelActiveSelf
    {
        get => createPatientPanel.activeSelf;
        set => createPatientPanel.SetActive(value);
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
    }

    private void Start()
    {
        transitionController.ActivatePanel(helloPanel);
        Signals.ApplicationStartEvent.Invoke();
    }

    private void Initialize()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
        authentication = FindObjectOfType<AuthUser>();
        transitionController = FindObjectOfType<MenuTransitionController>();

        Signals.ResetPasswordEvent.AddListener(() => resetPasswordPanel.SetActive(true));
    }

    private void BindBtns()
    {
        if (skipHelloScreenBtn != null) skipHelloScreenBtn.onClick.AddListener(() =>
        {
            if (!LoginManager.HasEnter)
            {
                transitionController.ActivatePanel(authTypePanel);
                Signals.EnterToAuthWindowEvent.Invoke();
                transitionController.AddEventToReturnBtn(() => Signals.StopPlayAudioEvent.Invoke());
            }
            else
            {
                transitionController.ActivatePanel(patientSelector);
                transitionController.AddEventToReturnBtn(() => Signals.StopPlayAudioEvent.Invoke());
            }
        });
        if (selectSignInBtn != null) selectSignInBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignIn));
        if (selectSignUpBtn != null) selectSignUpBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignUp));
        if (openCreatePatientPanelBtn != null) openCreatePatientPanelBtn.onClick.AddListener(() =>
        {
            transitionController.SwitchBackBtnRender(true);
            transitionController.ActivatePanel(createPatientPanel);
            Signals.OpenCreatePatientPanelEvent.Invoke();
            transitionController.AddEventToReturnBtn(() => transitionController.SwitchBackBtnRender(false));
        });
        if (openLoadPatientPanelBtn != null) openLoadPatientPanelBtn.onClick.AddListener(() =>
        {
            transitionController.SwitchBackBtnRender(true);
            transitionController.ActivatePanel(loadPatientPanel);
            transitionController.AddEventToReturnBtn(() => transitionController.SwitchBackBtnRender(false));
        });
        if (loadPatientBtn != null) loadPatientBtn.onClick.AddListener(
            () => StartCoroutine(loginMenuControl.AddPatient(patientLoginFieldForAdd.text)));
        if (resetYesBtn != null) resetYesBtn.onClick.AddListener(() => authentication.ResetPassword(loginMenuControl.Email));
        if (resetNoBtn != null) resetNoBtn.onClick.AddListener(() => resetPasswordPanel.SetActive(false));
        if (openPatientDatasPanelBtn != null) openPatientDatasPanelBtn.onClick.AddListener(() => patientsDataPanel.SetActive(true));
    }

    private void BindFields()
    {
        if (emailField != null) emailField.onValueChanged.AddListener((value) => loginMenuControl.Email = value);
        if (passwordField != null) passwordField.onValueChanged.AddListener((value) => loginMenuControl.Password = value);
    }

    private void OpenAuthPanel(AuthType _authType)
    {
        signBtn.onClick.RemoveAllListeners();
        var btnText = signBtn.GetComponentInChildren<Text>();
        UnityEngine.Events.UnityAction hintAction = null;
        switch (_authType)
        {
            case AuthType.SignIn:
                signBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.EnterRoutine()));
                btnText.text = "войти";
                hintAction = () => Signals.SignInEvent.Invoke();
                break;
            case AuthType.SignUp:
                signBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.RegistryRoutine()));
                btnText.text = "регистрация";
                hintAction = () => Signals.SignUpEvent.Invoke();
                break;
            default:
                break;
        }
        transitionController.ActivatePanel(loginPanel);
        transitionController.SwitchBackBtnRender(true);
        hintAction();
        transitionController.AddEventToReturnBtn(() =>
        {
            Signals.StopPlayAudioEvent.Invoke();
            transitionController.SwitchBackBtnRender(false);
        });
    }
}
