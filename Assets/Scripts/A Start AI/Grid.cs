/*

            Handles the Grid logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is put on the GameobjectManager.
/// </summary>
public class Grid : MonoBehaviour
{
    /// <summary>
    /// Displays Gizmos.
    /// </summary>
    public bool displayGridGizmos;
    /// <summary>
    /// This is where the program will start the pathfinding from.
    /// </summary>
    public Transform StartPosition;
    /// <summary>
    /// This is the mask that the program will look for when trying to find obstructions to the path.
    /// </summary>
    public LayerMask WallMask;
    /// <summary>
    /// A vector2 to store the width and height of the graph in world units.
    /// </summary>
    public Vector2 GridWorldSize;
    /// <summary>
    /// This stores how big each square on the graph will be.
    /// </summary>
    public float NodeRadius;
    /// <summary>
    /// The Walkable regions.
    /// </summary>
    public TerrainType[] walkableRegions;
    public int obstacleProximityPenalty = 10;
    /// <summary>
    /// Sets the regions Penalty value.
    /// </summary>
    Dictionary<int, int> walkableRegionDictonary = new Dictionary<int, int>();
    LayerMask walkableMask;

    /// <summary>
    /// The array of nodes that the A Star algorithm uses.
    /// </summary>
    Node[,] nodeArray;

    /// <summary>
    /// Twice the amount of the radius (Set in the start function)
    /// </summary>
    float nodeDiameter;
    /// <summary>
    /// Size of the Grid in Array units.
    /// </summary>
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    void Awake()
    {
        nodeDiameter = NodeRadius * 2;//Double the radius to get diameter
        gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.

        foreach (TerrainType region in walkableRegions)
        {
            walkableMask.value = walkableMask |= region.terrainMask.value;
            walkableRegionDictonary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();//Draw the grid
    }

    /// <summary>
    /// Gets the max-size of the grid.
    /// </summary>
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    /// <summary>
    /// Creates the grid.
    /// </summary>
    public void CreateGrid()
    {
        nodeArray = new Node[gridSizeX, gridSizeY];//Declare the array of nodes.
        Vector3 bottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 - Vector3.forward * GridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < gridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + NodeRadius) + Vector3.forward * (y * nodeDiameter + NodeRadius);//Get the world coordinates of the bottom left of the graph
                bool Wall = !(Physics.CheckSphere(worldPoint, NodeRadius, WallMask));

                int movementPenalty = 0;


                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit, 100, walkableMask))
                {
                    walkableRegionDictonary.TryGetValue(Hit.collider.gameObject.layer, out movementPenalty);
                }
                if (!Wall)
                {
                    movementPenalty += obstacleProximityPenalty;
                }

                nodeArray[x, y] = new Node(Wall, worldPoint, x, y, movementPenalty);//Create a new node in the array.
            }
        }
        //BlurPenaltyMap(3);
    }

    /// <summary>
    /// Function that gets the neighboring nodes of the given node.
    /// </summary>
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.GridX + 1;
        icheckY = a_NeighborNode.GridY;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.GridX - 1;
        icheckY = a_NeighborNode.GridY;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.GridX;
        icheckY = a_NeighborNode.GridY + 1;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.GridX;
        icheckY = a_NeighborNode.GridY - 1;
        if (icheckX >= 0 && icheckX < gridSizeX)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < gridSizeY)//If the YPosition is in range of the array
            {
                NeighborList.Add(nodeArray[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    /// <summary>
    /// Gets the closest node to the given world position.
    /// </summary>
    /// <param name="a_vWorldPos">The world position.</param>
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float xPos = ((a_vWorldPos.x + GridWorldSize.x / 2) / GridWorldSize.x);
        float yPos = ((a_vWorldPos.z + GridWorldSize.y / 2) / GridWorldSize.y);

        xPos = Mathf.Clamp01(xPos);
        yPos = Mathf.Clamp01(yPos);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xPos);
        int y = Mathf.RoundToInt((gridSizeY - 1) * yPos);

        return nodeArray[x, y];
    }

    /// <summary>
    /// Function that draws the wireframe. AKA Draws the grid.
    /// </summary>    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector


        if (nodeArray != null && displayGridGizmos)//If the grid is not empty
        {
            foreach (Node n in nodeArray)
            {

                Gizmos.color = Color.Lerp(Color.white, Color.red, Mathf.InverseLerp(penaltyMin, penaltyMax, n.movementPenalty));
                Gizmos.color = (n.IsWall) ? Gizmos.color : Color.red;
                Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter));
            }
        }
    }

    /// <summary>
    /// Gets the nearest point to the position.
    /// </summary>
    /// <param name="pos">The position.</param>
    /// <returns>The nearest point.</returns>
    public Vector3 GetNearestPoint(Vector3 pos)
    {
        pos -= transform.position;

        int xCount = Mathf.RoundToInt(pos.x);
        int yCount = Mathf.RoundToInt(pos.y);
        int zCount = Mathf.RoundToInt(pos.z);

        Vector3 result = new Vector3((float)xCount, (float)yCount + 0.5f, (float)zCount);

        result += transform.position;

        return result;
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
