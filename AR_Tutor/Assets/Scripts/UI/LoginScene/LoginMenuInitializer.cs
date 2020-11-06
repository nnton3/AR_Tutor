using UnityEngine;
using System.Collections;

public class LoginMenuInitializer : MonoBehaviour
{
    private PatientSelectorControl selectorControl;
    private PatientDatasPanelControl patientDatasControl;
    private LoginManager loginManager;

    private void Awake()
    {
        selectorControl = FindObjectOfType<PatientSelectorControl>();
        patientDatasControl = FindObjectOfType<PatientDatasPanelControl>();
        loginManager = FindObjectOfType<LoginManager>();

        if (selectorControl != null) selectorControl.Initialize();
        if (patientDatasControl != null) patientDatasControl.Initialize();
        if (loginManager != null) loginManager.Initialize();
    }
}
