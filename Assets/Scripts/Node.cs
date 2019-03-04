using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int id;
    public List<Node> neighbours;

    private Transform mTranform;
    public Vector3 Pos { get { return mTranform.position; } }

	private void Awake () {
        mTranform = transform;
    }

    public float Distance(Node other)
    {
        float dstX = Mathf.Abs(Pos.x - other.Pos.x);
        float dstY = Mathf.Abs(Pos.y - other.Pos.y);

        return dstX + dstY;
    }
}
