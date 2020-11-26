using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElementsManagment : MonoBehaviour
{
    #region Variables
    [SerializeField] protected List<GameObject> elements = new List<GameObject>();
    [SerializeField] protected GameObject emptyElement;
    [SerializeField] protected Transform contentPanel, parent, visibleArea;
    [SerializeField] protected Vector2 scrollDetectionBound = new Vector2();
    [SerializeField] private HorizontalContentMover contentMover;
    [SerializeField] private bool manageCategory, manageCard;
    protected Vector2 contentBound = new Vector2();
    private string categoryKey;
    protected Transform addBtn;
    protected GameObject target = null;
    protected Coroutine coroutine = null;
    #region Manage cards events
    [HideInInspector] public GameObjectEvent BeganDragEvent = new GameObjectEvent();
    [HideInInspector] public UnityEvent EndDragEvent = new UnityEvent();
    [HideInInspector] public Vector3Event DragEvent = new Vector3Event();
    #endregion
    #endregion

    private void OnValidate()
    {
        if (manageCard) manageCategory = false;
        if (manageCategory) manageCard = false;
    }

    public void Initialize(string _categoryKey)
    {
        categoryKey = _categoryKey;
        Initialize();
    }

    public void Initialize()
    {
        FillElementsList();

        BeganDragEvent.AddListener(OnBeganDrag);
        DragEvent.AddListener(OnDrag);
        EndDragEvent.AddListener(OnEndDrag);
    }

    private void Start()
    {
        CalculateVisibleAreaCount();
    }

    protected virtual void CalculateVisibleAreaCount()
    {
        if (visibleArea == null)
        {
            Debug.Log(gameObject.name);
            return;
        }
        var corners = new Vector3[4];
        visibleArea.GetComponent<RectTransform>().GetWorldCorners(corners);

        contentBound = new Vector2(corners[0].x, corners[3].x);
    }

    private void FillElementsList()
    {
        foreach (Transform element in contentPanel)
        {
            if (element.GetComponent<AddCardBtnInitializer>()) addBtn = element;
            elements.Add(element.gameObject);
            element.GetComponent<Dragable>()?.Initialize(BeganDragEvent, EndDragEvent, DragEvent);
        }
    }

    private void OnBeganDrag(GameObject _target)
    {
        if (MainMenuUIControl.Mode == MenuMode.Play) return;
        target = _target;
        emptyElement.transform.SetSiblingIndex(target.transform.GetSiblingIndex());
        target.transform.parent = parent;
        emptyElement.SetActive(true);
    }

    protected virtual void OnDrag(Vector3 _position)
    {
        if (target == null) return;
        CheckScrollEvent(_position);
        if (_position.x > contentBound.x && _position.x < contentBound.y)
            CalculateNewPosition(_position);
    }

    protected void CalculateNewPosition(Vector3 _position)
    {
        var disY = Mathf.Infinity;
        var distance = Mathf.Infinity;
        int targetIndex = emptyElement.transform.GetSiblingIndex();
        foreach (var element in elements)
        {
            if (element == target) continue;
            if (element.GetComponent<AddCardBtnInitializer>()) continue;
            if (element.transform.GetSiblingIndex() > addBtn.GetSiblingIndex()) continue;

            var tempDis = Vector3.Distance(element.GetComponent<RectTransform>().position, _position);
            if (tempDis < distance)
                if (Mathf.Abs(element.GetComponent<RectTransform>().anchoredPosition.y - _position.y) < disY)
                {
                    distance = tempDis;
                    targetIndex = element.transform.GetSiblingIndex();
                }
        }

        if (targetIndex != emptyElement.transform.GetSiblingIndex())
            emptyElement.transform.SetSiblingIndex(targetIndex);
    }

    protected virtual void CheckScrollEvent(Vector3 _position)
    {
        if (_position.x <= contentBound.x ||
            _position.x >= contentBound.y)
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

    protected virtual IEnumerator ScrollAreaTimer()
    {
        yield return new WaitForSeconds(2f);
        coroutine = null;
        if (target.GetComponent<RectTransform>().anchoredPosition.x <= scrollDetectionBound.x)
            contentMover.MoveLeft();
        else if (target.GetComponent<RectTransform>().anchoredPosition.x >= scrollDetectionBound.y)
            contentMover.MoveRigth();
    }

    private void OnEndDrag()
    {
        if (target == null) return;
        target.transform.SetParent(contentPanel);
        target.transform.SetSiblingIndex(emptyElement.transform.GetSiblingIndex());
        emptyElement.SetActive(false);
        int index = target.transform.GetSiblingIndex();
        if (manageCategory)
            Signals.SetIndexForCategory.Invoke(target.GetComponent<CategoryInitializer>().CategoryKey, index);
        if (manageCard)
            Signals.SetIndexForCard.Invoke(categoryKey, target.GetComponent<CardBase>().Key, index);
    }
}
