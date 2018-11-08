/*

            Handles Node logic.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is used in other scripts.
/// </summary>
public class Node : IHeapItem<Node>
{
    /// <summary>
    /// X Position in the Node Array
    /// </summary>
    public int GridX;
    /// <summary>
    /// Y Position in the Node Array
    /// </summary>
    public int GridY;
    /// <summary>
    /// Tells the program if this node is being obstructed.
    /// </summary>
    public bool IsWall;
    /// <summary>
    /// The world position of the node.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// The movement penalty of the node.
    /// </summary>
    public int movementPenalty;

    /// <summary>
    /// For the AStar algoritm, will store what node it previously came from so it can trace the shortest path.
    /// </summary>
    public Node ParentNode;

    /// <summary>
    /// The cost of moving to the next square.
    /// </summary>
    public int gCost;
    /// <summary>
    /// The distance to the goal from this node.
    /// </summary>
    public int hCost;

    /// <summary>
    /// The heapIndex.
    /// </summary>
    int heapIndex;

    /// <summary>
    /// Get function to add G cost and H Cost.
    /// </summary>
    public int FCost
    {
        get { return gCost + hCost; }
    }

    /// <summary>
    /// A constructor for the node.
    /// </summary>
    /// <param name="a_IsWall">Tells the program if this node is being obstructed.</param>
    /// <param name="a_Pos">The world position of the node.</param>
    /// <param name="a_gridX">X Position in the Node Array.</param>
    /// <param name="a_gridY">Y Position in the Node Array.</param>
    /// <param name="_penalty">The penalty for the node.</param>
    public Node(bool a_IsWall, Vector3 a_Pos, int a_gridX, int a_gridY, int _penalty)
    {
        IsWall = a_IsWall;
        Position = a_Pos;
        GridX = a_gridX;
        GridY = a_gridY;
        movementPenalty = _penalty;
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

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
