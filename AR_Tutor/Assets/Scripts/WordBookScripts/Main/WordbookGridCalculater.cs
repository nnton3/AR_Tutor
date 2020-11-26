using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordbookGridCalculater : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private GridLayoutGroup grid;
    public float PanelHeight { get; private set; }

    public void CalculateCategoryGrid()
    {
        var corners = new Vector3[4];
        rect.GetLocalCorners(corners);

        var width = 290f;
        var height = corners[1].y - corners[0].y;
        PanelHeight = height;
        var elementWidth = 0f;
        var elementHeight = 0f;
        var spacing = Vector2.zero;

        if (height/width < 14f/ 7f)
        {
            elementHeight = (height / 14f) * 6f;
            spacing.y = height / 14f;
            elementWidth = elementHeight * 0.8f;
            spacing.x = (width - elementWidth) / 2f;
            grid.padding = new RectOffset(0, 0, (int)spacing.y/2, (int)spacing.y/2);
            spacing = new Vector2(0f, grid.padding.top * 2f);

            elementHeight = (height - 2f * spacing.y) / 2f;
            elementWidth = elementHeight * 0.8f;
        }
        else
        {
            elementWidth = (width / 7f) * 5f;
            spacing.x = width / 7f;
            elementHeight = elementWidth * 1.25f;
            spacing.y = (height - elementHeight * 2f) / 3f;
            grid.padding = new RectOffset(0, 0, (int)spacing.y, (int)spacing.y);
            spacing = new Vector2(0f, grid.padding.top);

            elementWidth = width - 2f * spacing.x;
            elementHeight = elementWidth * 1.25f;
        }

        grid.spacing = spacing;
        grid.cellSize = new Vector2(elementWidth, elementHeight);
    }

    public void CalculateContentSize(int _elementsCount)
    {
        var size = 0f;
        size = grid.cellSize.y * _elementsCount + grid.spacing.y * (_elementsCount - 1) + grid.padding.top + grid.padding.bottom;
        grid.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
    }
}
