using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsClusterControl : MonoBehaviour
{
    public void UpdateClusters(List<GridLayoutGroup> grids)
    {
        var gridCount = grids.Count;
        for (int i = 0; i < gridCount; i++)
        {
            if (UIInstruments.GetVisibleElements(grids[i].transform) < 6)
                FillGrid(grids, i);
            if (UIInstruments.GetVisibleElements(grids[i].transform) > 6)
                RelocateElements(grids, i);
        }
    }

    private void RelocateElements(List<GridLayoutGroup> grids, int _gridIndex)
    {
        if (_gridIndex > grids.Count - 1) return;
        var nextGrid = grids[_gridIndex + 1];
        var visibleElements = UIInstruments.GetVisibleElements(grids[_gridIndex].transform);
        for (int j = 0; j < visibleElements - 6; j++)
        {
            var targetChildIndex = grids[_gridIndex].transform.childCount - 1;
            var targetChild = grids[_gridIndex].transform.GetChild(targetChildIndex);
            targetChild.parent = nextGrid.transform;
            targetChild.SetAsFirstSibling();
            if (UIInstruments.GetVisibleElements(grids[_gridIndex].transform) == 6) return;
        }
    }

    private void FillGrid(List<GridLayoutGroup> grids, int _gridIndex)
    {
        if (_gridIndex >= grids.Count - 1) return;
        for (int i = 0; i < grids.Count - _gridIndex; i++)
        {
            var nextGrid = grids[_gridIndex + i];
            if (UIInstruments.GetVisibleElements(nextGrid.transform) > 0)
                for (int j = 0; j < nextGrid.transform.childCount; j++)
                {
                    nextGrid.transform.GetChild(0).parent = grids[_gridIndex].transform;
                    if (UIInstruments.GetVisibleElements(grids[_gridIndex].transform) == 6) return;
                }
            else continue;
        }
    }
}
