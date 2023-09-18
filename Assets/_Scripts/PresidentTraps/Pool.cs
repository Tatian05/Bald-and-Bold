using System.Collections;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterPs;

    private void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(StopWater());
        Helpers.LevelTimerManager.OnLevelStart += () => _waterPs.Play();
    }
    IEnumerator StopWater()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        _waterPs.Stop();
        yield return wait;
        _waterPs.Play();
    }
}
