using UnityEngine;
using UnityEngine.UI;

public class WordbookCardGridCalculater : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private RectTransform visibleAreaRect;
    [SerializeField] private GridLayoutGroup grid;
    public float PanelHeight { get; private set; }

    public void CalculateCardGrid()
    {
        var corners = new Vector3[4];
        visibleAreaRect.GetLocalCorners(corners);

        var width = 392f;
        var height = corners[1].y - corners[0].y;
        PanelHeight = height;
        var elementWidth = 0f;
        var elementHeight = 0f;
        var spacing = Vector2.zero;

        if (height / width < 14f / 24f)
        {
            elementHeight = (height / 14f) * 6f;
            spacing.y = height / 14f;
            elementWidth = elementHeight * 0.8f;
            spacing.x = (width - elementWidth * 4f) / 4f;
            grid.padding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y / 2, (int)spacing.y / 2);
            spacing = new Vector2(grid.padding.right * 2f, grid.padding.top * 2f);

            elementHeight = (height - 2f * spacing.y) / 2f;
            elementWidth = elementHeight * 0.8f;
        }
        else
        {
            elementWidth = (width / 24f) * 5f;
            spacing.x = width / 24f;
            elementHeight = elementWidth * 1.25f;
            spacing.y = (height - elementHeight * 2f) / 2f;
            grid.padding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y / 2, (int)spacing.y / 2);
            spacing = new Vector2(grid.padding.right * 2f, grid.padding.top * 2f);

            elementWidth = (width - 4f * spacing.x) / 4f;
            elementHeight = elementWidth * 1.25f;
        }

        grid.spacing = spacing;
        grid.cellSize = new Vector2(elementWidth, elementHeight);
    }

    public void CalculateContentSize(int _elementsCount)
    {
        float size = 0;
        var rows = _elementsCount / 4;
        if (_elementsCount % 4 != 0) rows++;
        size = (grid.cellSize.y + grid.spacing.y) * rows;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        rect.ForceUpdateRectTransforms();
    }
}
