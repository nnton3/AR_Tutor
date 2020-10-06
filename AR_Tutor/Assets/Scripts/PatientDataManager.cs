using System.Collections.Generic;
using UnityEngine;

public enum GameName { Variant = 0, Buttons = 1, WordBook = 2, WordComposing = 3}

public class PatientDataManager : MonoBehaviour
{
    #region Variables
    private SaveSystem saveSystem;
    private CategoryStorage categoryStorage;
    public PatientGameData PatientData { get; private set; }
    [SerializeField] private PatientGameData testData;
    [SerializeField] private string login;
    #endregion

    public string GetPatientLogin()
    {
        return login;
    }

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        categoryStorage = FindObjectOfType<CategoryStorage>();
        PatientData = new PatientGameData();
        var loadData = saveSystem.LoadPatientDataFromLocal(login);

        if (loadData != null) PatientData = loadData.Value;
        else PatientData = new PatientGameData();

        Signals.AddCardEvent.AddListener(AddCardToCategory);
        Signals.DeleteCardFromCategory.AddListener(DeleteCardFromCategory);
        Signals.SwitchCardVisibleEvent.AddListener(SwitchCardVisible);
        Signals.SwitchCategoryVisibleEvent.AddListener(SwitchCategoryVisible);
        testData = PatientData;
    }

    public void UpdatePatientData()
    {
        saveSystem.SavePatientDataFromLocal(PatientData, login);
    }

    #region Category management
    public void SwitchCategoryVisible(string _categoryKey, bool _visible)
    {
        var index = GetCategoryIndex(_categoryKey);
        PatientData.CategoriesVisible[index] = _visible;
        UpdatePatientData();
    }
    #endregion

    #region Card management
    public void SwitchCardVisible(string _categoryKey, string _cardKey, bool _visible)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        var cardIndex = GetCardIndex(_categoryKey, _cardKey);

        var data = PatientData;
        data.CardsVisible[categoryIndex][cardIndex] = _visible;
        PatientData = data;
        UpdatePatientData();
    }

    public bool CardExists(GameName game, string categoryIndex, string cardIndex)
    {
        if (GetCardIndex(categoryIndex, cardIndex) < 0)
            return false;

        return true;
    }

    public void AddCardToCategory(string _categoryIndex, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryIndex);
        if (categoryIndex < 0) return;
        if (PatientData.CardKeys[categoryIndex].Contains(_cardKey)) return;

        PatientData.CardKeys[categoryIndex].Add(_cardKey);
    }

    public void DeleteCardFromCategory(string _categoryIndex, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryIndex);
        if (categoryIndex < 0) return;
        if (!PatientData.CardKeys[categoryIndex].Contains(_cardKey)) return;

        PatientData.CardKeys[categoryIndex].Remove(_cardKey);
    }
    #endregion

    public int GetCategoryIndex(string _categoryKey)
    {
        var targetCategory = PatientData.CategoriesKeys.Find((key) => key == _categoryKey);
        if (targetCategory == null) return -1;
        return PatientData.CategoriesKeys.IndexOf(targetCategory);
    } 

    public int GetCardIndex(string _categoryKey, string _cardKey)
    {
        var categoryIndex = GetCategoryIndex(_categoryKey);
        if (categoryIndex < 0) return -1;
        var targetCard = PatientData.CardKeys[categoryIndex].Find((key) => key == _cardKey);
        if (targetCard == null) return -1;
        return PatientData.CardKeys[categoryIndex].IndexOf(targetCard);
    }
}
