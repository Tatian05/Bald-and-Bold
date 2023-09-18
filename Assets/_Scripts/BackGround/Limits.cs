using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MovementDecorations>())
            Destroy(collision.gameObject);
    }
}
