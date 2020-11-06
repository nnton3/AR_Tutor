using UnityEngine;
using UnityEngine.UI;

public class PatientCard : MonoBehaviour
{
    [SerializeField] private Text patientName;
    [SerializeField] private Image img;

    public void Initialize(PatientData _data)
    {
        patientName.text += _data.PatientName;
        patientName.text += $", {_data.PatientAge}";
        img.sprite = _data.img;
    }
}
