using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariantCardInitializer : CardBase
{
    private RectTransform rect;
    private bool isActive;
    [SerializeField] private Vector2 startPos;

    public void Initialize(CardData _data)
    {
        rect = GetComponent<RectTransform>();
        title.text = _data.Title;
        image.sprite = _data.img;
        selectBtn.onClick.AddListener(OnClickEventHandler);
        startPos = rect.localPosition;
    }

    private void OnClickEventHandler()
    {
        if (isActive) Desactive();
        else Active();
    }

    private void Active()
    {
        isActive = true;
        rect.localPosition = Vector2.zero;
        rect.localScale = Vector3.one * 2;
        transform.SetAsLastSibling();
    }

    private void Desactive()
    {
        isActive = false;
        rect.localPosition = startPos;
        rect.localScale = Vector3.one;
    }
}
