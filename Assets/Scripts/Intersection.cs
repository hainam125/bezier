using UnityEngine;

public class Intersection
{
    public Node[] nodes;
    public Road[] roads;
    private Transform transform;
    public float GridX { get { return transform.position.x; } }
    public float GridY { get { return transform.position.z; } }

    public Intersection(Transform transform)
    {
        this.transform = transform;
    }

    public void ConnectNodes()
    {
        if (nodes[0] != null)
        {
            if (roads[0] != null)
            {
                nodes[0].AddNeighbour(roads[0].nodes[1]);
            }
            if (roads[3] != null)
            {
                nodes[0].AddNeighbour(roads[3].nodes[0]);
            }
        }
        if (nodes[1] != null)
        {
            if (roads[0] != null)
            {
                nodes[1].AddNeighbour(roads[0].nodes[1]);
            }
            if (roads[1] != null)
            {
                nodes[1].AddNeighbour(roads[1].nodes[1]);
            }
        }
        if (nodes[2] != null)
        {
            if (roads[2] != null)
            {
                nodes[2].AddNeighbour(roads[2].nodes[0]);
            }
            if (roads[1] != null)
            {
                nodes[2].AddNeighbour(roads[1].nodes[1]);
            }
        }
        if (nodes[3] != null)
        {
            if (roads[2] != null)
            {
                nodes[3].AddNeighbour(roads[2].nodes[0]);
            }
            if (roads[3] != null)
            {
                nodes[3].AddNeighbour(roads[3].nodes[0]);
            }
        }

        if(roads[0] != null)
        {
            if (nodes[3] != null) roads[0].nodes[0].AddNeighbour(nodes[3]);
            if (nodes[2] != null) roads[0].nodes[0].AddNeighbour(nodes[2]);
        }
        if (roads[1] != null)
        {
            if (nodes[0] != null) roads[1].nodes[0].AddNeighbour(nodes[0]);
            if (nodes[3] != null) roads[1].nodes[0].AddNeighbour(nodes[3]);
        }
        if (roads[2] != null)
        {
            if (nodes[1] != null) roads[2].nodes[1].AddNeighbour(nodes[1]);
            if (nodes[0] != null) roads[2].nodes[1].AddNeighbour(nodes[0]);
        }
        if (roads[3] != null)
        {
            if (nodes[2] != null) roads[3].nodes[1].AddNeighbour(nodes[2]);
            if (nodes[1] != null) roads[3].nodes[1].AddNeighbour(nodes[1]);
        }
    }

    public void FindNodes()
    {
        var nodesTransform = transform.GetChild(0);
        int n = nodesTransform.childCount;
        nodes = new Node[n];
        for (int i = 0; i < n; i++)
        {
            nodes[i] = nodesTransform.GetChild(i).GetComponent<Node>();
        }
    }
}
