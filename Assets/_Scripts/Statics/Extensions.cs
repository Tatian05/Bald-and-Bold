using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    public static Dictionary<K, V> DictioraryFromTwoLists<K, V>(this IEnumerable<K> keys, IEnumerable<V> values)
    {
        return keys.Zip(values, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
    }

    public static void ChangeAlpha(this SpriteRenderer sprite, float newAlpha)
    {
        var color = sprite.color;
        color.a = newAlpha;
        sprite.color = color;
    }
    public static T GetComponentInChildren<T>(this GameObject obj, bool includeInactive = false, bool excludeParent = false) where T : Component
    {
        var components = obj.GetComponentsInChildren<T>(includeInactive);

        if (!excludeParent)
            return components.FirstOrDefault();

        return components.FirstOrDefault(childComponent =>
            childComponent.transform != obj.transform);
    }
    public static T[] AddToArray<T>(this T[] array, T element)
    {
        return (FList.Cast(array) + element).ToArray();
    }
    public static T[] RemoveToArray<T>(this T[] array, T element)
    {
        return array.Where(x => !x.Equals(element)).ToArray();
    }
    public static T[] RemoveToArrayByIndex<T>(this T[] array, int index)
    {
        return array.Where((item, i) => i != index).ToArray();
    }
}
