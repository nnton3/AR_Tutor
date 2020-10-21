using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordBook
{
    public class SectionChange : MonoBehaviour
    {
        public Transform Content;
        private void Start()
        {
            Content.transform.GetChild(0).GetComponent<CreateCards>().CreateSubcards(false);
            
        }
    }
}