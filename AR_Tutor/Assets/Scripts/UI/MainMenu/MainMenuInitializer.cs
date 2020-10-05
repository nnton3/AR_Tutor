using UnityEngine;
using System.Collections;

public class MainMenuInitializer : MonoBehaviour
{
    #region Variables
    private VariantGameMenu variantGameUI;
    private CardStorage cardStorage;
    private MainMenuUIControl mainMenuUI;
    private PatientDataManager patientManager;
    private SaveSystem saveSystem;
    private LibraryUIControl library;
    #endregion

    private void Awake()
    {
        variantGameUI = FindObjectOfType<VariantGameMenu>();
        cardStorage = FindObjectOfType<CardStorage>();
        mainMenuUI = FindObjectOfType<MainMenuUIControl>();
        patientManager = FindObjectOfType<PatientDataManager>();
        saveSystem = FindObjectOfType<SaveSystem>();
        library = FindObjectOfType<LibraryUIControl>();

        if (patientManager != null) patientManager.Initialize();
        if (saveSystem != null) saveSystem.Initialize();
        if (cardStorage != null) cardStorage.Initialize();
        if (variantGameUI != null) variantGameUI.Initialize();
        if (library != null) library.Initialize();
        if (mainMenuUI != null) mainMenuUI.Initialize();
    }
}
