using UnityEngine;
using UnityEngine.UI;

public class PatientDataCard : MonoBehaviour
{
    [SerializeField] private Text patientName, patientAge, patientIdentifier;
    [SerializeField] private Image img;

    public void Initialize(PatientData _data, string _identifier)
    {
        patientName.text += _data.PatientName;
        patientAge.text += _data.PatientAge;
        patientIdentifier.text += _identifier;
        img.sprite = _data.img;
    }
}
