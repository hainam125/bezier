using UnityEngine;

public class Road : MonoBehaviour {
    public Node[] nodes;
    public Intersection[] intersections;
    
    public void FindNodes()
    {
        var nodesTransform = transform.GetChild(0);
        int n = nodesTransform.childCount;
        nodes = new Node[n];
        for(int i = 0; i < n; i++)
        {
            nodes[i] = nodesTransform.GetChild(i).GetComponent<Node>();
        }
    }
}
