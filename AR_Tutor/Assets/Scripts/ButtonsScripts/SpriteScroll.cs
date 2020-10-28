using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteScroll : MonoBehaviour
{
    public Image Img;
    public Sprite[] Frames;
    public int CurrentFrame;
    public float Speed = 0.5f;

    private void OnEnable()
    {
        Img = gameObject.GetComponent<Image>();
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        Img.sprite = Frames[CurrentFrame];
        if (CurrentFrame >= Frames.Length - 1)
        {
            CurrentFrame = 0;
        }

        else
        {
            CurrentFrame++;
        }

        //if (CurrentFrame < Frames.Length - 1)
            
        yield return new WaitForSeconds(Speed);
        StartCoroutine(Play());
    }
}
