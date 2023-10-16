using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public static class Extensions
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = Mathf.Abs(1 - (dir.magnitude / explosionRadius));
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    public static T Next<T>(this IEnumerable<T> source, T current)
    {
        T next = source.SkipWhile(x => !x.Equals(current)).Skip(1).FirstOrDefault();
        return next != null ? next : source.FirstOrDefault();
    }
}
