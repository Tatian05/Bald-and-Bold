using System;
using UnityEngine;
using System.Collections;
public class Platform_Disappear : MonoBehaviour
{
    public Action OnUpdate;

    [SerializeField] float _timeToAppear;
    [SerializeField, Range(0, 1)] float _addCracksMultiplier;
    [SerializeField] AnimationClip _defaultAnimationClip;
    [SerializeField] GameObject _sprite;
    [SerializeField] ParticleSystem _dustPS;
    [SerializeField] BoxCollider2D _collision, _trigger;

    float _disappeatTimer, _startAppearTimer;
    float _disappearAnimDuration;
    Animator _anim;
    bool _shaking;
    ParticleSystem.MainModule _psModule;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _disappearAnimDuration = _defaultAnimationClip.length;
        _psModule = _dustPS.main;

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, AppearPlatform);
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, AppearPlatform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnUpdate = AddCracks;
            if (!_shaking) StartCoroutine(Shake());
            _anim.SetFloat("Disappear", 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_disappeatTimer < _disappearAnimDuration) OnUpdate = LessCracks;
            _anim.SetFloat("Disappear", -1 * _addCracksMultiplier);
        }
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    void LessCracks()
    {
        if (_disappeatTimer <= 0)
            return;

        _anim.SetBool("Idle", _disappeatTimer <= 0);
        _psModule.maxParticles = (int)Mathf.Lerp(0, 30, _disappeatTimer / _disappearAnimDuration);
        _disappeatTimer -= Time.deltaTime * _addCracksMultiplier;
    }
    void AddCracks()
    {
        if (_disappeatTimer > _disappearAnimDuration)
        {
            _psModule.maxParticles = 0;
            OnUpdate = null;
            return;
        }
        _anim.SetBool("Idle", _disappeatTimer <= 0);
        _psModule.maxParticles = (int)Mathf.Lerp(0, 30, _disappeatTimer / _disappearAnimDuration);
        _disappeatTimer += Time.deltaTime;
    }

    void AppearPlatform(params object[] param)
    {
        _sprite.SetActive(true);
        _psModule.maxParticles = 0;
        _disappeatTimer = 0;
        _startAppearTimer = 0;
        OnUpdate = null;

        _collision.enabled = true;
        _trigger.enabled = true;
    }
    void StartAppearPlatform()
    {
        _startAppearTimer += Time.deltaTime;
        if (_startAppearTimer >= _timeToAppear)
        {
            AppearPlatform();
            _anim.Play("Repair");
        }
    }

    //LOS LLAMO POR ANIMACION
    public void DisappearPlatform()
    {
        OnUpdate = StartAppearPlatform;
        _sprite.SetActive(false);
    }
    public void DisableColliders()
    {
        _collision.enabled = false;
        _trigger.enabled = false;
    }
    IEnumerator Shake()
    {
        yield return null;
        Vector3 startPosition = transform.position;
        _shaking = true;

        while (_disappeatTimer < _disappearAnimDuration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * _disappeatTimer * .05f;
            float y = UnityEngine.Random.Range(-1f, 1f) * _disappeatTimer * .05f;

            transform.position = new Vector3(startPosition.x + x, startPosition.y + y, startPosition.z);

            if (_disappeatTimer <= 0f) break;
            yield return null;
        }

        transform.position = startPosition;
        _shaking = false;
    }
}
