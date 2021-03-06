﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalContentMover : MonoBehaviour
{
    #region Variables
    [SerializeField] private Button leftBtn, rightBtn;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private iTween.EaseType easyType;
    [SerializeField] private float visibleAreaWidth;
    [SerializeField] private float time = 1f;
    [SerializeField] private float minPos = -275f;
    [SerializeField] private float maxPos = -10f;
    [SerializeField] private float moveStep = 285f;
    [SerializeField] private float startPos = 0f;
    private float currentPos = 0f;
    private bool canMove = true;
    #endregion

    private void Awake()
    {
        MoveOnValue(startPos);
        currentPos = contentPanel.anchoredPosition.x;
        CalculateMinPos();

        if (leftBtn != null) leftBtn.onClick.AddListener(MoveLeft);
        if (rightBtn != null) rightBtn.onClick.AddListener(MoveRigth);
    }

    public void CalculateMinPos()
    {
        var contentSize = contentPanel.sizeDelta.x * contentPanel.localScale.x;
        minPos = visibleAreaWidth - contentSize + 10f;

        if (contentPanel.anchoredPosition.x >= maxPos || contentPanel.anchoredPosition.x <= minPos)
            currentPos = contentPanel.anchoredPosition.x;
    }

    #region Horizontal
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
                "onupdate", "MoveOnValue",
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
                "onupdate", "MoveOnValue",
                "easetype", easyType));
            yield return new WaitForSeconds(time);
            currentPos = targetPos;
            canMove = true;
        }
    }

    private void MoveOnValue(float value)
    {
        var pos = contentPanel.anchoredPosition;
        pos.x = value;
        contentPanel.anchoredPosition = pos;
    }
    #endregion
}
