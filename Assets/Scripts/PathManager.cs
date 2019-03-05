using System;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private static PathManager instance;

    private Queue<PathRequest> queue = new Queue<PathRequest>();

    private void Awake()
    {
        instance = this;
        APathfinding.Init(150);
    }

    private void Update()
    {
        if(queue.Count > 0)
        {
            var request = queue.Dequeue();
            request.Find();
        }
    }

    public static void RequestPath(Node startNode, Node endNode, Action<List<Node>> callback)
    {
        instance.queue.Enqueue(new PathRequest(startNode, endNode, callback));
    }
}

public class PathRequest
{
    private Node start, end;
    private Action<List<Node>> callback;

    public PathRequest(Node startNode, Node endNode, Action<List<Node>> cb)
    {
        start = startNode;
        end = endNode;
        callback = cb;
    }

    public void Find()
    {
        var r = APathfinding.FindPath(start, end);
        callback.Invoke(r);
    }
}
