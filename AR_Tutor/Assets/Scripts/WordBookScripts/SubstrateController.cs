using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubstrateController : MonoBehaviour
{
    public GameObject Scroll;
    public GameObject Cross;
    public GameObject SubstratePanel;
    public GameObject Frame;
    public GameObject[] SubCards;
    public int TempSubCardNum;
    public int TempNum;
    public GameObject Content;
    public AudioSource SFX;

    public bool CanSwipe;
    public bool CanSwipe1;
    public Vector2 firstPressPos;
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;
    public Image CardSprite;
    //public Sprite[] ImageSprites;
    //public int TempSpriteNumber;
    //public GameObject Cont;
    //public Sprite[] ImageSprites;
    public int TempSprtNum;
    [Space(20)]
    public GameObject SubcardsContent;
    public List<Sprite> SubcardsSprites;
    public Image[] imgs;

    private void Start()
    {
        //CanSwipe = true;
        //SubCards = Content.GetComponent<CreateCards>().Subcards;
        SFX.Play();

    }

    public void GetSpritesToFrame()
    {
        //List<Sprite> SubcardsSprites = new List<Sprite>();

        
        //imgs = SubcardsContent.GetComponentsInChildren<Image>();
        /*
        for (int i = 0; i < imgs.Count; i++)
        {
            SubcardsSprites.Add(imgs[i].GetComponent<Image>().sprite);
        }*/
    }

    public void CloseBtn()
    {
        Frame.SetActive(false);
        Scroll.SetActive(true);

        for (int i = 0; i < SubCards.Length; i++)
        {
            if (SubCards[i] != null)
            {
                SubCards[i].gameObject.transform.SetParent(Content.transform);
                SubCards[i].gameObject.transform.localScale = Vector2.one;
            }
        }
        CanSwipe = false;
    }

    public void GetCards()
    {
        for (int i = 0; i < Content.transform.childCount; i++)
        {
            SubCards[i] = Content.transform.GetChild(i).gameObject;
        }
        

        for (int i = 0; i < SubCards.Length; i++)
        {
            SubCards[i].gameObject.transform.SetParent(SubstratePanel.transform);
            SubCards[i].gameObject.transform.localScale = Vector2.zero;
        }
        //CreateBigCard();
    }

    public void CreateBigCard(int num)
    {

        /*
        for (int i = 0; i < SubCards.Length; i++)
        {
            if (SubCards[i].activeInHierarchy)
            {
                TempNum = i;
            }
        }*/
        TempNum = num;
        Scroll.SetActive(false);
        Frame.SetActive(true);
        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
        CanSwipe = true;       
    }

    void Update()
    {

            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }
                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    //normalize the 2d vector
                    currentSwipe.Normalize();

                    //swipe upwards
                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                    TempNum += 1;
                    if (TempNum < SubCards.Length)
                    {
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }

                    if (TempNum >= SubCards.Length)
                    {
                        TempNum = 0;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }
                }
                    //swipe down
                    if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                    TempNum -= 1;
                    if (TempNum < 0)
                    {
                        TempNum = SubCards.Length - 1;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }
                    if (TempNum < SubCards.Length && TempNum >= 0)
                    {
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }
                }
                    //swipe left
                    if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                    if (TempSprtNum == 0)
                    {
                        TempSprtNum = SubCards[TempNum].GetComponent<CardController>().ImageSprites.Length;
                    }
                    if (TempSprtNum > 0 && TempSprtNum != 0)
                    {
                        TempSprtNum -= 1;
                    }
                    else
                    {
                        TempSprtNum = 0;
                    }

                    CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[TempSprtNum];
                }
                    //swipe right
                    if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                    if (TempSprtNum < SubCards[TempNum].GetComponent<CardController>().ImageSprites.Length - 1)
                    {
                        TempSprtNum += 1;
                    }
                    else
                    {
                        TempSprtNum = 0;
                    }

                    CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[TempSprtNum];
                }
                }
            }
        //CardSprite = GetComponent<CardController>().ImageSprites[TempSubCardNum];
        if (CanSwipe1)
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

                //swipe upwards
                if (currentSwipe.y > 0.01f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                    TempNum += 1;
                    if (TempNum < SubCards.Length)
                    {
                        //SubCards[TempNum - 1].SetActive(false);
                        //SubCards[TempNum].SetActive(true);
                        //SubCards[TempNum - 1].gameObject.transform.SetParent(Scroll.transform);
                        //SubCards[TempNum - 1].gameObject.transform.localScale = Vector2.one;
                        //SubCards[TempNum].gameObject.transform.SetParent(SubstratePanel.transform);
                        //SubCards[TempNum].gameObject.transform.localScale = Vector2.zero;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }

                    if(TempNum >= SubCards.Length)
                    {
                        TempNum = 0;
                        //SubCards[SubCards.Length - 1].SetActive(false);
                        //SubCards[TempNum].SetActive(true);
                        //SubCards[SubCards.Length - 1].gameObject.transform.SetParent(Scroll.transform);
                        //SubCards[SubCards.Length - 1].gameObject.transform.localScale = Vector2.one;
                        //SubCards[TempNum].gameObject.transform.SetParent(SubstratePanel.transform);
                        //SubCards[TempNum].gameObject.transform.localScale = Vector2.zero;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }

                }
                //swipe down
                if (currentSwipe.y < -0.01f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                    TempNum -= 1;
                    if (TempNum < 0)
                    {
                        TempNum = SubCards.Length - 1;
                        //SubCards[0].SetActive(false);
                        //SubCards[TempNum].SetActive(true);
                        //SubCards[0].gameObject.transform.SetParent(Scroll.transform);
                        //SubCards[0].gameObject.transform.localScale = Vector2.one;
                        //SubCards[TempNum].gameObject.transform.SetParent(SubstratePanel.transform);
                        //SubCards[TempNum].gameObject.transform.localScale = Vector2.zero;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }
                    if (TempNum < SubCards.Length && TempNum >= 0)
                    {
                        //SubCards[TempNum + 1].SetActive(false);
                        //SubCards[TempNum].SetActive(true);
                        //SubCards[TempNum + 1].gameObject.transform.SetParent(Scroll.transform);
                        //SubCards[TempNum + 1].gameObject.transform.localScale = Vector2.one;
                        //SubCards[TempNum].gameObject.transform.SetParent(SubstratePanel.transform);
                        //SubCards[TempNum].gameObject.transform.localScale = Vector2.zero;
                        CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[0];
                    }

                }
                if (currentSwipe.x < -0.01f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                    if (TempSprtNum == 0)
                    {
                        TempSprtNum = SubCards[TempNum].GetComponent<CardController>().ImageSprites.Length;
                    }
                    if (TempSprtNum > 0 && TempSprtNum != 0)
                    {
                        TempSprtNum -= 1;
                    }
                    else
                    {
                        TempSprtNum = 0;
                    }

                    CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[TempSprtNum];
                }

                //swipe right
                if (currentSwipe.x > 0.01f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                    if (TempSprtNum < SubCards[TempNum].GetComponent<CardController>().ImageSprites.Length - 1)
                    {
                        TempSprtNum += 1;
                    }
                    else
                    {
                        TempSprtNum = 0;
                    }

                    CardSprite.sprite = SubCards[TempNum].GetComponent<CardController>().ImageSprites[TempSprtNum];
                }
            }
        }
        }

    }


