using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenControl : MonoBehaviour
{
    [SerializeField] private GameObject screen;

    private void Awake()
    {
        Signals.ShowLoadScreen.AddListener(ShowScreen);
    }

    private void ShowScreen()
    {
        screen.SetActive(true); 
    }
}
