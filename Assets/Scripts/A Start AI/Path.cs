/*

            Handles all path logic

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that is used in other scipts.
/// </summary>
public class Paths
{
    /// <summary>
    /// The lookpoints for the waypoint.
    /// </summary>
    public readonly Vector3[] lookPoints;
    /// <summary>
    /// The turnBoundary for the waypoint.
    /// </summary>
    public readonly Line[] turnBoundaries;
    /// <summary>
    /// The finish line Index.
    /// </summary>
    public readonly int finishLineIndex;

    /// <summary>
    /// A constructor for the path.
    /// </summary>
    /// <param name="waypoints">Waypoints.</param>
    /// <param name="startPos">StartPosition of the path.</param>
    /// <param name="turnDst">The turnDistance.</param>
    public Paths(Vector3[] waypoints, Vector3 startPos, float turnDst)
    {
        lookPoints = waypoints;
        turnBoundaries = new Line[lookPoints.Length];
        finishLineIndex = turnBoundaries.Length - 1;

        Vector2 previousPoint = V3ToV2(startPos);
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(lookPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = turnBoundaryPoint;
        }
    }

    /// <summary>
    /// Converts a vector3 to a vector2.
    /// </summary>
    /// <param name="v3">A vector3.</param>
    /// <returns>A vector2.</returns>
    Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    /// <summary>
    /// Draws gizmos on the waypoints and the turn-line.
    /// </summary>
    public void DrawWithGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 p in lookPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }
        Gizmos.color = Color.white;
        foreach (Line l in turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}
