using UnityEngine;

public class Bezier : MonoBehaviour {
    public LineRenderer lineRenderer;
    public Transform point0, point1, point2, point3;
    public Transform player;

    public Transform[] pointTransforms;

    private int numPoints = 50;
    private Vector3[] positions;

    private void Start () {
        positions = new Vector3[numPoints];
        lineRenderer.positionCount = numPoints;
    }

    private void Update()
    {
        DrawGeneralCurve();
        //MovePlayer();
        //DrawCubicCurve();
        //DrawQuadraticCurve();
        //DrawLinearCurve();
    }

    private void MovePlayer()
    {
        var time = 3;
        var t = (Time.timeSinceLevelLoad % time) / time;
        player.position = CalculateCubicBezierPoint2(t, point0.position, point1.position, point2.position, point3.position);
    }

    private void DrawGeneralCurve()
    {
        positions[0] = pointTransforms[0].position;
        positions[numPoints - 1] = pointTransforms[pointTransforms.Length - 1].position;
        var pointPositions = new Vector3[pointTransforms.Length];
        for (int i = 0; i < pointPositions.Length; i++) pointPositions[i] = pointTransforms[i].position;
        for (int i = 1; i < numPoints - 1; i++)
        {
            float t = i * 1f / (numPoints - 1);
            positions[i] = CalculateGeneralBezier(t, pointPositions);
        }
        lineRenderer.SetPositions(positions);
    }

    private void DrawCubicCurve()
    {
        positions[0] = point0.position;
        positions[numPoints - 1] = point3.position;
        for (int i = 1; i < numPoints - 1; i++)
        {
            float t = i * 1f / (numPoints - 1);
            positions[i] = CalculateCubicBezierPoint2(t, point0.position, point1.position, point2.position, point3.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private void DrawQuadraticCurve()
    {
        positions[0] = point0.position;
        positions[numPoints - 1] = point2.position;
        for (int i = 1; i < numPoints - 1; i++)
        {
            float t = i * 1f / (numPoints - 1);
            positions[i] = CalculateQuadraticBezierPoint2(t, point0.position, point1.position, point2.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private void DrawLinearCurve()
    {
        positions[0] = point0.position;
        positions[numPoints - 1] = point1.position;
        for(int i = 1; i < numPoints - 1; i++)
        {
            float t = i * 1f / (numPoints - 1);
            positions[i] = CalculateLinearBezierPoint(t, point0.position, point1.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateCubicBezierPoint1(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var u = (1f - t);
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }

    private Vector3 CalculateQuadraticBezierPoint1(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        var u = (1f - t);
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    private Vector3 CalculateGeneralBezier(float t, Vector3[] points)
    {
        if (points.Length == 2) return CalculateLinearBezierPoint(t, points[0], points[1]);
        return CalculateLinearBezierPoint(t, CalculateGeneralBezier(t, points.Slice(0, points.Length - 1)), CalculateGeneralBezier(t, points.Slice(1, points.Length)));
    }

    private Vector3 CalculateCubicBezierPoint2(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return CalculateLinearBezierPoint(t, CalculateQuadraticBezierPoint2(t, p0, p1, p2), CalculateQuadraticBezierPoint2(t, p1, p2, p3));
    }

    private Vector3 CalculateQuadraticBezierPoint2(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return CalculateLinearBezierPoint(t, CalculateLinearBezierPoint(t, p0, p1), CalculateLinearBezierPoint(t, p1, p2));
    }

    private Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }
}

public static class ArrayExtensions
{
    /// <summary>
    /// Get the array slice between the two indexes.
    /// ... Inclusive for start index, exclusive for end index.
    /// </summary>
    public static T[] Slice<T>(this T[] source, int start, int end)
    {
        // Handles negative ends.
        if (end < 0)
        {
            end = source.Length + end;
        }
        int len = end - start;

        // Return new array.
        T[] res = new T[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = source[i + start];
        }
        return res;
    }
}
