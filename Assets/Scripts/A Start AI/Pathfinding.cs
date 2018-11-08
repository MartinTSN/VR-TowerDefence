﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid GridReference;//For referencing the grid class

    void Awake()//When the program starts
    {
        requestManager = GetComponent<PathRequestManager>();
        GridReference = GetComponent<Grid>();//Get a reference to the game manager
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

        if (StartNode.IsWall && TargetNode.IsWall)
        {
            Heap<Node> OpenSet = new Heap<Node>(GridReference.MaxSize);//List of nodes for the open list
            HashSet<Node> ClosedSet = new HashSet<Node>();//Hashset of nodes for the closed list

            OpenSet.Add(StartNode);//Add the starting node to the open list to begin the program

            while (OpenSet.Count > 0)//Whilst there is something in the open list
            {
                Node CurrentNode = OpenSet.RemoveFirst();//Create a node and set it to the first item in the open list

                ClosedSet.Add(CurrentNode);//And add it to the closed list

                if (CurrentNode == TargetNode)//If the current node is the same as the target node
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
                {
                    if (!NeighborNode.IsWall || ClosedSet.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                    {
                        continue;//Skip it
                    }
                    int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode) + NeighborNode.movementPenalty;//Get the F cost of that neighbor

                    if (MoveCost < NeighborNode.gCost || !OpenSet.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                    {
                        NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                        NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                        NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                        if (!OpenSet.Contains(NeighborNode))//If the neighbor is not in the openlist
                        {
                            OpenSet.Add(NeighborNode);//Add it to the list
                        }
                        else
                        {
                            OpenSet.UpdateItem(NeighborNode);
                        }
                    }
                }

            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = GetFinalPath(StartNode, TargetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        foreach (Node node in FinalPath)
        {
            node.Position.y = 0.5f;
        }

        Vector3[] waypoints = SimplifyPath(FinalPath);


        Array.Reverse(waypoints);//Reverse the path to get the correct order
        return waypoints;


    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].Position);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.GridX - a_nodeB.GridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.GridY - a_nodeB.GridY);//y1-y2

        return ix + iy;//Return the sum
    }
}