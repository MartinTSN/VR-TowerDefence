using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public int GridX;//X Position in the Node Array
    public int GridY;//Y Position in the Node Array

    public bool IsWall;//Tells the program if this node is being obstructed.
    public Vector3 Position;//The world position of the node.

    public int movementPenalty;

    public Node ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public int gCost;//The cost of moving to the next square.
    public int hCost;//The distance to the goal from this node.

    int heapIndex;

    public int FCost//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.
    {
        get { return gCost + hCost; }
    }

    public Node(bool a_IsWall, Vector3 a_Pos, int a_gridX, int a_gridY, int _penalty)//Constructor
    {
        IsWall = a_IsWall;//Tells the program if this node is being obstructed.
        Position = a_Pos;//The world position of the node.
        GridX = a_gridX;//X Position in the Node Array
        GridY = a_gridY;//Y Position in the Node Array
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
