using System.Collections.Generic;

public class APathfinding {
    private static Node[] parents;
    private static float[] gCosts;
    private static float[] hCosts;
    private static Heap<Node> openSet;
    private static HashSet<Node> closedSet;
    private static List<Node> path;

    public static void Init(int size)
    {
        gCosts = new float[size];
        hCosts = new float[size];
        parents = new Node[size];
        openSet = new Heap<Node>(size, Compare);
        closedSet = new HashSet<Node>();
        path = new List<Node>();
    }

    public static List<Node> FindPath(Node startNode, Node targetNode) {
        closedSet.Clear();
        openSet.Clear();
		gCosts.Clear();
        hCosts.Clear();
        parents.Clear();
        path.Clear();

        openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.RemoveFirst();
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				return RetracePath(startNode,targetNode);
			}

			foreach (Node neighbour in currentNode.neighbours) {
				if (closedSet.Contains(neighbour)) {
					continue;
				}

                float newMovementCostToNeighbour = gCosts[currentNode.Id] + currentNode.Distance(neighbour);
                if (newMovementCostToNeighbour < gCosts[neighbour.Id] || !openSet.Contains(neighbour))
                {
                    gCosts[neighbour.Id] = newMovementCostToNeighbour;
                    hCosts[neighbour.Id] = neighbour.Distance(targetNode);
                    parents[neighbour.Id] = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
			}
		}
        return null;
	}

    public static List<Node> RetracePath(Node startNode, Node endNode) {
		Node currentNode = endNode;

		while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = parents[currentNode.Id];
        }
        path.Add(startNode);
		path.Reverse();
		return path;
	}

    private static int Compare(Node nodeA, Node nodeB) {
        int compare = (gCosts[nodeA.Id] + hCosts[nodeA.Id]).CompareTo(gCosts[nodeB.Id] + hCosts[nodeB.Id]);
        if (compare == 0)
        {
            compare = hCosts[nodeA.Id].CompareTo(hCosts[nodeB.Id]);
        }
        return -compare;
    }
}
