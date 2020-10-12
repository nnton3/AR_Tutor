using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        /*for (int i = 0; i < Cards.Length; i++)
        {
            
        } */
    }

    public void CreateSubcards()
    {
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
        }
    }

    public void CreatingCards(int num)
    {
        //Debug.Log(1);
        //if (SectionNumber == 0)
        //{
        //    TempCards = GameObject.FindGameObjectsWithTag("0");
        //    for (int i = 0; i < TempCards.Length; i++)
        //    {
        //        Instantiate(TempCards[i], StartPos.transform.position, Quaternion.identity);
        //    }
        //}
        /*
        SectionNumber = num;
        switch (SectionNumber)
        {
            case 0:
                TempCards = GameObject.FindGameObjectsWithTag("0");
                for (int i = 0; i < TempCards.Length; i++)
                {
                    Instantiate(TempCards[i], StartPos.transform.position, Quaternion.identity);
                }
                break;
            case 1:
                TempCards = GameObject.FindGameObjectsWithTag("1");
                for (int i = 0; i < TempCards.Length; i++)
                {
                    Instantiate(TempCards[i], StartPos.transform.position, Quaternion.identity);
                }
                break;
            case 2:
                TempCards = GameObject.FindGameObjectsWithTag("2");
                for (int i = 0; i < TempCards.Length; i++)
                {
                    Instantiate(TempCards[i], StartPos.transform.position, Quaternion.identity);
                }
                break;
        }
        */

        //GameObject GO = Instantiate(Cards, Pos2.transform.position, Quaternion.identity);
    }

}
