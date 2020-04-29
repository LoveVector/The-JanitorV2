using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    Grid grid;

    AstarNode currentNode;
    AstarNode startNode;
    AstarNode endNode;

    bool found;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        grid = GetComponent<Grid>();
    }

    public List<Vector3> PathFind(Vector3 startPosition, Vector3 endPosition)
    {
        found = false;
        startNode = grid.GetNodePoint(startPosition);
        endNode = grid.GetNodePoint(endPosition);
        List<AstarNode> nodes = new List<AstarNode>();
        startNode.hCost = GetDistance(startNode, endNode);
        nodes.Add(startNode);

        while (nodes.Count > 0)
        {
            currentNode = nodes[0];
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].fCost < currentNode.fCost || nodes[i].fCost == currentNode.fCost && nodes[i].hCost < currentNode.hCost)
                {
                    currentNode = nodes[i];
                }
            }

            nodes.Remove(currentNode);
            currentNode.closed = true;

            if (currentNode == endNode)
            {
                found = true;
                return SendPath();
            }

            List<AstarNode> neighbors = grid.GetNeighbors(currentNode);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].closed == true || neighbors[i].walkable == false)
                {
                    continue;
                }

                int distanceToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbors[i]);

                if (distanceToNeighbor < neighbors[i].gCost || nodes.Contains(neighbors[i]) != true)
                {
                    neighbors[i].gCost = distanceToNeighbor;
                    neighbors[i].hCost = GetDistance(neighbors[i], endNode);
                    neighbors[i].parent = currentNode;

                    if (nodes.Contains(neighbors[i]) != true)
                    {
                        nodes.Add(neighbors[i]);
                    }
                }
            }
        }
        return null;
    }

    List<Vector3> SendPath()
    {
        List<AstarNode> path = new List<AstarNode>();
        List<Vector3> waypoints = new List<Vector3>();
        currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        Vector2 directionOld = Vector3.zero;

        for (int i = 0; i < path.Count; i++)
        {
            if (i != path.Count - 1)
            {
                Vector2 directionNew = new Vector2(path[i + 1].xPos - path[i].xPos, path[i + 1].yPos - path[i].yPos);
                if (directionOld != directionNew)
                {
                    waypoints.Add(path[i].worldPosition);
                    directionOld = directionNew;
                }
            }
            else
            {
                waypoints.Add(path[i].worldPosition);
            }
        }
        return waypoints;
    }

    int GetDistance(AstarNode a, AstarNode b)
    {
        int distX = Mathf.Abs(a.xPos - b.xPos);
        int distY = Mathf.Abs(a.yPos - b.yPos);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distX);
        }
    }
}
