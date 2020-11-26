using UnityEngine;

public class WordComposingCardPanel : MonoBehaviour
{
    [SerializeField] private WordcomposingGridCalculater gridCalculater;
    [SerializeField] private HorizontalContentMover contentMover;

    public void UpdateGrid(int _constraintValue = 2)
    {
        gridCalculater.SetRowConstraint(_constraintValue);
        gridCalculater.CalculateCategoryGrid();

        gridCalculater.CalculateCategoryContentSize(UIInstruments.GetVisibleElements(transform.Find("Mask/Content")));
        contentMover.CalculateMinPos();
    }
}
