using UnityEngine;
public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = Mathf.Abs(1 - (dir.magnitude / explosionRadius));
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }
    public static Vector3 MultiLerp(float time, Vector3[] points)
    {
        if (points.Length == 1)
            return points[0];
        else if (points.Length == 2)
            return Vector3.Lerp(points[0], points[1], time);

        if (time == 0)
            return points[0];

        if (time == 1)
            return points[points.Length - 1];

        float t = time * (points.Length - 1);

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            if (t < i)
            {
                pointA = points[i - 1];
                pointB = points[i];
                return Vector3.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
            {
                return points[i];
            }
        }
        return Vector3.zero;
    }
}
