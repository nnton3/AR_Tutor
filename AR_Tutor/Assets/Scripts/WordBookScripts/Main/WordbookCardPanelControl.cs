using UnityEngine;
using System.Collections;

public class WordbookCardPanelControl : MonoBehaviour
{
    [SerializeField] private WordbookCardGridCalculater gridCalculater;
    [SerializeField] private VerticalContentMover contentMover;

    public void UpdateGrid()
    {
        gridCalculater.CalculateContentSize(UIInstruments.GetVisibleElements(transform.Find("Mask/Content")));
        contentMover.UpdateMoveStep(gridCalculater.PanelHeight);
    }

    public void CalculateGrid()
    {
        gridCalculater.CalculateCardGrid();
    }
}
