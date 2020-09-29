using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserCardUI : MonoBehaviour
{
    [SerializeField] private Text patientName, age;

    public void Initialize(string _name, string _age)
    {
        patientName.text = _name;
        age.text = _age;
    }
}
