using System.Collections;
using UnityEngine;

public class ElementsManagmentWordbook : ElementsManagment
{
    [SerializeField] private VerticalContentMover verticalContentMover;

    protected override void CalculateVisibleAreaCount()
    {
        var corners = new Vector3[4];
        visibleArea.GetComponent<RectTransform>().GetWorldCorners(corners);

        contentBound = new Vector2(corners[0].y, corners[1].y);
        Debug.Log(contentBound);
    }

    protected override void OnDrag(Vector3 _position)
    {
        if (target == null) return;
        CheckScrollEvent(_position);
        if (_position.y > contentBound.x && _position.y < contentBound.y)
            CalculateNewPosition(_position);
    }

    protected override void CheckScrollEvent(Vector3 _position)
    {
        if (_position.y <= contentBound.x ||
            _position.y >= contentBound.y)
        {
            if (coroutine != null) return;
            coroutine = StartCoroutine(ScrollAreaTimer());
        }
        else if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    protected override IEnumerator ScrollAreaTimer()
    {
        Debug.Log("start timer");
        yield return new WaitForSeconds(2f);
        coroutine = null;
        if (target.GetComponent<RectTransform>().anchoredPosition.x <= contentBound.y)
            verticalContentMover.MoveUp();
        else if (target.GetComponent<RectTransform>().anchoredPosition.x >= contentBound.y)
            verticalContentMover.MoveDown();
    }
}
