using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UserData
{
    public List<string> patients;

    public UserData(List<string> _patients)
    {
        patients = _patients;
    }
}
