using System.Collections.Generic;
using UnityEngine;

public class MoveSegment {
    public bool IsDone { get; private set; }
    public bool DirectionChanged { get { return points.Count == 3; } }
    public Vector3 Pos { get; private set; }
    private Vector3 heading;
    public Vector3 Heading { get { return heading; } }
    private List<Vector3> points = new List<Vector3>();
    private float moved;
    private float total;

    public void Update(float deltaTime, float speed)
    {
        if (points.Count == 3) Update3(deltaTime, speed);
        else if(points.Count == 2) Update2(deltaTime, speed);
    }

    public void Reset()
    {
        moved = 0;
        IsDone = false;
    }

    public void AddPoint(Vector3 p)
    {
        points.Add(p);
    }

    public void Calculate()
    {
        if (points.Count == 3) total = Vector3.Distance(points[0], points[1]) + Vector3.Distance(points[2], points[1]);
        if (points.Count == 2)
        {
            total = Vector3.Distance(points[0], points[1]);
            heading = points[1] - points[0];
        }
    }

    private void Update2(float deltaTime, float speed)
    {
        float dist = speed * deltaTime;
        moved += dist;
        float t = moved / total;
        if (t >= 1)
        {
            t = 1;
            IsDone = true;
        }
        Pos = Bezier.CalculateLinearBezierPoint(t, points[0], points[1]);
    }

    private void Update3(float deltaTime, float speed)
    {
        float dist = speed * deltaTime;
        moved += dist;
        float t = moved / total;
        if (t >= 1)
        {
            t = 1;
            IsDone = true;
        }
        Pos = Bezier.CalculateQuadraticBezierWithDirection(t, points[0], points[1], points[2], out heading);
    }
}
