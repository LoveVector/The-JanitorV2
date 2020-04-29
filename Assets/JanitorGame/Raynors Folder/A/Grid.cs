using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public float gridSizeX;
    public float gridSizeY;
    public float nodeRadius;

    AstarNode[,] node;

    public GameObject seeker;
    public GameObject target;

    public LayerMask unwalkable;

    float nodeDiameter;
    int totalNodes;
    int totalNodesX;
    int totalNodesY;

    Vector3 bottomLeft;

    public List<AstarNode> path;
    // Start is called before the first frame update
    void Awake()
    {
        path = new List<AstarNode>();
        nodeDiameter = nodeRadius * 2;
        totalNodesX = Mathf.RoundToInt(gridSizeX / nodeDiameter);
        totalNodesY = Mathf.RoundToInt(gridSizeY / nodeDiameter);
        bottomLeft = transform.position - Vector3.right * gridSizeX / 2 - Vector3.forward * gridSizeY / 2;

        node = new AstarNode[totalNodesX, totalNodesY];

        CreateNodes();
    }

    // Update is called once per frame
    void Update()
    {
        NodeChecks();
    }

    void CreateNodes()
    {
        int i = 0;
        for (int x = 0; x < totalNodesX; x++)
        {
            for (int y = 0; y < totalNodesY; y++)
            {
                Vector3 worldPosition = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPosition, nodeRadius, unwalkable));
                node[x,y] = new AstarNode(worldPosition, walkable, x, y);
            }
        }
    }

    public AstarNode GetNodePoint(Vector3 worldposition)
    {
        float x = (worldposition.x  + gridSizeX / 2) / gridSizeX;
        float y = (worldposition.z + gridSizeY / 2) / gridSizeY;

        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        int x1 = Mathf.RoundToInt((totalNodesX - 1) * x);
        int y1 = Mathf.RoundToInt((totalNodesY - 1) * y);

        return node[x1, y1];
    }

    public List<AstarNode> GetNeighbors(AstarNode node1)
    {
        List<AstarNode> neighbors =  new List<AstarNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int xPos = node1.xPos + x;
                int yPos = node1.yPos + y;

                if(xPos >= 0 && xPos < totalNodesX && yPos >= 0 && yPos < totalNodesY)
                {
                    neighbors.Add(node[xPos, yPos]);
                }
            }
        }
        return neighbors;
    }

    void NodeChecks()
    {
        for (int i = 0; i < totalNodesX; i++)
        {
            for (int y = 0; y < totalNodesY; y++)
            {
                node[i, y].closed = false;
                node[i, y].walkable = !(Physics.CheckSphere(node[i, y].worldPosition, nodeRadius, unwalkable));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, 0, gridSizeY));
   
        for (int i = 0; i < totalNodesX; i++)
        {
            for (int y = 0; y < totalNodesY; y++)
            {
                if (node[i, y].walkable == true) 
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                if (path != null && path.Contains(node[i, y]))
                {
                    Gizmos.color = Color.yellow;
                }
                Gizmos.DrawCube(node[i,y].worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
