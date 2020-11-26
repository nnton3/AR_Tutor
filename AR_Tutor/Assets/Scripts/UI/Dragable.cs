using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;
    private Canvas canvas;
    private GameObjectEvent began;
    private Vector3Event drag;
    private UnityEvent end;

    public void Initialize(GameObjectEvent beganDragEvent, UnityEvent endDragEvent, Vector3Event dragEvent)
    {
        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        began = beganDragEvent;
        drag = dragEvent;
        end = endDragEvent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (MainMenuUIControl.Mode == MenuMode.Play) return;
        if (!GetComponent<EditableElement>().Visible) return;
        began.Invoke(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (MainMenuUIControl.Mode == MenuMode.Play) return;
        if (!GetComponent<EditableElement>().Visible) return;

        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        drag.Invoke(rect.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (MainMenuUIControl.Mode == MenuMode.Play) return;
        if (!GetComponent<EditableElement>().Visible) return;
        end.Invoke();  
    }

    public void OnPointerDown(PointerEventData eventData) { }
}
