using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private static int currentId = 1;
    public int Id { get; private set; }
    public List<Node> neighbours;

    private Transform mTranform;
    public Vector3 Pos { get { return mTranform.position; } }
    public float GridX { get { return Pos.x; } }
    public float GridY { get { return Pos.z; } }

    private void Awake () {
        mTranform = transform;
        Id = currentId;
        currentId++;
    }

    public void AddNeighbour(Node node)
    {
        neighbours.Add(node);
    }

    public float Distance(Node other)
    {
        float dstX = Mathf.Abs(GridX - other.GridX);
        float dstY = Mathf.Abs(GridY - other.GridY);

        return dstX + dstY;
    }
}
