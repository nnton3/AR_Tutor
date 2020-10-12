using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideMenuContent : MonoBehaviour
{
    public SlideMenuSection[] Sections;
    private GridLayoutGroup Grid;
    private bool CanPress = true;
    private bool Up;

    private void Start()
    {
        Grid = GetComponent<GridLayoutGroup>();    
    }

    public void MoveUp()
    {
        if (CanPress)
        {
            Up = true;
            StartCoroutine(MoveUpIE());
        }
    }

    public void MoveDown()
    {
        if (CanPress)
        {
            Up = false;
            StartCoroutine(MoveUpIE());
        }
    }

    IEnumerator MoveUpIE()
    {
        CanPress = false;
        Grid.enabled = false;
        if (Up == false)
        {
            transform.GetChild(Sections.Length - 1).transform.localPosition = new Vector2(transform.GetChild(0).transform.localPosition.x, transform.GetChild(0).transform.localPosition.y + 250);
            transform.GetChild(Sections.Length - 2).transform.localPosition = new Vector2(transform.GetChild(1).transform.localPosition.x, transform.GetChild(0).transform.localPosition.y + 500);
            transform.GetChild(Sections.Length - 1).SetSiblingIndex(0);
            transform.GetChild(Sections.Length - 1).SetSiblingIndex(0);
        }

        for (int i = 0; i < Sections.Length; i++)
        {
            if (Up)
            {
                Sections[i].MoveUp();
            }
            else
            {
                Sections[i].MoveDown();
            }
        }
        yield return new WaitForSeconds(1.2f);
        if (Up)
        {
            transform.GetChild(0).SetSiblingIndex(Sections.Length - 1);
            transform.GetChild(0).SetSiblingIndex(Sections.Length - 1);
        }
        else
        {
            //transform.GetChild(Sections.Length - 1).SetSiblingIndex(0);
            //transform.GetChild(Sections.Length - 1).SetSiblingIndex(0);
        }
        Grid.enabled = true;
        CanPress = true;
        Up = false;
    }
}
