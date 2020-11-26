using UnityEngine;
using UnityEngine.UI;

public class WordcomposingGridCalculater : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private GridLayoutGroup grid;

    public void SetRowConstraint(int _value)
    {
        grid.constraintCount = _value;
    }

    public void CalculateCategoryGrid()
    {
        var corners = new Vector3[4];
        rect.GetLocalCorners(corners);

        var width = 430;
        var height = corners[1].y - corners[0].y;
        var elementHeight = 0f;
        var elementWidth = 0f;
        var spacing = Vector2.zero;

        if (height / width < (grid.constraintCount + 0.5f + 6f* grid.constraintCount) / 24f)
        {
            elementHeight = (height / (grid.constraintCount + 0.5f + 6f * grid.constraintCount)) * 6f;
            spacing.y = height / (grid.constraintCount + 0.5f + 6f * grid.constraintCount);
            elementWidth = elementHeight * 0.8f;
            spacing.x = (width - elementWidth * 4f) / 4f;
            grid.padding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y, (int)spacing.y / 2);
            spacing = new Vector2(grid.padding.right * 2f, grid.padding.top);
            
            elementHeight = (height - 2.5f * spacing.y) / 2f;
            elementWidth = elementHeight * 0.8f;
        }
        else
        {
            elementWidth = (width / 24f) * 5f;
            spacing.x = width / 24f;
            elementHeight = elementWidth * 1.25f;
            spacing.y = (height - elementHeight * grid.constraintCount) / (grid.constraintCount + 0.5f);
            grid.padding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y, (int)spacing.y / 2);
            spacing = new Vector2(grid.padding.right * 2f, grid.padding.top);

            elementWidth = (width - spacing.x * 4f) / 4f;
            elementHeight = elementWidth * 1.25f;
        }

        grid.spacing = spacing;
        grid.cellSize = new Vector2(elementWidth, elementHeight);
    }

    public void CalculateCategoryContentSize(int _elementsCount)
    {
        var rect = grid.GetComponent<RectTransform>();
        float size = 0;
        int elementsInRow = grid.constraintCount;
        if (elementsInRow == 0) return;
        int rows = _elementsCount / elementsInRow;
        if (_elementsCount % elementsInRow != 0) rows++;
        size = rows * grid.cellSize.x + (rows - 1) * grid.spacing.x + grid.padding.left + grid.padding.right;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }
}
