using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UniRx;

public class LoginUIControl : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Button
        signUpBtn,
        signInBtn,
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
        loginPanel,
        addUserPanel,
        patientSelector,
        patientSelectorContent,
        patientCardPref;

    private GameManager gameManager;
    private AuthUser authentication;
    private UserData config;

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
    }

    private void Initialize()
    {
        gameManager = FindObjectOfType<GameManager>();
        authentication = FindObjectOfType<AuthUser>();
    }

    private void BindBtns()
    {
        if (signUpBtn != null) signUpBtn.onClick.AddListener(() => authentication.CreateUser(gameManager.Email, gameManager.Password));
        if (signInBtn != null) signInBtn.onClick.AddListener(() => authentication.SignIn(gameManager.Email, gameManager.Password));
        if (addUserBtn != null) addUserBtn.onClick.AddListener(() =>
        {
            if (addUserPanel != null)
                addUserPanel.SetActive(true);
        });
        if (createPatientDataBtn != null) createPatientDataBtn.onClick.AddListener(() => gameManager.CreatePatient());
        if (addPatientDataBtn != null) addPatientDataBtn.onClick.AddListener(() => StartCoroutine(gameManager.AddPatient()));
    }

    private void BindFields()
    {
        if (emailField != null) emailField.onValueChanged.AddListener((value) => gameManager.Email = value);
        if (passwordField != null) passwordField.onValueChanged.AddListener((value) => gameManager.Password = value);
        if (patientLoginFieldForCreate != null) patientLoginFieldForCreate.onValueChanged.AddListener((value) => gameManager.PatientLogin = value);
        if (patientLoginFieldForAdd != null) patientLoginFieldForAdd.onValueChanged.AddListener((value) => gameManager.PatientLogin = value);
        if (patientNameField != null) patientNameField.onValueChanged.AddListener((value) => gameManager.PatientName = value);
        if (patientAgeField != null) patientAgeField.onValueChanged.AddListener((value) => gameManager.PatientAge = value);
    }

    public void AddPatientCardInSelector(PatientData data, string patient)
    {
        if (patientSelectorContent == null) return;
        if (patientCardPref == null) return;

        var card = Instantiate(patientCardPref, patientSelectorContent.transform);
        card.GetComponent<UserCardUI>().Initialize(data.PatientName, data.PatientAge);
        card.GetComponent<Button>().onClick.AddListener(() =>
        {
            gameManager.SelectPatient(patient);
        });
    }
}
