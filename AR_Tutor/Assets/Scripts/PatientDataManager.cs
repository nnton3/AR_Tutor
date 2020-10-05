using UnityEngine;

public enum GameName { Variant = 0, Buttons = 1, WordBook = 2, WordComposing = 3}

public class PatientDataManager : MonoBehaviour
{
    #region Variables
    private SaveSystem saveSystem;
    private PatientGameData? patientData;
    [SerializeField] private PatientGameData testData;
    [SerializeField] private string login;
    #endregion

    public PatientGameData? GetPatientData()
    {
        return patientData;
    }

    public string GetPatientLogin()
    {
        return login;
    }

    public void Initialize()
    {
        saveSystem = FindObjectOfType<SaveSystem>();
        Signals.AddCardEvent.AddListener(AddCardToCategory);
        patientData = new PatientGameData(null);
        var loadData = saveSystem.LoadPatientDataFromLocal(login);

        if (loadData != null) patientData = loadData;

        Signals.SwitchCardVisibleEvent.AddListener(SwitchCardVisible);
        Signals.DeleteCardFromCategory.AddListener(DeleteCardFromCategory);
        testData = patientData.Value;
    }

    public void UpdatePatientData()
    {
        saveSystem.SavePatientDataFromLocal(patientData, login);
    }

    #region Category management
    public void HideCategory(GameName game, int index)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                    patientData.Value.VariantGameConfig[index].visible = false;
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void ShowCategory(GameName game, int index)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                    patientData.Value.VariantGameConfig[index].visible = true;
                UpdatePatientData();
                break;
            default:
                break;
        }
    }
    #endregion

    #region Card management
    public void SwitchCardVisible(GameName _game, int _categoryIndex, string _cardKey)
    {
        switch (_game)
        {
            case GameName.Variant:
                if (patientData.HasValue)
                {
                    int index = patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.IndexOf(_cardKey);
                    patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue[index] = 
                        !patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue[index];
                }
                    
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public bool CardExists(GameName game, int categoryIndex, string cardIndex)
    {
        if (patientData.Value.VariantGameConfig[categoryIndex].cardKeys.Contains(cardIndex))
            return true;

        return false;
    }

    public void AddCardToCategory(GameName game, int _categoryIndex, string _cardKey)
    {
        switch (game)
        {
            case GameName.Variant:
                if (patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Contains(_cardKey)) return;

                patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Add(_cardKey);
                patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue.Add(true);
                UpdatePatientData();
                break;
            default:
                break;
        }
    }

    public void DeleteCardFromCategory(GameName game, int _categoryIndex, string _cardKey)
    {
        switch (game)
        {
            case GameName.Variant:
                patientData.Value.VariantGameConfig[_categoryIndex].cardKeys.Remove(_cardKey);
                patientData.Value.VariantGameConfig[_categoryIndex].cardVisibleValue.Remove(true);
                UpdatePatientData();
                break;
            default:
                break;
        }
    }
    #endregion
}
