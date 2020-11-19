using UnityEngine;
using UnityEngine.UI;

public class PatientSelectorControl : MonoBehaviour
{
    private StartGamePanelControl startGamePanelControl;
    [SerializeField] private HorizontalLayoutGroup patientSelectorLayoutGroup;
    [SerializeField]
    private GameObject
        patientSelectorContent,
        patientCardPref,
        startGamePanel;
    [SerializeField] private RectTransform openCreatePatientPanelBtn;
    [SerializeField] private HorizontalContentMover patientSelectorMover;

    public void Initialize()
    {
        startGamePanelControl = FindObjectOfType<StartGamePanelControl>();

        Signals.AddPatientEvent.AddListener(AddPatientCardInSelector);
    }

    private void AddPatientCardInSelector(PatientData data, string _identifier)
    {
        if (patientSelectorContent == null) return;
        if (patientCardPref == null) return;

        var card = Instantiate(patientCardPref, patientSelectorContent.transform);
        card.GetComponent<PatientCard>().Initialize(data);
        card.GetComponent<Button>().onClick.AddListener(() =>
        {
            startGamePanelControl.Initialize(data, _identifier);
            startGamePanel.SetActive(true);
        });
        card.transform.SetAsFirstSibling();

        UIInstruments.GetSizeForHorizontalGroup(
            patientSelectorLayoutGroup,
            patientSelectorContent.transform.childCount - 2,
            card.GetComponent<RectTransform>().sizeDelta.x * card.GetComponent<RectTransform>().localScale.x,
            openCreatePatientPanelBtn.sizeDelta.x * openCreatePatientPanelBtn.localScale.x * 2);

        patientSelectorMover.CalculateMinPos();
    }
}
