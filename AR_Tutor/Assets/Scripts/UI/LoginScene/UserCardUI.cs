using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCardUI : MonoBehaviour
{
    [SerializeField] private Text patientName, age, identifier;
    [SerializeField] private Image img;

    public void Initialize(PatientData _data, string _identifier)
    {
        patientName.text += _data.PatientName;
        age.text += _data.PatientAge;
        identifier.text += _identifier;
        img.sprite = _data.img;
    }
}
