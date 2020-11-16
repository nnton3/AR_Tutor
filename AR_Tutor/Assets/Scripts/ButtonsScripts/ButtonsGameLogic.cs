using UnityEngine;
using UnityEngine.UI;

public class ButtonsGameLogic : MonoBehaviour
{
    [SerializeField] private Button leftBtn, rightBtn;
    [SerializeField] private Image leftBtnImg, rightBtnImg;
    private int maxRepetititions = 3;
    private int currentRepetitions = 0;

    public void FillPanel(ButtonsCard[] _cards)
    {
        SetColors(_cards);
        SetEffects(_cards);
    }

    private void SetColors(ButtonsCard[] _cards)
    {
        leftBtnImg.color = _cards[0].GetColor();
        rightBtnImg.color = _cards[1].GetColor();
    }

    private void SetEffects(ButtonsCard[] _cards)
    {
        leftBtn.onClick.AddListener(() =>
        {
            leftBtn.GetComponent<Animator>().SetTrigger("press");
            _cards[0].GetEffect().gameObject.SetActive(true);
        });
        rightBtn.onClick.AddListener(() =>
        {
            currentRepetitions++;
            rightBtn.GetComponent<Animator>().SetTrigger("press");
            _cards[1].GetEffect().gameObject.SetActive(true);
            if (currentRepetitions == maxRepetititions) leftBtn.gameObject.SetActive(true);
        });
    }

    public void SetRepetitionsNumber(int _number)
    {
        maxRepetititions = _number;
    }

    public void Reset()
    {
        ClearOldData();
        leftBtn.GetComponent<Animator>().SetTrigger("reset");
        rightBtn.GetComponent<Animator>().SetTrigger("reset");
    }

    public void ClearOldData()
    {
        leftBtn.onClick.RemoveAllListeners();
        rightBtn.onClick.RemoveAllListeners();
        currentRepetitions = 0;
        leftBtn.gameObject.SetActive(false);
    }
}
