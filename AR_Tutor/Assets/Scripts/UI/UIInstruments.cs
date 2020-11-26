using UnityEngine;
using UnityEngine.UI;

public static class UIInstruments
{
    public static void GetSizeForVerticalGrid(GridLayoutGroup _grid, int _elementsCount)
    {
        var rect = _grid.GetComponent<RectTransform>();
        float size = 0;
        var height = rect.sizeDelta.y * rect.localScale.y;
        size = height;
        int elementsInLine = _grid.constraintCount;
        if (elementsInLine == 0) return;
        int lines = _elementsCount / elementsInLine;
        if (_elementsCount % elementsInLine != 0) lines++;
        size = lines * _grid.cellSize.y + (lines - 1) * _grid.spacing.y + _grid.padding.top + _grid.padding.bottom;
        if (size < height) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }

    public static void GetSizeForHorizontalGrid(GridLayoutGroup _grid, int _elementsCount)
    {
        var rect = _grid.GetComponent<RectTransform>();
        float size = 0;
        var width = rect.sizeDelta.x * rect.localScale.x;
        int elementsInRow = _grid.constraintCount;
        if (elementsInRow == 0) return;
        int rows = _elementsCount / elementsInRow;
        if (_elementsCount % elementsInRow != 0) rows++;
        size = rows * _grid.cellSize.x + (rows - 1) * _grid.spacing.x + _grid.padding.left + _grid.padding.right;
        if (size < width) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    public static void GetSizeForHorizontalGroup(HorizontalLayoutGroup _group, int _elementsCount, float _elementWidth, float _deltaWidth = 0f)
    {
        var rect = _group.GetComponent<RectTransform>();
        float size = 0;
        var width = rect.sizeDelta.x * rect.localScale.x;
        size = width;
        size = _elementWidth * _elementsCount + _elementsCount * _group.spacing + _group.padding.left + _group.padding.right + _deltaWidth;
        if (size < width) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    public static void GetSizeForVerticalGroup(VerticalLayoutGroup _group, int _elementsCount, float _elementHeight)
    {
        var rect = _group.GetComponent<RectTransform>();
        float size = 0;
        var height = rect.sizeDelta.y * rect.localScale.y;
        size = height;
        size = _elementHeight * _elementsCount + (_elementsCount - 1) * _group.spacing + _group.padding.top + _group.padding.bottom;
        if (size < height) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }

    public static int GetVisibleElements(Transform _target)
    {
        var visibleCategoryCount = 0;
        foreach (Transform element in _target)
            if (element.gameObject.activeSelf) visibleCategoryCount++;

        return visibleCategoryCount;
    }
}
