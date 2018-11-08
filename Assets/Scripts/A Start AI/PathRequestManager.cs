/*

            Handles PathRequests.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A script that is put on the GameobjectManager.
/// </summary>
public class PathRequestManager : MonoBehaviour
{
    /// <summary>
    /// The PathRequest Queue.
    /// </summary>
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    /// <summary>
    /// The pathRequest that is being processed.
    /// </summary>
    PathRequest currentPathRequest;

    /// <summary>
    /// This instance.
    /// </summary>
    static PathRequestManager instance;
    /// <summary>
    /// The pathfinding script.
    /// </summary>
    Pathfinding pathfinding;

    /// <summary>
    /// Is it processing a path;
    /// </summary>
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    /// <summary>
    /// RequestPath constructor.
    /// </summary>
    /// <param name="pathStart">StartPosition of the path.</param>
    /// <param name="pathEnd">Endposition of the path.</param>
    /// <param name="callback">The function that is run after a path has been found.</param>
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    /// <summary>
    /// starts processing a path or tries to process the next path.
    /// </summary>
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    /// <summary>
    /// Run when the processing path is done.
    /// </summary>
    /// <param name="path">The path that is processed.</param>
    /// <param name="success">Did the path get found.</param>
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        /// <summary>
        /// StartPosition of the path.
        /// </summary>
        public Vector3 pathStart;
        /// <summary>
        /// Endposition of the path.
        /// </summary>
        public Vector3 pathEnd;
        /// <summary>
        /// The function that is run after a path has been found.
        /// </summary>
        public Action<Vector3[], bool> callback;

        /// <summary>
        /// The pathRequest constructor.
        /// </summary>
        /// <param name="_start">StartPosition of the path.</param>
        /// <param name="_end">Endposition of the path.</param>
        /// <param name="_callback">The function that is run after a path has been found.</param>
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}
