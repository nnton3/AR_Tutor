using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VerticalContentMover : MonoBehaviour
{
    #region Variables
    [SerializeField] private Button upBtn, downBtn;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private iTween.EaseType easyType;
    [SerializeField] private float visibleAreaHeight;
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
        currentPos = contentPanel.anchoredPosition.y;
        CalculateMinPos();

        if (downBtn != null) downBtn.onClick.AddListener(MoveDown);
        if (upBtn != null) upBtn.onClick.AddListener(MoveUp);
    }

    public void UpdateMoveStep(float _value)
    {
        moveStep = _value;
        visibleAreaHeight = _value;
        CalculateMinPos();
    }

    public void CalculateMinPos()
    {
        var contentSize = contentPanel.sizeDelta.y * contentPanel.localScale.y;
        minPos = contentSize - visibleAreaHeight - 10f;
    }

    #region Vertical
    public void MoveUp()
    {
        if (!canMove) return;
        StartCoroutine(MoveUpRoutine());
    }

    public void MoveDown()
    {
        if (!canMove) return;
        StartCoroutine(MoveDownRoutine());
    }

    private IEnumerator MoveUpRoutine()
    {
        if (contentPanel.anchoredPosition.y < minPos)
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

    private IEnumerator MoveDownRoutine()
    {
        if (contentPanel.anchoredPosition.y > maxPos)
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
        pos.y = value;
        contentPanel.anchoredPosition = pos;
    }
    #endregion
}
