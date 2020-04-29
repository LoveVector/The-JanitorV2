using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarNode 
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost;
    public int hCost;

    public int xPos;
    public int yPos;

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
}
