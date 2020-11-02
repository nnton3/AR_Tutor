using UnityEngine;
using UnityEngine.UI;

public class ButtonsMenuControl : MonoBehaviour
{
    [SerializeField] private Sprite defaultImg, selectedImg;
    [SerializeField] private Button oneRepetitionBtn, twoRepetitionBtn, threeRepetitionBtn;
    private ButtonsGameLogic gameLogic;

    private void Awake()
    {
        gameLogic = FindObjectOfType<ButtonsGameLogic>();
        LoadCurrentMode();

        oneRepetitionBtn.onClick.AddListener(() =>
        {
            SaveMode(1);
            ConfigurateModeSelector(1);
        });
        twoRepetitionBtn.onClick.AddListener(() =>
        {
            SaveMode(2);
            ConfigurateModeSelector(2);
        });
        threeRepetitionBtn.onClick.AddListener(() => 
        {
            SaveMode(3);
            ConfigurateModeSelector(3);
        });
    }

    private void SaveMode(int mode)
    {
        PlayerPrefs.SetInt("ButtonsMode", mode);
    }

    private void LoadCurrentMode()
    {
        if (PlayerPrefs.HasKey("ButtonsMode"))
        {
            int mode = PlayerPrefs.GetInt("ButtonsMode");
            gameLogic.SetRepetitionsNumber(mode);

            ConfigurateModeSelector(mode);
        }
        else
            ConfigurateModeSelector(3);
    }

    private void ConfigurateModeSelector(int mode)
    {
        switch (mode)
        {
            case 1:
                oneRepetitionBtn.GetComponent<Image>().sprite = selectedImg;
                twoRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
                threeRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
                break;
            case 2:
                oneRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
                twoRepetitionBtn.GetComponent<Image>().sprite = selectedImg;
                threeRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
                break;
            case 3:
                DefaulConfig();
                break;
            default:
                DefaulConfig();
                break;
        }

        void DefaulConfig()
        {
            oneRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
            twoRepetitionBtn.GetComponent<Image>().sprite = defaultImg;
            threeRepetitionBtn.GetComponent<Image>().sprite = selectedImg;
        }
    }

    #region Legacy
    //#region Variables
    //public GameObject AnimLeftBtn;
    //public GameObject AnimRightBtn;
    //public GameObject BackBtn;
    //public GameObject Substrate;
    //public AudioSource source;
    //public AudioClip[] CardClip;
    //public Animation ButtonAnim;
    //public Animation[] CardAnim;
    //[Header("Colors")]
    //public Renderer ButtonLeftUp;
    //public Renderer ButtonRightUp;
    //[Header("Big buttons")]
    //public int LeftBigButtonPressTimes;
    //public int RightBigButtonPressTimes;
    //public float LeftAnimLength;
    //public float RightAnimLength;
    //public GameObject NeededEffectLeft;
    //public GameObject NeededEffectRight;
    //public Animator LeftBigButtonAnim;
    //public Animator RightBigButtonAnim;
    //public AudioClip NeededSoundLeft;
    //public AudioClip NeededSoundRight;
    //[Header("Big buttons scaling")]
    //public bool CanScalePerRound = true;
    //public float BigButtonScaleSpeed = 5;
    //public bool LeftBigButtonScaleIn;
    //public bool RightBigButtonScaleIn;
    //public Transform LeftBigButtonTr;
    //public Transform RightBigButtonTr;
    //#endregion

    //private void Awake()
    //{
    //    source = FindObjectOfType<AudioSource>();
    //}

    //public void ButtonSetColor(Renderer rend, Material mat)
    //{
    //    rend.material = mat;
    //}

    //public void ActivateScene()
    //{
    //    BackBtn.SetActive(true);
    //    AnimRightBtn.gameObject.SetActive(true);
    //    RightBigButtonAnim.Play("Reset");
    //}
    //public void BackToCards()
    //{
    //    BackBtn.SetActive(false);
    //    AnimRightBtn.gameObject.SetActive(false);
    //    AnimLeftBtn.gameObject.SetActive(false);
    //    Substrate.gameObject.SetActive(true);
    //    //FindObjectOfType<ButtonsSelector>().ResetCards();
    //    source.Stop();
    //}

    //public void PlayFirstEffect()
    //{
    //    ButtonAnim.Play();
    //}

    //public void LeftBigButton()
    //{
    //    NeededEffectRight.SetActive(false);
    //    LeftBigButtonPressTimes++;
    //    NeededEffectLeft.SetActive(false);
    //    NeededEffectLeft.SetActive(true);
    //    source.PlayOneShot(NeededSoundLeft);
    //    LeftBigButtonAnim.Play("Press");
    //    LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = false;
    //    StartCoroutine(UnPressLeftByAnimTime());
    //}

    //public void RightBigButton()
    //{
    //    NeededEffectLeft.SetActive(false);
    //    RightBigButtonPressTimes++;
    //    NeededEffectRight.SetActive(false);
    //    NeededEffectRight.SetActive(true);
    //    source.PlayOneShot(NeededSoundRight);
    //    RightBigButtonAnim.Play("Press");
    //    RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = false;
    //    StartCoroutine(UnPressRightByAnimTime());
    //}

    //IEnumerator UnPressLeftByAnimTime()
    //{
    //    LeftAnimLength = NeededEffectLeft.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length / 2;
    //    yield return new WaitForSeconds(LeftAnimLength);
    //    LeftBigButtonAnim.Play("UnPress");
    //    yield return new WaitForSeconds(1);
    //    LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;

    //    if (LeftBigButtonPressTimes >= 3 && CanScalePerRound)
    //    {
    //        //right scale in
    //        RightBigButtonScaleIn = true;
    //        CanScalePerRound = false;
    //    }
    //}

    //IEnumerator UnPressRightByAnimTime()
    //{
    //    RightAnimLength = NeededEffectRight.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length / 2;
    //    yield return new WaitForSeconds(RightAnimLength);
    //    RightBigButtonAnim.Play("UnPress");
    //    yield return new WaitForSeconds(1);
    //    RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;

    //    if (RightBigButtonPressTimes >= 3 && CanScalePerRound)
    //    {
    //        NeededEffectRight.SetActive(false);
    //        //left scale in
    //        LeftBigButtonTr.gameObject.SetActive(true);
    //        LeftBigButtonScaleIn = true;
    //        LeftBigButtonAnim.Play("Reset");
    //        RightBigButtonScaleIn = false;
    //        //right scale out
    //    }
    //}

    //public void RestartRound()
    //{
    //    CanScalePerRound = true;
    //    LeftBigButtonAnim.Play("Reset");
    //    RightBigButtonAnim.Play("Reset");
    //    LeftBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;
    //    RightBigButtonAnim.GetComponent<MousePressEvents>().CanPress = true;
    //    LeftBigButtonPressTimes = 0;
    //    RightBigButtonPressTimes = 0;
    //    NeededEffectLeft.SetActive(false);
    //    NeededEffectRight.SetActive(false);
    //    LeftBigButtonTr.gameObject.SetActive(false);
    //    LeftBigButtonScaleIn = false;
    //    RightBigButtonScaleIn = true;
    //}
    #endregion
}
