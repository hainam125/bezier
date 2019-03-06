using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private static int currentId = 1;
    public int Id { get; private set; }
    public List<Node> Neighbours { get; private set; }

    private Transform mTransform;
    public Vector3 Pos { get { return mTransform.position; } }
    public float GridX { get { return Pos.x; } }
    public float GridY { get { return Pos.z; } }

    public Node(Transform transform) {
        mTransform = transform;
        Neighbours = new List<Node>();
        Id = currentId;
        currentId++;
    }

    public void AddNeighbour(Node node)
    {
        Neighbours.Add(node);
    }

    public float Distance(Node other)
    {
        float dstX = Mathf.Abs(GridX - other.GridX);
        float dstY = Mathf.Abs(GridY - other.GridY);

        return dstX + dstY;
    }
}
