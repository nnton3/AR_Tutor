using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using JetBrains.Annotations;

public class ButtonsSubstrateController : MonoBehaviour
{
    public GameObject[] Cards;
    public List<GameObject> AllCards;
    public List<GameObject> ChangedCards;
    public int ChangedCardsNumber;
    public GameObject Content;
    public GameObject LeftBtn;
    public GameObject RightBtn;
    public Transform StartPosContent;
    public Transform FinalPosContent;
    public Transform TempPosContent;
    public float transitSpeed = 10f;
    public bool Left;
    public bool Right;
    private ButtonsSceneController _ButtonsSceneController;


    void Start()
    {
        _ButtonsSceneController = FindObjectOfType<ButtonsSceneController>();
        AllCards = new List<GameObject>();
        for (int i = 0; i < Cards.Length; i++)
        {
            AllCards.Add(Cards[i]);
        }
        Debug.Log(Content.transform.localPosition);
    }
    

    private void Update()
    {
        if(ChangedCardsNumber == 2)
        {
            FindObjectOfType<ButtonsSceneController>().ActivateScene();

            gameObject.SetActive(false);
        }
    }
    public void AddCards()
    {
        for (int i = 0; i < AllCards.Count; i++)
        {
            if (AllCards[i].GetComponent<ButtonsCardController>().IsChanged)
            {
                ChangedCards.Add(AllCards[i]);
                if (ChangedCards.Count > 0)
                {
                    _ButtonsSceneController.ButtonSetColor(_ButtonsSceneController.ButtonRightUp, ChangedCards[0].GetComponent<ButtonsCardController>().ButtonMaterial);
                    _ButtonsSceneController.NeededEffectRight = ChangedCards[0].GetComponent<ButtonsCardController>().Effect;
                    _ButtonsSceneController.NeededSoundRight = ChangedCards[0].GetComponent<ButtonsCardController>().Clip;
                }
                if (ChangedCards.Count > 1)
                {
                    _ButtonsSceneController.ButtonSetColor(_ButtonsSceneController.ButtonLeftUp, ChangedCards[1].GetComponent<ButtonsCardController>().ButtonMaterial);
                    _ButtonsSceneController.NeededEffectLeft = ChangedCards[1].GetComponent<ButtonsCardController>().Effect;
                    _ButtonsSceneController.NeededSoundLeft = ChangedCards[1].GetComponent<ButtonsCardController>().Clip;
                }
                AllCards.Remove(AllCards[i]);
                break;
            }
        }
        ChangedCardsNumber++;
    }

    public void ResetCards()
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            Cards[i].GetComponent<ButtonsCardController>().IsChanged = true;
            Cards[i].GetComponent<ButtonsCardController>().CardChanged();
        }
        
        AllCards = new List<GameObject>();
        for (int i = 0; i < Cards.Length; i++)
        {
            AllCards.Add(Cards[i]);
        }
        ChangedCards = new List<GameObject>();
        ChangedCardsNumber = 0;
    }


    public void RemoveCards()
    {
        for (int i = 0; i < ChangedCards.Count; i++)
        {
            if (ChangedCards[i].GetComponent<ButtonsCardController>().IsChanged == false)
            {
                AllCards.Add(ChangedCards[i]);
                ChangedCards.Remove(ChangedCards[i]);
                break;
            }
        }
        ChangedCardsNumber--;
    }
    //public void CheckCards()
    //{
    //    for (int i = 0; i < AllCards.Count; i++)
    //    {
    //        if (AllCards[i].GetComponent<ButtonsCardController>().IsChanged)
    //        {
    //            AllCards.Remove(AllCards[i]);
    //            ChangedCards.Add(AllCards[i]);
    //            break;
    //        }
    //        if (ChangedCards[i].GetComponent<ButtonsCardController>().IsChanged == false)
    //        {
    //            ChangedCards.Remove(AllCards[i]);
    //            AllCards.Add(AllCards[i]);
    //            break;
    //        }
    //    }
    //}
}
