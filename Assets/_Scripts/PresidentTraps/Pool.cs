using System.Collections;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] ParticleSystem _waterPs;

    private void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(StopWater());
        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, PlayWater);
    }
    void PlayWater(params object[] param) { _waterPs.Play(); }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, PlayWater);        
    }
    IEnumerator StopWater()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        _waterPs.Stop();
        yield return wait;
        _waterPs.Play();
    }
}
