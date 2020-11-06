using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsContentMover : MonoBehaviour
{
    [SerializeField] private GameObject[] Cards;
    [SerializeField] private Button leftBtn, rightBtn;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private iTween.EaseType easyType;
    [SerializeField] private float time = 1.5f;
    private bool canMove = true;

    private void Awake()
    {
        leftBtn.onClick.AddListener(MoveLeft);
        rightBtn.onClick.AddListener(MoveRigth);
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
        canMove = false;
        if (contentPanel.anchoredPosition.x < -10)
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", -285f,
                "to", 0f,
                "time", time,
                "onupdate", "MoveContentOnValue",
                "easetype", easyType));
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private IEnumerator MoveRightRoutine()
    {
        canMove = false;
        if (contentPanel.anchoredPosition.x > -275)
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 0f,
                "to", -285f,
                "time", time,
                "onupdate", "MoveContentOnValue",
                "easetype", easyType));
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    private void MoveContentOnValue(float value)
    {
        var pos = contentPanel.anchoredPosition;
        pos.x = value;
        contentPanel.anchoredPosition = pos;
    }
}
