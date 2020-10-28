using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonsCardController : MonoBehaviour
{
    public bool IsChanged;
    public Color ButtonColor;
    public Material ButtonMaterial;
    public GameObject Effect;
    public AudioClip Clip;
    public void Start()
    {
        
    }
    public void CardChanged()
    {
        if (!IsChanged)
        {
            gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            IsChanged = true;
            FindObjectOfType<ButtonsSubstrateController>().AddCards();
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            IsChanged = false;
            FindObjectOfType<ButtonsSubstrateController>().RemoveCards();
        }
        
    }
}
