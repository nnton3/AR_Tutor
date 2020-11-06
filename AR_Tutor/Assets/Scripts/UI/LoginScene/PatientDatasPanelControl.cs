using UnityEngine;
using UnityEngine.UI;

public class PatientDatasPanelControl : MonoBehaviour
{
    [SerializeField] private GameObject patinetDatasPanel, patientsDataContent, patientDataCardPref;
    [SerializeField] private VerticalLayoutGroup patientDatasLayoutGroup;
    [SerializeField] private Button closeBtn;

    public void Initialize()
    {
        if (closeBtn != null) closeBtn.onClick.AddListener(() => patinetDatasPanel.SetActive(false));

        Signals.AddPatientEvent.AddListener(AddPatientInDataPanel);
    }

    public void AddPatientInDataPanel(PatientData _data, string _identifier)
    {
        if (patientsDataContent == null) return;
        if (patientDataCardPref == null) return;

        var card = Instantiate(patientDataCardPref, patientsDataContent.transform);
        card.GetComponent<PatientDataCard>().Initialize(_data, _identifier);

        card.transform.SetAsFirstSibling();

        UIInstruments.GetSizeForVerticalGroup(
            patientDatasLayoutGroup,
            patientsDataContent.transform.childCount,
            card.GetComponent<RectTransform>().sizeDelta.y * card.GetComponent<RectTransform>().localScale.y);
    }
}
