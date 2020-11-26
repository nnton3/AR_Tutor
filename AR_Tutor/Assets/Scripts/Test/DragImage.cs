using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public struct UserImgRect
{
    public Vector2 anchoredPosition;
    public Vector2 size;

    public UserImgRect(Vector2 _anchoredPosition, Vector2 _size)
    {
        anchoredPosition = _anchoredPosition;
        size = _size;
    }
}

public class DragImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform visibleArea;
    [SerializeField] private RectTransform imgRect;
    [SerializeField] private float visibleAreaW, visibleAreaH;
    [SerializeField] private float deltaX, deltaY;
    private Canvas canvas;

    private void Awake() { canvas = FindObjectOfType<Canvas>(); }

    private void Start()
    {
        CalculateVisibleArea();
        LoadImageRect();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveImageRect();
    }

    private void SaveImageRect()
    {
        var data = new UserImgRect(imgRect.anchoredPosition, imgRect.sizeDelta);
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("userImgRect", json);
    }

    private void LoadImageRect()
    {
        if (!PlayerPrefs.HasKey("userImgRect")) return;

        var json = PlayerPrefs.GetString("userImgRect");
        var data = JsonUtility.FromJson<UserImgRect>(json);
        imgRect.anchoredPosition = data.anchoredPosition;
        imgRect.sizeDelta = data.size;
    }

    private void CalculateVisibleArea()
    {
        var corners = new Vector3[4];
        visibleArea.GetLocalCorners(corners);

        visibleAreaW = corners[2].x - corners[1].x;
        visibleAreaH = corners[1].y - corners[0].y;

        deltaX = Math.Abs((visibleAreaW - imgRect.sizeDelta.x) / 2);
        deltaY = Math.Abs((visibleAreaH - imgRect.sizeDelta.y) / 2);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveImage(eventData.delta);
    }

    private void MoveImage(Vector2 _delta)
    {
        var pos = imgRect.anchoredPosition;
        pos += _delta * canvas.scaleFactor;
        pos.x = Mathf.Clamp(pos.x, -deltaX, deltaX);
        pos.y = Mathf.Clamp(pos.y, -deltaY, deltaY);
        imgRect.anchoredPosition = pos;
    }

    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }
}
