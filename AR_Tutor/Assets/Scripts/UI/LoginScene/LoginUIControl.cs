﻿using System.Collections;
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
        openCreatePatientPanelBtn,
        openLoadPatientPanelBtn,
        loadPatientBtn,
        resetYesBtn,
        resetNoBtn;
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
        patientSelectorContent,
        patientCardPref,
        resetPasswordPanel;

    private LoginManager loginMenuControl;
    private AuthUser authentication;
    private UserData config;
    private AudioHintControl hintControl;
    private Queue<GameObject> patientsInstances = new Queue<GameObject>();

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
    }

    private void Initialize()
    {
        loginMenuControl = FindObjectOfType<LoginManager>();
        authentication = FindObjectOfType<AuthUser>();
        transitionController = FindObjectOfType<MenuTransitionController>();
        hintControl = FindObjectOfType<AudioHintControl>();

        Signals.ResetPasswordEvent.AddListener(() => resetPasswordPanel.SetActive(true));
    }

    private void BindBtns()
    {
        if (skipHelloScreenBtn != null) skipHelloScreenBtn.onClick.AddListener(() =>
        {
            hintControl.StopAudio();

            if (!loginMenuControl.HasEnter())
            {
                hintControl.PlayAuthHint();
                transitionController.ActivatePanel(authTypePanel);
                transitionController.AddEventToReturnBtn(() => hintControl.StopAudio());
            }
            else
            {
                transitionController.ActivatePanel(patientSelector);
                transitionController.AddEventToReturnBtn(() => hintControl.StopAudio());
            }
        });
        if (selectSignInBtn != null) selectSignInBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignIn));
        if (selectSignUpBtn != null) selectSignUpBtn.onClick.AddListener(() => OpenAuthPanel(AuthType.SignUp));
        if (openCreatePatientPanelBtn != null) openCreatePatientPanelBtn.onClick.AddListener(() =>
        {
            transitionController.SwitchBackBtnRender(true);
            transitionController.ActivatePanel(createPatientPanel);
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
        card.transform.SetAsFirstSibling();

        UIInstruments.GetSizeForHorizontalGroup(
            layoutGroup,
            patientsInstances.Count,
            card.GetComponent<RectTransform>().sizeDelta.x * card.GetComponent<RectTransform>().localScale.x,
            openCreatePatientPanelBtn.GetComponent<RectTransform>().sizeDelta.x * openCreatePatientPanelBtn.GetComponent<RectTransform>().localScale.x * 2);
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
                hintAction = () => hintControl.PlaySignInHint();
                break;
            case AuthType.SignUp:
                signBtn.onClick.AddListener(() => StartCoroutine(loginMenuControl.RegistryRoutine()));
                btnText.text = "регистрация";
                hintAction = () => hintControl.PlaySingUpHint();
                break;
            default:
                break;
        }
        transitionController.ActivatePanel(loginPanel);
        transitionController.SwitchBackBtnRender(true);
        hintAction();
        transitionController.AddEventToReturnBtn(() =>
        {
            hintControl.StopAudio();
            transitionController.SwitchBackBtnRender(false);
        });
    }
}
