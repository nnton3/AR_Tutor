using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSceneController : MonoBehaviour
{
    public GameObject AnimLeftBtn;
    public GameObject AnimRightBtn;
    public GameObject BackBtn;
    public GameObject Substrate;
    public AudioSource SFX;
    public AudioClip[] CardClip;
    public Animation ButtonAnim;
    public Animation[] CardAnim;
    [Header("Colors")]
    public Renderer ButtonLeftUp;
    public Renderer ButtonRightUp;
    [Header("Big buttons")]
    public int LeftBigButtonPressTimes;
    public int RightBigButtonPressTimes;
    public float LeftAnimLength;
    public float RightAnimLength;
    public GameObject NeededEffectLeft;
    public GameObject NeededEffectRight;
    public Animator LeftBigButtonAnim;
    public Animator RightBigButtonAnim;
    public AudioClip NeededSoundLeft;
    public AudioClip NeededSoundRight;
    [Header("Big buttons scaling")]
    public bool CanScalePerRound = true;
    public float BigButtonScaleSpeed = 5;
    public bool LeftBigButtonScaleIn;
    public bool RightBigButtonScaleIn;
    public Transform LeftBigButtonTr;
    public Transform RightBigButtonTr;


    private void Start()
    {
        SFX.Play();
    }
    private void Update()
    {
        if (LeftBigButtonScaleIn)
        {
            LeftBigButtonTr.localScale = Vector3.Lerp(LeftBigButtonTr.localScale, new Vector3(50, 50, 50), BigButtonScaleSpeed * Time.deltaTime);
        }
        else
        {
            LeftBigButtonTr.localScale = Vector3.Lerp(LeftBigButtonTr.localScale, Vector3.zero, BigButtonScaleSpeed * Time.deltaTime);
        }

        if (RightBigButtonScaleIn)
        {
            RightBigButtonTr.localScale = Vector3.Lerp(RightBigButtonTr.localScale, new Vector3(50, 50, 50), BigButtonScaleSpeed * Time.deltaTime);
        }
        else
        {
            RightBigButtonTr.localScale = Vector3.Lerp(RightBigButtonTr.localScale, Vector3.zero, BigButtonScaleSpeed * Time.deltaTime);
        }
    }

    public void ButtonSetColor(Renderer rend, /*Color col,*/ Material mat)
    {
        //rend.material.color = col;
        rend.material = mat;
    }

    public void ActivateScene()
    {
        BackBtn.SetActive(true);
        //AnimRightBtn.gameObject.SetActive(true);
        AnimRightBtn.gameObject.SetActive(true);
        RightBigButtonAnim.Play("Reset");
    }
    public void BackToCards()
    {
        //LeftBigButtonAnim.CrossFadeInFixedTime(;
        BackBtn.SetActive(false);
        AnimRightBtn.gameObject.SetActive(false);
        AnimLeftBtn.gameObject.SetActive(false);
        Substrate.gameObject.SetActive(true);
        FindObjectOfType<ButtonsSubstrateController>().ResetCards();
        SFX.Stop();
    }

    public void PlayFirstEffect()
    {
        ButtonAnim.Play();
    }

    public void LeftBigButton()
    {
        NeededEffectRight.SetActive(false);
        LeftBigButtonPressTimes++;
        NeededEffectLeft.SetActive(false);
        NeededEffectLeft.SetActive(true);
        SFX.PlayOneShot(NeededSoundLeft);
        //NeededEffectLeft.GetComponent<Animator>().Play("Effect", -1, 0);
        LeftBigButtonAnim.Play("Press");
        LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = false;
        StartCoroutine(UnPressLeftByAnimTime());
    }

    public void RightBigButton()
    {
        NeededEffectLeft.SetActive(false);
        RightBigButtonPressTimes++;
        NeededEffectRight.SetActive(false);
        NeededEffectRight.SetActive(true);
        SFX.PlayOneShot(NeededSoundRight);
        //NeededEffectRight.GetComponent<Animator>().Play("Effect", -1, 0);
        RightBigButtonAnim.Play("Press");
        RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = false;
        StartCoroutine(UnPressRightByAnimTime());
    }

    IEnumerator UnPressLeftByAnimTime()
    {
        LeftAnimLength = NeededEffectLeft.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length / 2;
        yield return new WaitForSeconds(LeftAnimLength);
        LeftBigButtonAnim.Play("UnPress");
        yield return new WaitForSeconds(1);
        LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;

        if (LeftBigButtonPressTimes >= 3 && CanScalePerRound)
        {
            //right scale in
            RightBigButtonScaleIn = true;
            CanScalePerRound = false;
        }
    }

    IEnumerator UnPressRightByAnimTime()
    {
        RightAnimLength = NeededEffectRight.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length / 2;
        yield return new WaitForSeconds(RightAnimLength);
        RightBigButtonAnim.Play("UnPress");
        yield return new WaitForSeconds(1);
        RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;

        if (RightBigButtonPressTimes >= 3 && CanScalePerRound)
        {
            NeededEffectRight.SetActive(false);
            //left scale in
            LeftBigButtonTr.gameObject.SetActive(true);
            LeftBigButtonScaleIn = true;
            LeftBigButtonAnim.Play("Reset");
            RightBigButtonScaleIn = false;
            //right scale out
        }
    }

    public void RestartRound()
    {
        CanScalePerRound = true;
        LeftBigButtonAnim.Play("Reset");
        RightBigButtonAnim.Play("Reset");
        LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;
        RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;
        LeftBigButtonPressTimes = 0;
        RightBigButtonPressTimes = 0;
        NeededEffectLeft.SetActive(false);
        NeededEffectRight.SetActive(false);
        LeftBigButtonTr.gameObject.SetActive(false);
        LeftBigButtonScaleIn = false;
        RightBigButtonScaleIn = true;
    }
}
