using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsContentController : MonoBehaviour
{

    public GameObject Content;
    public GameObject LeftBtn;
    public GameObject RightBtn;
    //public Transform StartPosContent;
    //public Transform FinalPosContent;
    public Transform TempPosContent;
    public float transitSpeed = 10f;
    public bool Left;
    public bool Right;

    private void Start()
    {
        LeftBtn.GetComponent<Button>().enabled = false;
    }
    public void GotoLeft()
    {
        TempPosContent.localPosition = new Vector2(transform.localPosition.x + 900f, transform.localPosition.y);
        Right = false;
        Left = true;
        LeftBtn.GetComponent<Button>().enabled = false;
        RightBtn.GetComponent<Button>().enabled = true;
    }
    public void GotoRight()
    {
        TempPosContent.localPosition = new Vector2(transform.localPosition.x - 900f, transform.localPosition.y);
        Left = false;
        Right = true;
        LeftBtn.GetComponent<Button>().enabled = true;
        RightBtn.GetComponent<Button>().enabled = false;
    }

    private void Update()
    {
        if (Left)
        {
            transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, TempPosContent.localPosition.x, Time.deltaTime * transitSpeed), transform.localPosition.y);
            Debug.Log(transform.localPosition);
            LeftBtn.GetComponent<Button>().enabled = false;
            StartCoroutine(Falsing());
        }
        if (Right)
        {
            transform.localPosition = new Vector2((Mathf.Lerp(transform.localPosition.x, TempPosContent.localPosition.x, Time.deltaTime * transitSpeed)), transform.localPosition.y);
            Debug.Log(transform.localPosition);
            RightBtn.GetComponent<Button>().enabled = false;
            StartCoroutine(Falsing());
        }

    }

    IEnumerator Falsing()
    {
        //LeftBtn.GetComponent<Button>().interactable = false;
        //RightBtn.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(0.3f);
        Left = false;
        Right = false;
        //LeftBtn.GetComponent<Button>().interactable = true;
        //RightBtn.GetComponent<Button>().interactable = true;
    }
}
