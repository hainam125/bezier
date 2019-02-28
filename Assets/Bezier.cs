using UnityEngine;

public class Bezier
{
    public static Vector3 CalculateGeneralBezier(float t, Vector3[] points)
    {
        if (points.Length == 2) return CalculateLinearBezierPoint(t, points[0], points[1]);
        return CalculateLinearBezierPoint(t, CalculateGeneralBezier(t, points.Slice(0, points.Length - 1)), CalculateGeneralBezier(t, points.Slice(1, points.Length)));
    }

    public static Vector3 CalculateCubicBezierPoint2(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return CalculateLinearBezierPoint(t, CalculateQuadraticBezierPoint2(t, p0, p1, p2), CalculateQuadraticBezierPoint2(t, p1, p2, p3));
    }

    public static Vector3 CalculateQuadraticBezierPoint2(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return CalculateLinearBezierPoint(t, CalculateLinearBezierPoint(t, p0, p1), CalculateLinearBezierPoint(t, p1, p2));
    }

    public static Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        return p0 + t * (p1 - p0);
    }

    public static Vector3 CalculateQuadraticBezierWithDirection(float t, Vector3 p0, Vector3 p1, Vector3 p2, out Vector3 dir)
    {
        var pos1 = CalculateLinearBezierPoint(t, p0, p1);
        var pos2 = CalculateLinearBezierPoint(t, p1, p2);
        dir = pos2 - pos1;
        return CalculateLinearBezierPoint(t, pos1, pos2);
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
