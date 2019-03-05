using UnityEngine;

public class Road : MonoBehaviour {
    [HideInInspector]
    public Node[] nodes;
    public float GridX { get { return transform.position.x; } }
    public float GridY { get { return transform.position.z; } }
    public Vector2 Bound { get; private set; }

    public void Init()
    {
        FindNodes();
        FindBound();
    }

    private void FindNodes()
    {
        var nodesTransform = transform.GetChild(0);
        int n = nodesTransform.childCount;
        nodes = new Node[n];
        for(int i = 0; i < n; i++)
        {
            nodes[i] = nodesTransform.GetChild(i).GetComponent<Node>();
        }
    }

    private void FindBound()
    {
        var viewTransform = transform.GetChild(1);
        var scale = viewTransform.localScale;
        Bound = new Vector2(scale.x * MoveExample3.Scale, scale.z * MoveExample3.Scale);
    }
}
