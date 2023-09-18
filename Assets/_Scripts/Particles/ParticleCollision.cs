using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    ParticleSystem particle;
    public GameObject splatPrefab;
    public Transform SplatHolder;
    private List<ParticleCollisionEvent> CollisionEvents = new List<ParticleCollisionEvent>();


    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, CollisionEvents);

        int count = CollisionEvents.Count;
        for (int i = 0; i < count; i++)
        {
            Instantiate(splatPrefab, CollisionEvents[i].intersection, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), SplatHolder);
        }
    }

}
