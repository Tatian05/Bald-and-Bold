using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRateController : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float increaseRate = 10f; // Rate increase per second
    public float maxRate = 100f; // Maximum emission rate
    public float duration = 5f; // Duration of rate increase in seconds
    private float originalRate;
    private float timer;

    private void Start()
    {
        originalRate = particleSystem.emission.rateOverTime.constant; // Store the original emission rate
        ResetParticleRate(); // Reset emission rate at the start
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (particleSystem.isPlaying && timer <= duration)
        {
            float newRate = originalRate + increaseRate * timer;
            var emission = particleSystem.emission;
            emission.rateOverTime = Mathf.Min(newRate, maxRate);
        }
        else
        {
            ResetParticleRate();
        }
    }

    private void ResetParticleRate()
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = originalRate;
        particleSystem.Play();
        timer = 0f;
    }
}
