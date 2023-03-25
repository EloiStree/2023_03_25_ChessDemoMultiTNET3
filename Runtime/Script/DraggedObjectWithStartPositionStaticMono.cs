using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggedObjectWithStartPositionStaticMono : MonoBehaviour
{
    [ContextMenu("Recover all position")]
    public void RecoverAllPositionsOfAllObjectInScene()
    {
        DraggedObjectWithStartPositionMono[] targetToReset = new DraggedObjectWithStartPositionMono[0];
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref targetToReset);
        foreach (var item in targetToReset)
        {
            if (item != null)
                item.RecoverCurrentPositionAsStartPoint();
        }
    }
    [ContextMenu("Unclaim All")]
    public void UnclaimAll()
    {
        DraggedObjectWithStartPositionMono[] targetToReset = new DraggedObjectWithStartPositionMono[0];
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref targetToReset);
        foreach (var item in targetToReset)
        {
            if (item != null)
                item.UnclaimObject();
        }
    }
    [ContextMenu("Put all position cimetary ")]
    public void PutAllInCimetary()
    {
        LinkedCimeteryChessMono[] targetToReset = new LinkedCimeteryChessMono[0];
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref targetToReset);
        foreach (var item in targetToReset)
        {
            if (item != null)
                item.MoveToCimetary();
        }
    }

    [ContextMenu("Put all position start")]
    public void PutAllAtStart()
    {
        LinkedStartPointChessMono[] targetToReset = new LinkedStartPointChessMono[0];
        Eloi.E_SearchInSceneUtility.TryToFetchWithActiveInScene(ref targetToReset);
        foreach (var item in targetToReset)
        {
            if (item != null)
                item.MoveToStartPoint();
        }
    }
}
