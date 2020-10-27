using UnityEngine;
using UnityEngine.UI;

public static class UIInstruments
{
    public static void GetSizeForGrid(GridLayoutGroup _grid, int _elementsCount)
    {
        var rect = _grid.GetComponent<RectTransform>();
        float size = 0;
        var width = rect.sizeDelta.x * rect.localScale.x;
        var height = rect.sizeDelta.y * rect.localScale.y;
        size = height;
        int elementsInLine = (int)(width / _grid.cellSize.x);
        if (elementsInLine == 0) return;
        int lines = _elementsCount / elementsInLine;
        size = lines * _grid.cellSize.y * (_grid.spacing.y - 1) + _grid.padding.top + _grid.padding.bottom;
        if (size < height) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
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
        size = _elementHeight * _elementsCount + _elementsCount * _group.spacing + _group.padding.top + _group.padding.bottom;
        if (size < height) return;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}
