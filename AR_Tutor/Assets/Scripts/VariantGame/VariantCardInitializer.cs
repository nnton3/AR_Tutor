using UnityEngine;

public class VariantCardInitializer : CardBase
{
    private RectTransform rect;
    private AudioClip clip;

    public override void Initialize(GameName _game, string _categoryKey, string cardKey, CardData _data)
    {
        Key = cardKey;

        rect = GetComponent<RectTransform>();
        image.sprite = _data.img1;
        selectBtn.onClick.AddListener(Active);
        if (_data.audioClip1 != null) clip = _data.audioClip1;
    }

    private void Active()
    {
        Signals.VariantGameCardSelect.Invoke(Key);
        rect.localPosition = Vector2.zero;
        rect.localScale = Vector3.one * 2;
        if (clip != null) Signals.PlayAudioClipEvent.Invoke(clip);
    }
}
