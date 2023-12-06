using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaBallAnimationScrip : MonoBehaviour
{
    [SerializeField] private ParticleSystem _psA;
    [SerializeField] private ParticleSystem _psB;

    public void ActiveParticles()
    {
        _psA.Play();
        _psB.Play();
    }
}
