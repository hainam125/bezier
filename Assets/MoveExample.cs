using System.Collections.Generic;
using UnityEngine;

public class MoveExample : MonoBehaviour
{
    public Transform player;
    public float speed = 5;
    public Transform map;

    private List<Transform> pointList = new List<Transform>();
    private List<MoveSegment> segments = new List<MoveSegment>();
    private int currentIdx;
    private bool isMoving;

    private void Start ()
    {
        for (int i = 0; i < map.childCount; i++) pointList.Add(map.GetChild(i));
        CreateSegments();
        isMoving = true;
        player.transform.position = pointList[0].position;
    }
	
	private void Update ()
    {
        if (isMoving) Move();
    }
    private void Move()
    {
        var segment = segments[currentIdx];
        segment.Update(Time.deltaTime, speed);
        player.transform.position = segment.Pos;
        if (segment.DirectionChanged)
        {
            Vector3 dir = segment.Heading;
            float a = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            player.eulerAngles = new Vector3(0, 0, a);
        }
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
    private void CreateSegments()
    {
        var segment = new MoveSegment();
        segment.AddPoint(pointList[0].position);
        segments.Add(segment);

        for (int i = 1; i < pointList.Count - 2; i++)
        {
            if((pointList[i].position.x == pointList[i + 2].position.x && pointList[i].position.x == pointList[i + 1].position.x) ||
               (pointList[i].position.y == pointList[i + 2].position.y && pointList[i].position.y == pointList[i + 1].position.y))
            {
                continue;
            }
            segment.AddPoint(pointList[i].position);
            segment.Calculate();

            segment = new MoveSegment();
            segment.AddPoint(pointList[i].position);
            segment.AddPoint(pointList[i + 1].position);
            segment.AddPoint(pointList[i + 2].position);
            segment.Calculate();
            segments.Add(segment);

            segment = new MoveSegment();
            segment.AddPoint(pointList[i + 2].position);
            segments.Add(segment);

            i +=1;
        }
        segment.AddPoint(pointList[pointList.Count - 1].position);
        segment.Calculate();
    }
}
