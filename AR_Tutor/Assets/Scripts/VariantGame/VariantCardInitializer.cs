using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantCardInitializer : CardBase
{
    private RectTransform rect;

    public override void Initialize(GameName _game, string _categoryKey, string cardKey, CardData _data)
    {
        Key = cardKey;

        rect = GetComponent<RectTransform>();
        title.text = _data.Title;
        image.sprite = _data.img1;
        selectBtn.onClick.AddListener(Active);
    }

    private void Active()
    {
        Signals.VariantGameCardSelect.Invoke(Key);
        rect.localPosition = Vector2.zero;
        rect.localScale = Vector3.one * 2;
    }
}
