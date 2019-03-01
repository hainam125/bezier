using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveExample2 : MonoBehaviour
{
    public Transform player;
    public float speed = 5;
    public float curveDist = 1;
    public Transform map;
    
    public List<MoveSegment> segments = new List<MoveSegment>();
    private int currentIdx;
    private bool isMoving;

    private void Start()
    {
        List<Vector3> pointList = new List<Vector3>();
        for (int i = 0; i < map.childCount; i++) pointList.Add(map.GetChild(i).position);
        CreateSegments(pointList);
        isMoving = true;
        player.transform.position = pointList[0];
        RotatePlayer(segments[0].Heading);
    }

    private void Update()
    {
        if (isMoving) Move();
    }
    private void Move()
    {
        var segment = segments[currentIdx];
        segment.Update(Time.deltaTime, speed);
        player.transform.position = segment.Pos;
        if (segment.DirectionChanged) RotatePlayer(segment.Heading);
        if (segment.IsDone)
        {
            currentIdx++;
            segment.Reset();
            if (currentIdx == segments.Count)
            {
                currentIdx = 0;
                //isMoving = false;
            }
        }
    }

    private void RotatePlayer(Vector3 dir)
    {
        float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        player.eulerAngles = new Vector3(0, 0, a);
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
            if (sqrDist <= curveDist * curveDist)
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
                var normal = (pointList[i + 1] - pointList[i]).normalized * curveDist * 0.5f;
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
