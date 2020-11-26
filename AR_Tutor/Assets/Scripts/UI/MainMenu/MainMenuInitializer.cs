using UnityEngine;

public class MainMenuInitializer : MonoBehaviour
{
    #region Variables
    private VariantGameMenu variantGameUI;
    private WordBookMenuControl wordbookGameUI;
    private WordComposingMenuControl wordCompositngUI;
    private CardStorage cardStorage;
    private CategoryStorage categoryStorage;
    private MainMenuUIControl mainMenuUI;
    private PatientDataManager patientManager;
    private SaveSystem saveSystem;
    private CardLibraryUIControl cardLibrary;
    private CategoryLibraryUIControl categoryLibrary;
    #endregion

    private void Awake()
    {
        variantGameUI = FindObjectOfType<VariantGameMenu>();
        wordbookGameUI = FindObjectOfType<WordBookMenuControl>();
        wordCompositngUI = FindObjectOfType<WordComposingMenuControl>();
        cardStorage = FindObjectOfType<CardStorage>();
        categoryStorage = FindObjectOfType<CategoryStorage>();
        mainMenuUI = FindObjectOfType<MainMenuUIControl>();
        patientManager = FindObjectOfType<PatientDataManager>();
        saveSystem = FindObjectOfType<SaveSystem>();
        cardLibrary = FindObjectOfType<CardLibraryUIControl>();
        categoryLibrary = FindObjectOfType<CategoryLibraryUIControl>();

        if (patientManager != null) patientManager.Initialize();
        if (saveSystem != null) saveSystem.Initialize();
        if (categoryStorage != null) categoryStorage.Initialize();
        if (cardStorage != null) cardStorage.Initialize();
        if (variantGameUI != null) variantGameUI.Initialize();
        //if (wordbookGameUI != null) wordbookGameUI.Initialize();
        //if (wordCompositngUI != null) wordCompositngUI.Initialize();
        if (categoryLibrary != null) categoryLibrary.Initialize();
        if (cardLibrary != null) cardLibrary.Initialize();
        if (mainMenuUI != null) mainMenuUI.Initialize();
    }
}
