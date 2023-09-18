using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 50f; // Adjust this value to control the speed of rotation
    public float rotationAngle = 145f; // Adjust this value to control the angle of rotation
    public float direction = -1; // The direction of rotation. 1 for clockwise, -1 for counterclockwise.
    public Transform spotlightTransform; // Reference to the transform of the 2D spotlight attached to the object

    void Update()
    {
        // Calculate the new rotation based on the current rotation and rotation speed
        float newRotation = Time.time * rotationSpeed * direction;

        // Flip the direction when the rotation reaches the limits
        if (newRotation > rotationAngle || newRotation < -rotationAngle)
        {
            direction = -direction;
        }

        // Apply the new rotation to the object, flipping the effect vertically
        transform.rotation = Quaternion.Euler(0, -newRotation, 0);

        // Rotate the spotlight to point downwards
        spotlightTransform.right = Quaternion.Euler(0, 0, -newRotation) * -transform.up;
    }
}