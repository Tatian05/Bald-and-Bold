using System.Collections;
using UnityEngine;
public class Cohete : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PlayCoheteCountDown());
    }
    IEnumerator PlayCoheteCountDown()
    {
        yield return new WaitUntil(() => (Helpers.LevelTimerManager.LevelMaxTime - Helpers.LevelTimerManager.Timer) <= 10);
        Helpers.AudioManager.PlaySFX("CoheteCountDown");
    }
}
