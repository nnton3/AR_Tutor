using UnityEngine;
using UnityEngine.UI;

public static class UIInstruments
{
    public static void CalculateContentSize(GridLayoutGroup grid, int elementsCount)
    {
        var rect = grid.GetComponent<RectTransform>();
        float size = 0;
        var width = rect.sizeDelta.x * rect.localScale.x;
        var height = rect.sizeDelta.y * rect.localScale.y;
        size = height;
        int elementsInLine = (int)(width / grid.cellSize.x);
        if (elementsInLine == 0) return;
        int lines = elementsCount / elementsInLine;
        size = lines * grid.cellSize.y * (grid.spacing.y - 1) + grid.padding.top + grid.padding.bottom;
        if (size < height) return;
        Debug.Log(grid.cellSize.x);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}
