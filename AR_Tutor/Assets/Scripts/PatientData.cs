using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PatientData
{
    public string PatientName;
    public string PatientAge;

    public PatientData(string patientName, string patientAge)
    {
        PatientName = patientName;
        PatientAge = patientAge;
    }
}

[Serializable]
public struct PatientGameData
{
    public VariantCategoryData[] VariantGameConfig;

    public PatientGameData(VariantCategoryData[] variantGameConfig = null)
    {
        if (variantGameConfig == null)
            VariantGameConfig = Resources.Load<VariantGameConfig>("DefaultVariantGameConfig").Categories;
        else  VariantGameConfig = variantGameConfig;
    }
}
