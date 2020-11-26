using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class VariantElementsManagment : MonoBehaviour
{
    #region Variables
    [SerializeField] protected List<GameObject> elements = new List<GameObject>();
    [SerializeField] protected Transform visibleArea, parent;
    [SerializeField] protected Vector2 scrollDetectionBound = new Vector2();
    [SerializeField] private bool manageCategory, manageCard;
    [SerializeField] protected GameObject emptyElement;
    private HorizontalContentMover contentMover;
    private CardHierarchyManager hierarchyManager;
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
        hierarchyManager = GetComponent<CardHierarchyManager>();
        contentMover = GetComponent<HorizontalContentMover>();
        elements.Add(emptyElement);

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
        if (visibleArea == null) return;

        var corners = new Vector3[4];
        visibleArea.GetComponent<RectTransform>().GetWorldCorners(corners);

        contentBound = new Vector2(corners[0].x, corners[3].x);
    }

    public void AddElement(GameObject _elem)
    {
        if (_elem.GetComponent<AddCardBtnInitializer>()) addBtn = _elem.transform;
        elements.Add(_elem);
        _elem.GetComponent<Dragable>()?.Initialize(BeganDragEvent, EndDragEvent, DragEvent);
    }

    public void RemoveElement(GameObject _elem)
    {
        elements.Remove(_elem);
    }

    private void OnBeganDrag(GameObject _target)
    {
        if (MainMenuUIControl.Mode == MenuMode.Play) return;
        target = _target;
        emptyElement.transform.SetSiblingIndex(target.transform.GetSiblingIndex());
        emptyElement.transform.parent = target.transform.parent;
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
            if (element.transform.GetSiblingIndex() > addBtn.GetSiblingIndex())
                if (hierarchyManager.GetActivePage() == addBtn.parent) continue;

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
            if (!NextPageIsManagable(_position.x)) return;

            coroutine = StartCoroutine(ScrollAreaTimer());
        }
        else if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private bool NextPageIsManagable(float _pos)
    {
        if (_pos >= contentBound.y)
        {
            var nextPage = hierarchyManager.GetPage(hierarchyManager.ActivePage + 1);
            if (nextPage != null)
            {
                var firsChild = nextPage.GetChild(0);
                if (firsChild.GetComponent<AddCardBtnInitializer>()) return false;
                if (!firsChild.GetComponent<EditableElement>().Visible) return false;
            }
        }
        return true;
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
        target.transform.parent = emptyElement.transform.parent;
        target.transform.SetSiblingIndex(emptyElement.transform.GetSiblingIndex());
        emptyElement.SetActive(false);
        int index = target.transform.GetSiblingIndex() + hierarchyManager.ActivePage * 6;
        SetIndex(target, index);
    }

    public void SetIndex(GameObject _target, int index)
    {
        if (manageCategory)
            Signals.SetIndexForCategory.Invoke(_target.GetComponent<CategoryInitializer>().CategoryKey, index);
        if (manageCard)
            Signals.SetIndexForCard.Invoke(categoryKey, _target.GetComponent<CardBase>().Key, index);
    }

    public void SetEmptyElementParent(Transform _parent)
    {
        emptyElement.transform.parent = _parent;
        emptyElement.transform.SetSiblingIndex(1);
    }

    public Transform GetAddBtn() { return addBtn; }
}
