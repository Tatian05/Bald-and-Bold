using System.Collections;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    [SerializeField] ParticleSystem _freezerPS;
    [SerializeField] GameObject _president;
    [SerializeField] SpriteRenderer[] _presidentSprites;
    [SerializeField] SpriteRenderer _ice;
    [SerializeField] Color _targetColor;
    Color _iceStartColor, _onColorChange, _onIceColorChange;

    Animator _presidentAnimator;
    float _animSpeed, _levelMaxTime, _wonTimer;
    System.Action _freezerAction;
    private void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(StopFreezer());
        EventManager.SubscribeToEvent(Contains.ON_LEVEL_START, PlayFreezer);

        _presidentAnimator = _president.GetComponent<Animator>();
        _levelMaxTime = Helpers.LevelTimerManager.LevelMaxTime;

        RuntimeAnimatorController ac = _presidentAnimator.runtimeAnimatorController;
        foreach (var anim in ac.animationClips)
            if (anim.name == "I_Parado 2") _animSpeed = anim.length;

        _iceStartColor = _ice.color;

        _freezerAction = FreezerOn;
    }
    void PlayFreezer(params object[] param) => _freezerPS.Play();
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_LEVEL_START, PlayFreezer);
    }
    void Update()
    {
        _freezerAction();
    }
    IEnumerator StopFreezer()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        _freezerPS.Stop();
        yield return wait;
        _freezerPS.Play();
    }

    void FreezerOn()
    {
        _presidentAnimator.SetFloat("Speed", Mathf.Lerp(_animSpeed, 0, Helpers.LevelTimerManager.Timer / _levelMaxTime));

        for (int i = 0; i < _presidentSprites.Length; i++)
            _presidentSprites[i].color = Color.Lerp(Color.white, _targetColor, Helpers.LevelTimerManager.Timer / _levelMaxTime);


        _ice.color = new Color(_iceStartColor.r, _iceStartColor.g, _iceStartColor.b, Mathf.Lerp(0, 1, Helpers.LevelTimerManager.Timer / _levelMaxTime));
    }

    void Won()
    {
        _wonTimer += Time.deltaTime;
        for (int i = 0; i < _presidentSprites.Length; i++)
            _presidentSprites[i].color = Color.Lerp(_onColorChange, Color.white, _wonTimer / 1);

        _ice.color = new Color(_onIceColorChange.r, _onIceColorChange.g, _onIceColorChange.b, Mathf.Lerp(_onIceColorChange.a, 0, _wonTimer / 1));
    }

    public void PlayerWon()
    {
        _onColorChange = _presidentSprites[0].color;
        _onIceColorChange = _ice.color;
        _freezerAction = Won;
    }
}
