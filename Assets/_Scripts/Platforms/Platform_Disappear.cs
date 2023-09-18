using System.Collections;
using System;
using UnityEngine;

public class Platform_Disappear : MonoBehaviour
{
    public Action OnUpdate;

    [SerializeField] float _timeToDisappear;
    [SerializeField] float _timeToAppear;

    float _currentTimeToAppear;
    float _currentTimeToDisappear;

    [SerializeField] GameObject _sprite;
    [SerializeField] ParticleSystem _dustPS;

    Collider2D[] _colliders;
    Animator _anim;

    bool _isDisappearing = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider2D>();

        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, AppearPlatform);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, AppearPlatform);       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !_isDisappearing)
        {
            OnUpdate += CheckDisappear;
            _isDisappearing = true;
            _anim.Play("Breaking");
        }
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    void CheckAppear()
    {
        _currentTimeToAppear += Time.deltaTime;

        if (_currentTimeToAppear < _timeToAppear) return;

        AppearPlatform();
    }
    public void PlayDust()
    {
        _dustPS.Play();
    }
    void CheckDisappear()
    {
        _currentTimeToDisappear += Time.deltaTime;

        if (_currentTimeToDisappear < _timeToDisappear) return;

        DisappearPlatform();
        _currentTimeToDisappear = 0;
        OnUpdate += CheckAppear;
        OnUpdate -= CheckDisappear;
    }

    void AppearPlatform(params object[] param)
    {
        OnUpdate = null;
        _currentTimeToAppear = 0;
        _currentTimeToDisappear = 0;

        _sprite.SetActive(true);

        foreach (var item in _colliders)
        {
            item.enabled = true;
        }
        _anim.Play("Idle");
        _isDisappearing = false;
    }

    void DisappearPlatform()
    {
        _sprite.SetActive(false);

        foreach (var item in _colliders)
        {
            item.enabled = false;
        }
    }
}
