using UnityEngine;
using UnityEngine.UI;

public class TestRect : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private RectTransform rect;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("press");
            SetAnchor();
        }
        CalculateGridParam();
    }

    private void SetAnchor()
    {
        if (rect.anchorMin.y > 0.3)
        {
            Debug.Log("set 0");
            rect.anchorMin = new Vector2(0, 0);
        }
        else 
        {
            Debug.Log("set 0.35");
            rect.anchorMin = new Vector2(0, 0.35f);
        }
    }

    public void CalculateGridParam()
    {
        var corners = new Vector3[4];
        rect.GetLocalCorners(corners);

        var width = 420f;
        var height = corners[1].y - corners[0].y;

        Debug.Log(height);

        if (grid.constraint == GridLayoutGroup.Constraint.FixedRowCount)
        {
            var elementHeight = (height - grid.padding.top - grid.padding.bottom - grid.spacing.y * (grid.constraintCount - 1)) / grid.constraintCount;
            var elementWidth = elementHeight * 0.8f;
            grid.cellSize = new Vector2(elementWidth, elementHeight);
            int elementsInLine = (int)(width / elementWidth);
            var spacing = grid.spacing;
            spacing.x = (width - elementsInLine * elementWidth) / elementsInLine;
            if (spacing.x >= 5f)
            {
                grid.spacing = spacing;
                grid.padding.left = (int)grid.spacing.x / 2;
                grid.padding.right = (int)grid.spacing.x / 2;
            }
        }
    }
}
