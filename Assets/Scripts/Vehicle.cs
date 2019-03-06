using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [HideInInspector]
    public bool isHalt;

    private Transform mTransform;
    public Vector3 Pos {
        get { return mTransform.position; }
        set { mTransform.position = value; }
    }
    public Vector3 Heading { get; private set; }
    public bool IsTurning { get; private set; }

    private Road[] roads;
    private Node startNode, endNode;
    private Road startRoad, endRoad;

    private List<MoveSegment> segments = new List<MoveSegment>();
    private int currentIdx;
    private bool isMoving;

    private void Awake()
    {
        mTransform = transform;
    }

    public void UpdateGame(float deltaTime)
    {
        if (isMoving && !isHalt) Move(deltaTime);
    }

    public void Init(Road[] roads)
    {
        this.roads = roads;
        FindPath();
    }
    
    private void FindPath()
    {
        segments.Clear();

        startRoad = endRoad ?? roads[Random.Range(0, roads.Length)];
        endRoad = startRoad;
        while (startRoad == endRoad)
        {
            endRoad = roads[Random.Range(0, roads.Length)];
        }
        startNode = endNode ?? startRoad.nodes[Random.Range(0, startRoad.nodes.Length)];
        endNode = endRoad.nodes[Random.Range(0, endRoad.nodes.Length)];

        System.Action<List<Node>> callback = path =>
        {
            currentIdx = 0;
            List<Vector3> pointList = new List<Vector3>();
            for (int i = 0; i < path.Count; i++) pointList.Add(path[i].Pos);
            CreateSegments(pointList);
            isMoving = true;
            RotateTo(segments[currentIdx].Heading);
            CheckTurnDirection();
        };

        PathManager.RequestPath(startNode, endNode, callback);
    }

    private void Move(float deltaTime)
    {
        var segment = segments[currentIdx];
        segment.Update(deltaTime, speed);
        Pos = segment.Pos;
        if (segment.DirectionChanged) RotateTo(segment.Heading);
        if (segment.IsDone)
        {
            currentIdx++;
            segment.Reset();
            if (currentIdx == segments.Count)
            {
                isMoving = false;
                FindPath();
            }
            else
            {
                CheckTurnDirection();
            }
        }
    }

    private void CheckTurnDirection()
    {
        IsTurning = segments[currentIdx].DirectionChanged;
    }

    private void RotateTo(Vector3 dir)
    {
        Heading = dir;
        mTransform.rotation = Quaternion.LookRotation(Heading);
    }

    private void CreateSegments(List<Vector3> pointList)
    {
        var segment = new MoveSegment();
        segment.AddPoint(pointList[0]);
        segments.Add(segment);

        for (int i = 0; i < pointList.Count - 1; i++)
        {
            var sqrDist = (pointList[i + 1] - pointList[i]).sqrMagnitude;
            bool isLast = i + 1 == pointList.Count - 1;
            if (sqrDist <= MoveExample3.CurveDistance * MoveExample3.CurveDistance)
            {
                var mid = (pointList[i] + pointList[i + 1]) * 0.5f;
                segment.AddPoint(mid);
                segment.Calculate();

                segment = new MoveSegment();
                segment.AddPoint(mid);
                if (!isLast) segment.AddPoint(pointList[i + 1]);
                segments.Add(segment);
            }
            else
            {
                var normal = (pointList[i + 1] - pointList[i]).normalized * MoveExample3.CurveDistance * 0.5f;
                var front = pointList[i] + normal;
                var back = pointList[i + 1] - normal;

                if (i == 0)
                {
                    segment.AddPoint(back);
                    segment.Calculate();
                }
                else
                {
                    segment.AddPoint(front);
                    segment.Calculate();

                    if (!isLast)
                    {
                        segment = new MoveSegment();
                        segment.AddPoint(front);
                        segment.AddPoint(back);
                        segment.Calculate();
                        segments.Add(segment);
                    }
                }

                segment = new MoveSegment();
                if (isLast)
                {
                    segment.AddPoint(front);
                }
                else
                {
                    segment.AddPoint(back);
                    segment.AddPoint(pointList[i + 1]);
                }
                segments.Add(segment);
            }
        }

        segment.AddPoint(pointList[pointList.Count - 1]);
        segment.Calculate();
    }

}
