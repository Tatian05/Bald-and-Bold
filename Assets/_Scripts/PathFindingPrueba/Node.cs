using System.Collections.Generic;
using UnityEngine;
public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    public float cost;
    public Vector3 hitPoint { get; private set; }
    private void Start()
    {
        LayerMask borderMask = LayerMask.GetMask("Border");
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, -Vector3.up, .75f, borderMask);
        if (raycast) hitPoint = raycast.point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPoint, .25f);
    }
}
