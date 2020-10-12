using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordBook
{
    public class SectionChange : MonoBehaviour
    {
        public GameObject[] Section;
        public GameObject Content;
        public GameObject FirstSectionObj;
        public GameObject Content1;
        public GameObject Content2;
        public Transform ContentPos;
        public Transform StartPos;
        public Vector2[] TempPos;
        public int TempNumber;
        public float ChangeArg;
        public bool ChangeSection;
        public float BoardSpeed;
        public bool ArrayIsParity;
        public Transform FirstSection;

        public Transform Pos0;
        public Transform Pos1;
        public Transform Pos2;
        public Transform Pos3;
        public Transform Pos4;
        public Transform Pos5;


        public void Start()
        {
            Content1.transform.localPosition = new Vector2(0, 0);
            //Content2.transform.localPosition = new Vector2(0, -500);

            GameObject GO = Instantiate(Section[TempNumber], Pos2.transform.position, Quaternion.identity);
            GO.transform.SetParent(Content1.transform);
            GO.transform.localPosition = Pos2.transform.localPosition;
            TempNumber++;
            GameObject GO1 = Instantiate(Section[TempNumber], Pos3.transform.position, Quaternion.identity);
            GO1.transform.SetParent(Content1.transform);
            GO1.transform.localPosition = Pos3.transform.localPosition;
            TempNumber++;

        }

        //public void AddSection()
        //{
        //    for (int i = 0; i < Section.Length; i++)
        //    {
        //        Instantiate(Section[i+1], new Vector2(Section[i].transform.position.x, (Section[i].transform.position.y + ChangeArg))
        //    }
        //}

        public void CheckParity()
        {
            
            if (Section.Length % 2 == 0)
            {
                ArrayIsParity = true;
            }
        }

        public void ScrollContentUp()
        {
            if (TempNumber < Section.Length)
            {
                Content2.transform.localPosition = new Vector2(0, -500);
                if (TempNumber == Section.Length - 1)
                {
                    GameObject GO = Instantiate(Section[TempNumber], Pos4.transform.localPosition, Quaternion.identity);
                    GO.transform.SetParent(Content2.transform);
                    GO.transform.localPosition = Pos4.transform.localPosition;
                    TempNumber = 0;
                    GameObject GO1 = Instantiate(Section[TempNumber], Pos5.transform.localPosition, Quaternion.identity);
                    GO1.transform.SetParent(Content2.transform);
                    GO1.transform.localPosition = Pos5.transform.localPosition;
                    TempNumber++;
                }
                else
                {
                    GameObject GO = Instantiate(Section[TempNumber], Pos4.transform.localPosition, Quaternion.identity);
                    GO.transform.SetParent(Content2.transform);
                    GO.transform.localPosition = Pos4.transform.localPosition;
                    TempNumber++;
                    if (TempNumber == Section.Length - 1)
                    {
                        GameObject GO1 = Instantiate(Section[TempNumber], Pos5.transform.localPosition, Quaternion.identity);
                        GO1.transform.SetParent(Content2.transform);
                        GO1.transform.localPosition = Pos5.transform.localPosition;
                        TempNumber = 0;
                    }
                    else
                    {
                        GameObject GO1 = Instantiate(Section[TempNumber], Pos5.transform.localPosition, Quaternion.identity);
                        GO1.transform.SetParent(Content2.transform);
                        GO1.transform.localPosition = Pos5.transform.localPosition;
                        TempNumber++;
                    }
                        
                }
                ChangeSection = true;
            }

        }
        //Content.localPosition = Vector2.Lerp(Content.localPosition, StartBoardPos, 2f * BoardSpeed* Time.deltaTime);
        public void Update()
        {
            if (ChangeSection)
            {
                    Content1.transform.localPosition = Vector2.Lerp(Content1.transform.localPosition, new Vector2(0,500), 2f * BoardSpeed * Time.deltaTime);
                    //Section[TempNumber - 3].transform.localPosition = Vector2.Lerp(Section[TempNumber - 3].transform.localPosition, Pos1.localPosition, 2f * BoardSpeed * Time.deltaTime);
                    Content2.transform.localPosition = Vector2.Lerp(Content2.transform.localPosition, new Vector2(0, 0), 2f * BoardSpeed * Time.deltaTime);
                //Section[TempNumber - 1].transform.localPosition = Vector2.Lerp(Section[TempNumber - 1].transform.localPosition, Pos3.localPosition, 2f * BoardSpeed * Time.deltaTime);
                StartCoroutine(DestroyTempObjects());
                
            }
        }

        IEnumerator DestroyTempObjects()
        {
            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < Content1.transform.childCount; i++)
            {
                if (Content1.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    Destroy(Content1.transform.GetChild(i).gameObject);
                }
            }

            Content1.transform.localPosition = Content2.transform.localPosition;

            for (int i = 0; i < Content2.transform.childCount; i++)
            {
                if (Content1.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    Content2.transform.GetChild(i).SetParent(Content1.transform);
                }
            }
            
        }
    }
}





            //Debug.Log(Section.Length);
            ////TempPos[TempNumber] = new Vector2(Section[TempNumber].transform.localPosition.x, Section[TempNumber].transform.localPosition.y + 2 * ChangeArg);
            ////for (int i = TempNumber; i < Section.Length; i++)
            ////{
            //if (ArrayIsParity)
            //{
            //    if (TempNumber < Section.Length)
            //    {
            //        GameObject GO = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * 2), Quaternion.identity);
            //        GO.transform.SetParent(Content.transform);
            //        GO.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //        TempNumber++;
            //        GameObject GO1 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * TempNumber), Quaternion.identity);
            //        GO1.transform.SetParent(Content.transform);
            //        GO1.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //        TempNumber++;
            //        ChangeSection = true;
            //    }
            //}
            //else
            //{
            //    if (TempNumber < Section.Length)
            //    {
            //        if(TempNumber == Section.Length - 1)
            //        {
            //            GameObject GO0 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * 2), Quaternion.identity);
            //            GO0.transform.SetParent(Content.transform);
            //            GO0.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //            TempNumber = 0;
            //            FirstSection = FirstSectionObj.transform;

            //            GameObject GO01 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * TempNumber), Quaternion.identity);
            //            GO01.transform.SetParent(Content.transform);
            //            GO01.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //            TempNumber++;
            //            ChangeSection = true;
            //        }
            //        else
            //        {
            //            GameObject GO = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * 2), Quaternion.identity);
            //            GO.transform.SetParent(Content.transform);
            //            GO.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //            TempNumber++;
            //            GameObject GO1 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * TempNumber), Quaternion.identity);
            //            GO1.transform.SetParent(Content.transform);
            //            GO1.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //            TempNumber++;
            //            ChangeSection = true;
            //        }
            //        if (TempNumber == Section.Length)
            //        {
            //            FirstSection = FirstSectionObj.transform;
            //            TempNumber = 0;
            //            GameObject GO0 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * 2), Quaternion.identity);
            //            GO0.transform.SetParent(Content.transform);
            //            GO0.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);

            //            GameObject GO01 = Instantiate(Section[TempNumber], new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y + ChangeArg * TempNumber), Quaternion.identity);
            //            GO01.transform.SetParent(Content.transform);
            //            GO01.transform.localPosition = new Vector2(FirstSection.localPosition.x, FirstSection.localPosition.y - ChangeArg * TempNumber);
            //            TempNumber++;
            //            ChangeSection = true;
            //        }
            //        }
            //}
            
            ////if(TempNumber >= Section.Length) отключение прокрутки при окончании массива
            ////{
            ////    ChangeSection = false;
            ////}
              