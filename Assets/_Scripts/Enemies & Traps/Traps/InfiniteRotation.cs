using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // speed of rotation in degrees per second

    private void Update()
    {
        // get the current rotation of the sprite relative to its parent
        float currentRotation = transform.localRotation.eulerAngles.z;

        // calculate the new rotation of the sprite
        float newRotation = currentRotation + (rotationSpeed * Time.deltaTime);

        // set the new rotation of the sprite relative to its parent
        transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
    }
}