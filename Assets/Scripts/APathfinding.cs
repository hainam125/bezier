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

                float newMovementCostToNeighbour = gCosts[currentNode.id] + currentNode.Distance(neighbour);
                if (newMovementCostToNeighbour < gCosts[neighbour.id] || !openSet.Contains(neighbour))
                {
                    gCosts[neighbour.id] = newMovementCostToNeighbour;
                    hCosts[neighbour.id] = neighbour.Distance(targetNode);
                    parents[neighbour.id] = currentNode;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);
                    else openSet.UpdateItem(neighbour);
                }
			}
		}
        return null;
	}

    public static List<Node> RetracePath(Node startNode, Node endNode) {
		Node currentNode = endNode;
        Node prevNode = endNode;

		while (currentNode != startNode) {
            path.Add(currentNode);
            prevNode = currentNode;
            currentNode = parents[currentNode.id];
        }
        path.Add(startNode);
		path.Reverse();
		return path;
	}

    private static int Compare(Node nodeA, Node nodeB) {
        int compare = (gCosts[nodeA.id] + hCosts[nodeA.id]).CompareTo(gCosts[nodeB.id] + hCosts[nodeB.id]);
        if (compare == 0)
        {
            compare = hCosts[nodeA.id].CompareTo(hCosts[nodeB.id]);
        }
        return -compare;
    }
}
