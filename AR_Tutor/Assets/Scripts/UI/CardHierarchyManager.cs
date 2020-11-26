using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHierarchyManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GridLayoutGroup contentGrid;
    [SerializeField] private RectTransform visibleAreaRect;
    [SerializeField] private Transform pagesParent;
    [SerializeField] private GameObject pagePref;
    [SerializeField] private float pageWidth = 410f;
    [SerializeField] private int elementOnPage = 6;
    private HorizontalContentMover contentMover;
    private VariantGridCalculater gridCalculater;
    private VariantElementsManagment elementsManagment;
    private CardsClusterControl clustersControl;
    private float pageHeight = 0f;
    [SerializeField] private List<GridLayoutGroup> grids = new List<GridLayoutGroup>();
    public int ActivePage { get; private set; } = 0;
    private bool needUpdate;
    #endregion

    public void Initialize()
    {
        gridCalculater = GetComponent<VariantGridCalculater>();
        contentMover = GetComponent<HorizontalContentMover>();
        elementsManagment = GetComponent<VariantElementsManagment>();
        clustersControl = GetComponent<CardsClusterControl>();

        contentMover.EndMoveEvent.AddListener(() =>
        {
            ActivePage = GetActivePageIndex();
            elementsManagment.SetEmptyElementParent(grids[ActivePage].transform);
            needUpdate = true;
        });
    }

    private void Start()
    {
        CalculatePageHeight();
        contentGrid.cellSize = new Vector2(pageWidth, pageHeight);
        gridCalculater.CalculateGrid(new Vector2(pageWidth, pageHeight));

        var length = grids.Count;
        for (int i = 0; i < length; i++)
            SetGridValuesForPages(grids[i]);

        contentMover.CalculateMinPos();
    }

    public Transform GetActivePage() { return grids[ActivePage].transform; }

    private void CalculateContentSize()
    {
        var size = pagesParent.GetComponent<RectTransform>().sizeDelta;
        size.x = grids.Count * pageWidth;

        pagesParent.GetComponent<RectTransform>().sizeDelta = size;
    }

    private void CalculatePageHeight()
    {
        var corners = new Vector3[4];
        visibleAreaRect.GetLocalCorners(corners);

        pageHeight = corners[1].y - corners[0].y;
        contentGrid.cellSize = new Vector2(pageWidth, pageHeight);
        contentGrid.padding = new RectOffset(0, 0, 0, 0);
        contentGrid.spacing = new Vector2(0f, 0f);
    }

    public void AddCardToBeginning(Transform _target)
    {
        if (grids.Count == 0) AddNewPage();
        if (UIInstruments.GetVisibleElements(grids[grids.Count - 1].transform) >= 6) AddNewPage();
        _target.parent = grids[0].transform;
        _target.localScale = Vector3.one;
        _target.SetAsFirstSibling();
        elementsManagment.SetIndex(_target.gameObject, 0);
        elementsManagment.AddElement(_target.gameObject);
        needUpdate = true;
        contentMover.CalculateMinPos();
    }

    public void AddCardToEnd(Transform _target)
    {
        if (grids.Count == 0) AddNewPage();
        if (UIInstruments.GetVisibleElements(grids[grids.Count - 1].transform) >= 6) AddNewPage();
        _target.parent = grids[grids.Count - 1].transform;
        _target.localScale = Vector3.one;
        elementsManagment.AddElement(_target.gameObject);
        needUpdate = true;
        contentMover.CalculateMinPos();
    }

    public void SetFirst(Transform _target)
    {
        _target.parent = grids[0].transform;
        _target.SetAsFirstSibling();
        elementsManagment.SetIndex(_target.gameObject, 0);
        needUpdate = true;
    }

    public void SetLast(Transform _target)
    {
        var addBtn = elementsManagment.GetAddBtn();
        var index = addBtn.GetSiblingIndex() + 1;
        _target.parent = addBtn.parent;
        _target.SetSiblingIndex(index);
        elementsManagment.SetIndex(_target.gameObject, index);
        needUpdate = true;
    }

    public void DeleteCard(Transform _target)
    {
        elementsManagment.RemoveElement(_target.gameObject);
        Destroy(_target.gameObject);
        needUpdate = true;
    }

    private void AddNewPage()
    {
        var instance = Instantiate(pagePref, pagesParent);
        var grid = instance.transform.Find("Content").GetComponent<GridLayoutGroup>();
        grids.Add(grid);
        CalculateContentSize();
    }

    private void CheckLastPage()
    {
        if (grids[grids.Count - 1].transform.childCount == 0)
            DeleteLastPage();
    }

    private void DeleteLastPage()
    {
        var target = grids[grids.Count - 1];
        grids.Remove(target);
        Destroy(target.transform.parent.gameObject);
    }

    private void SetGridValuesForPages(GridLayoutGroup grid)
    {
        grid.padding = gridCalculater.GridPadding;
        grid.cellSize = gridCalculater.GridCellSize;
        grid.spacing = gridCalculater.GridSpacing;
    }

    private int GetActivePageIndex()
    {
        var contentPos = pagesParent.GetComponent<RectTransform>().anchoredPosition.x;
        int index = (int) Math.Abs(contentPos / pageWidth);
        if (Math.Abs((contentPos % pageWidth)/pageWidth) > 0.5f) index++;
        return index;
    }

    public Transform GetPage(int _index)
    {
        if (_index > grids.Count - 1) return null;
        return grids[_index].transform;
    }

    private void LateUpdate()
    {
        if (needUpdate)
        {
            needUpdate = false;
            clustersControl.UpdateClusters(grids);
            CheckLastPage();
            contentMover.CalculateMinPos();
        }
    }

    public bool CanMove()
    {
        if (ActivePage == grids.Count - 1) return false;
        return UIInstruments.GetVisibleElements(grids[ActivePage + 1].transform) > 0;
    }
}
