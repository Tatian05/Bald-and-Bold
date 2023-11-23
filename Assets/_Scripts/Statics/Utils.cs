using UnityEngine;
public static class Utils
{
    public static float MultiLerp(float time, int[] points)
    {
        if (points.Length == 1)
            return points[0];
        else if (points.Length == 2)
            return Mathf.Lerp(points[0], points[1], time);

        if (time == 0)
            return points[0];

        if (time == 1)
            return points[points.Length - 1];

        float t = time * (points.Length - 1);

        float pointA = 0;
        float pointB = 0;

        for (int i = 0; i < points.Length; i++)
        {
            if (t < i)
            {
                pointA = points[i - 1];
                pointB = points[i];
                return Mathf.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
                return points[i];
        }

        return 0;
    }   
}
