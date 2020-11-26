using UnityEngine;

public class VariantGridCalculater : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    public Vector2 GridCellSize { get; private set; } = new Vector2();
    public Vector2 GridSpacing { get; private set; } = new Vector2();
    public RectOffset GridPadding { get; private set; } = new RectOffset();

    public void CalculateGrid(Vector2 _visibleAreaSize)
    {
        var elementHeight = 0f;
        var elementWidth = 0f;
        var spacing = Vector2.zero;

        if (_visibleAreaSize.y/ _visibleAreaSize.x < 14f / 18f)
        {
            elementHeight = (_visibleAreaSize.y / 14f) * 6f;
            spacing.y = _visibleAreaSize.y / 14f;
            elementWidth = elementHeight * 0.8f;
            spacing.x = (_visibleAreaSize.x - elementWidth * 3f) / 3f;
            GridPadding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y/2, (int)spacing.y / 2);
            spacing = new Vector2(GridPadding.right * 2f, GridPadding.top * 2f);

            elementHeight = (_visibleAreaSize.y - 2f * spacing.y) / 2f;
            elementWidth = elementHeight * 0.8f;
        }
        else
        {
            elementWidth = (_visibleAreaSize.x / 3f) * 5f / 6f;
            spacing.x = (_visibleAreaSize.x / 3f) * (1f / 6f);
            elementHeight = elementWidth * 1.25f;
            spacing.y = (_visibleAreaSize.y - elementHeight * 2f) / 2f;
            GridPadding = new RectOffset((int)spacing.x / 2, (int)spacing.x / 2, (int)spacing.y / 2, (int)spacing.y / 2);
            spacing = new Vector2(GridPadding.right * 2f, GridPadding.top * 2f);

            elementWidth = (_visibleAreaSize.x - 3f * spacing.x)/3f ;
            elementHeight = elementWidth * 1.25f;
        }

        GridSpacing = spacing;
        GridCellSize = new Vector2(elementWidth, elementHeight);
    }
}
