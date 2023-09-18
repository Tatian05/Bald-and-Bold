using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{  
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float baseAmplitude = 0.1f;
    [SerializeField] private float speedVariation = 0.1f;
    [SerializeField] private float amplitudeVariation = 0.05f;

    private float startY;
    private float currentSpeed;
    private float currentAmplitude;

    private void Start()
    {
        startY = transform.localPosition.y;
        currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
        currentAmplitude = baseAmplitude + Random.Range(-amplitudeVariation, amplitudeVariation);
    }

    private void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * currentSpeed) * currentAmplitude;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        if (Time.frameCount % 60 == 0) // update speed and amplitude every 60 frames
        {
            currentSpeed = baseSpeed + Random.Range(-speedVariation, speedVariation);
            currentAmplitude = baseAmplitude + Random.Range(-amplitudeVariation, amplitudeVariation);
        }
    }
}