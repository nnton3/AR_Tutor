using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ContentMover : MonoBehaviour
{
    [SerializeField] private Button leftBtn, rightBtn;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private iTween.EaseType easyType;
    [SerializeField] private float visibleAreaWidth;
    [SerializeField] private float time = 1.5f;
    [SerializeField] private float minPos = -275f;
    [SerializeField] private float maxPos = -10f;
    [SerializeField] private float moveStep = 285f;
    [SerializeField] private float startPos = 0f;
    private float currentPos = 0f;
    private bool canMove = true;

    private void Awake()
    {
        MoveContentOnValue(startPos);
        currentPos = contentPanel.anchoredPosition.x;
        CalculateMinPos();

        leftBtn.onClick.AddListener(MoveLeft);
        rightBtn.onClick.AddListener(MoveRigth);
    }

    public void CalculateMinPos()
    {
        var contentPanelWidth = contentPanel.sizeDelta.x * contentPanel.localScale.x;
        minPos = visibleAreaWidth - contentPanelWidth + 10f;
    }

    private void MoveLeft()
    {
        if (!canMove) return;
        StartCoroutine(MoveLeftRoutine());
    }

    private void MoveRigth()
    {
        if (!canMove) return;
        StartCoroutine(MoveRightRoutine());
    }

    private IEnumerator MoveLeftRoutine()
    {
        if (contentPanel.anchoredPosition.x < maxPos)
        {
            canMove = false;
            var targetPos = currentPos + moveStep;
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", currentPos,
                "to", targetPos,
                "time", time,
                "onupdate", "MoveContentOnValue",
                "easetype", easyType));

            yield return new WaitForSeconds(time);
            currentPos = targetPos;
            canMove = true;
        }
    }

    private IEnumerator MoveRightRoutine()
    {
        if (contentPanel.anchoredPosition.x > minPos)
        {
            canMove = false;
            var targetPos = currentPos - moveStep;

            iTween.ValueTo(gameObject, iTween.Hash(
                "from", currentPos,
                "to", targetPos,
                "time", time,
                "onupdate", "MoveContentOnValue",
                "easetype", easyType));
            yield return new WaitForSeconds(time);
            currentPos = targetPos;
            canMove = true;
        }
    }

    private void MoveContentOnValue(float value)
    {
        var pos = contentPanel.anchoredPosition;
        pos.x = value;
        contentPanel.anchoredPosition = pos;
    }
}
