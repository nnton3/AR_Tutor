using UnityEngine;
using UnityEngine.UI;

public class WordBookContent : MonoBehaviour
{
    [SerializeField] private Image img1, img2, img3;
    [SerializeField] private Button clickBtn;
    [SerializeField] private Text title;
    [SerializeField] private RectTransform contentRect;
    private AudioClip clip;

    public void Initialize(CardData data)
    {
        img1.sprite = data.img1;
        img2.sprite = data.img2;
        img3.sprite = data.img3;
        clip = data.audioClip1;
        title.text = data.Title;

        clickBtn.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        FindObjectOfType<AudioSource>().PlayOneShot(clip);
    }

    public void ShowNextImg()
    {
        if (contentRect.anchoredPosition.x <= -400f) return;

        var pos = contentRect.anchoredPosition;
        pos.x -= 230;
        contentRect.anchoredPosition = pos;
    }

    public void ShowPreviousImg()
    {
        if (contentRect.anchoredPosition.x >= -60) return;

        var pos = contentRect.anchoredPosition;
        pos.x += 230;
        contentRect.anchoredPosition = pos;
    }
}
