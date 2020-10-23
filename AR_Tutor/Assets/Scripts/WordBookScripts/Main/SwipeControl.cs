using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeControl : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    [SerializeField] private float deadZone = 0.5f;

    #region Unity swipe control
    public void OnBeginDrag(PointerEventData eventData)
    {
        firstPressPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        secondPressPos = eventData.position;
        currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
        currentSwipe.Normalize();

        CalculateResult();
    }

    public void OnPointerClick(PointerEventData eventData) { }
    #endregion

    private void CalculateResult()
    {
        if (currentSwipe.y > 0 & currentSwipe.x > -deadZone & currentSwipe.x < deadZone)
            Signals.UpSwipeEvent.Invoke();

        if (currentSwipe.y < 0 & currentSwipe.x > -deadZone & currentSwipe.x < deadZone)
            Signals.DownSwipeEvent.Invoke();

        if (currentSwipe.x < 0 & currentSwipe.y > -deadZone & currentSwipe.y < deadZone)
            Signals.LeftSwipeEvent.Invoke();

        if (currentSwipe.x > 0 & currentSwipe.y > -deadZone & currentSwipe.y < deadZone)
            Signals.RightSwipeEvent.Invoke();
    }
}
