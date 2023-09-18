using UnityEngine;
public class FlameThrower : MonoBehaviour
{
    [SerializeField] ParticleSystem _ps;
    [SerializeField] float _psMaxSpeed;
    [SerializeField] int _psMaxEmission;
    ParticleSystem.MainModule _mainModule;
    private void Start()
    {
        _mainModule = _ps.main;
    }
    void Update()
    {
        _mainModule.startSpeed = Mathf.Lerp(0, _psMaxSpeed, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
        _mainModule.maxParticles = (int)Mathf.Lerp(0, _psMaxEmission, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
