using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode : IHeapItem<AstarNode>
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public int xPos;
    public int yPos;
    int heapIndex;
    public bool closed = false;

    public AstarNode parent;
    public AstarNode(Vector3 _worldPosition, bool _walkable, int _xPos, int _yPos)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        xPos = _xPos;
        yPos = _yPos;
    }

    public int fCost 
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(AstarNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
