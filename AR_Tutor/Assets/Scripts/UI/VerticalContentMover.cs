using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VerticalContentMover : MonoBehaviour
{
    #region Variables
    [SerializeField] private Button downBtn, upBtn;
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
        currentPos = contentPanel.anchoredPosition.y;
        CalculateMinPos();

        if (upBtn != null) upBtn.onClick.AddListener(MoveUp);
        if (downBtn != null) downBtn.onClick.AddListener(MoveDown);
    }

    public void CalculateMinPos()
    {
        var contentSize = contentPanel.sizeDelta.y * contentPanel.localScale.y;
        minPos = contentSize - visibleAreaWidth - 10f;

        if (contentPanel.anchoredPosition.y >= minPos || contentPanel.anchoredPosition.y <= maxPos)
            currentPos = contentPanel.anchoredPosition.y;
    }

    #region Vertical
    private void MoveUp()
    {
        if (!canMove) return;
        StartCoroutine(MoveUpRoutine());
    }

    private void MoveDown()
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
