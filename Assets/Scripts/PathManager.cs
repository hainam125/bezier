using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private static PathManager instance;
    /*public static PathManager Instance
    {
        get
        {
            if (instance == null) instance = Object.FindObjectOfType<PathManager>();
            return instance;
        }
    }*/

    private void Awake()
    {
        APathfinding.Init(100);
    }

    public static List<Node> FindPath(Node startNode, Node endNode)
    {
        return APathfinding.FindPath(startNode, endNode);
    }
}
