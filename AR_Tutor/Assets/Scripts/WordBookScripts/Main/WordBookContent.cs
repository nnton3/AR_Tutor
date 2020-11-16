using UnityEngine;
using UnityEngine.UI;

public class WordBookContent : MonoBehaviour
{
    [SerializeField] private Image img1, img2, img3;
    [SerializeField] private Button clickBtn, audioBtn;
    [SerializeField] private Text title;
    [SerializeField] private RectTransform contentRect;
    private AudioClip clip, clip2;

    public void Initialize(CardData data)
    {
        img1.sprite = data.img1;
        img2.sprite = data.img2;
        img3.sprite = data.img3;
        clip = data.audioClip1;
        clip2 = data.audioClip2;
        title.text = data.Title;

        clickBtn.onClick.AddListener(PlayAudio);
        audioBtn.onClick.AddListener(PlayAnimalsAudio);
    }

    private void PlayAudio()
    {
        if (clip == null) return;
        Signals.PlayAudioClipEvent.Invoke(clip);
    }

    private void PlayAnimalsAudio()
    {
        if (clip2 == null) return;
        Signals.PlayAudioClipEvent.Invoke(clip2);
    }

    public void ShowNextImg()
    {
        if (contentRect.anchoredPosition.x <= -520f) return;

        var pos = contentRect.anchoredPosition;
        pos.x -= 290;
        contentRect.anchoredPosition = pos;
    }

    public void ShowPreviousImg()
    {
        if (contentRect.anchoredPosition.x >= -60) return;

        var pos = contentRect.anchoredPosition;
        pos.x += 290;
        contentRect.anchoredPosition = pos;
    }
}
