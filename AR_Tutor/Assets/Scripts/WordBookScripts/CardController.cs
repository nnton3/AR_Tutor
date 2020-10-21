using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{
    //public GameObject Scroll;
    public GameObject SubstratePanel;
    public GameObject CancelBtn;
    public Button btn;
    public ScrollRect Scroll;
    public bool CanSwipe;
    public Vector2 firstPressPos;
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;
    public Sprite[] ImageSprites;
    //public AudioSource SFX;
    public AudioClip CardClip;
    public AudioClip AdditionSnd;
    public int TempSpriteNumber;
    public GameObject Cont;
    public GameObject Substr;
    public int PartNumber;
    private void Start()
    {
        btn = gameObject.GetComponent<Button>();
        //Scroll = FindObjectOfType<ScrollRect>();
        //SubstratePanel = GameObject.Find("SubstratePanel");
        btn.onClick.AddListener(ScaleUp);
        Substr = GameObject.Find("Substrate");
        SubstratePanel = GameObject.Find("SubstratePanel");
        //GetComponent<SubstrateController>().CardSprite.GetComponent<Button>().onClick.AddListener(PushToPlaySnd);

    }

    

    //public void PushToPlaySnd()
    //{
    //    FindObjectOfType<SubstrateController>().SFX.PlayOneShot(CardClip);
    //}

    public void ScaleUp()
    {

        //gameObject.transform.SetParent(SubstratePanel.transform);
        //gameObject.transform.localScale = Vector2.zero;
        SubstrateController Subs = FindObjectOfType<SubstrateController>();
        PartNumber = transform.GetSiblingIndex();
        Subs.GetCards();
        Subs.CreateBigCard(PartNumber);
        FindObjectOfType<SubstrateController>().SFX.PlayOneShot(CardClip);
        //GetComponent<SubstrateController>().CardSprite.GetComponent<Button>().onClick.AddListener(PushToPlaySnd);
    }

    public void ScaleDown()
    {

    }

    //Vector2 firstPressPos;
    //Vector2 secondPressPos;
    //Vector2 currentSwipe;

    //public void Swipe()
    //{
    //    if (Input.touches.Length > 0)
    //    {
    //        Touch t = Input.GetTouch(0);
    //        if (t.phase == TouchPhase.Began)
    //        {
    //            //save began touch 2d point
    //            firstPressPos = new Vector2(t.position.x, t.position.y);
    //        }
    //        if (t.phase == TouchPhase.Ended)
    //        {
    //            //save ended touch 2d point
    //            secondPressPos = new Vector2(t.position.x, t.position.y);

    //            //create vector from the two points
    //            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

    //            //normalize the 2d vector
    //            currentSwipe.Normalize();

    //            //swipe upwards
    //            if (currentSwipe.y > 0 ||  currentSwipe.x > -0.5f || currentSwipe.x < 0.5f)
    //         {
    //                Debug.Log("up swipe");
    //            }
    //            //swipe down
    //            if (currentSwipe.y < 0 || currentSwipe.x > -0.5f || currentSwipe.x < 0.5f)
    //         {
    //                Debug.Log("down swipe");
    //            }
    //            //swipe left
    //            if (currentSwipe.x < 0 || currentSwipe.y > -0.5f || currentSwipe.y < 0.5f)
    //         {
    //                Debug.Log("left swipe");
    //            }
    //            //swipe right
    //            if (currentSwipe.x > 0 || currentSwipe.y > -0.5f || currentSwipe.y < 0.5f)
    //         {
    //                Debug.Log("right swipe");
    //            }
    //        }
    //    }
    //}
    public void Cancel()
    {
        Scroll.gameObject.SetActive(true);
        //создать карточки раздела
       
        //SubstratePanel.transform.GetChild(0).gameObject.SetActive(false);
        CancelBtn.gameObject.SetActive(false);
        CanSwipe = false;
    }

     void Update()
    {
        /*if (CanSwipe)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                //Debug.Log(firstPressPos);
            }
            if (Input.GetMouseButtonUp(0))
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                //create vector from the two points
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                //Debug.Log(currentSwipe.x + "," + currentSwipe.y);
                //normalize the 2d vector
                currentSwipe.Normalize();

                ////swipe upwards
                //if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                //{
                //    Debug.Log("up swipe");
                //    //Cont.GetComponent<CreateCards>().Subcards
                //}
                ////swipe down
                //if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                //{
                //    Debug.Log("down swipe");
                //}
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                    if (TempSpriteNumber == 0)
                    {
                        TempSpriteNumber = ImageSprites.Length;
                    }
                    if (TempSpriteNumber > 0 && TempSpriteNumber != 0)
                    {
                        TempSpriteNumber -= 1;
                    }
                    else
                    {
                        TempSpriteNumber = 0;
                    }

                    gameObject.GetComponent<Image>().sprite = ImageSprites[TempSpriteNumber];
                }

                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe"); 
                        if (TempSpriteNumber < ImageSprites.Length-1)
                        {
                            TempSpriteNumber += 1;
                        }
                        else
                        {
                            TempSpriteNumber = 0;
                        }

                        gameObject.GetComponent<Image>().sprite = ImageSprites[TempSpriteNumber];
                    }
                }
            }*/
        }
    }
 