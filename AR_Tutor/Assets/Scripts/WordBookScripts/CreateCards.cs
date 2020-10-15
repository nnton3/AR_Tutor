using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateCards : MonoBehaviour
{
    //public GameObject[] Sections;
    //public GameObject[] Cards;
    //public GameObject[] TempCards;
    //public Transform StartPos;
    //public int SectionNumber = 2;

    public Transform Content;
    public GameObject[] Subcards;

    void Start()
    {

    }

    public void CreateSubcards()
    {
        FindObjectOfType<SubstrateController>().CloseBtn();
        foreach (Transform child in Content)
        {
            GameObject.Destroy(child.gameObject);
        }


        for (int i = 0; i < Subcards.Length; i++)
        {
            GameObject GO = Instantiate(Subcards[i]);
            GO.transform.SetParent(Content);
            GO.transform.localPosition = Vector2.zero;
            GO.transform.localScale = Vector2.one;
            GO.GetComponent<Image>().sprite = GO.GetComponent<CardController>().ImageSprites[0];
        }
    }

    public void CreatingCards()
    {

    }

}
