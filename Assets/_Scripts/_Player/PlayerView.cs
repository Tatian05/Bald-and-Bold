using UnityEngine;
public class PlayerView
{
    Transform _transform;
    Animator _anim;
    ParticleSystem _dashParticle;
    SpriteRenderer[] _playerSprites;

    string[] _stepsSounds = new string[2] { "Footstep1", "Footstep2" };
    float _stepsTimer;
    int _stepsIndex;
    GameManager _gameManager;
    public PlayerView(Transform transform, Animator anim, ParticleSystem dashParticle, SpriteRenderer[] playerSprites)
    {
        _transform = transform;
        _anim = anim;
        _dashParticle = dashParticle;
        _playerSprites = playerSprites;

        _gameManager = Helpers.GameManager;
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, InvisibleConsumable);
    }
    public void Run(float xAxis, float yAxis = 0)
    {
        _anim.SetInteger("xAxis", Mathf.Abs(Mathf.RoundToInt(xAxis)));
        _stepsTimer += Time.deltaTime;

        if (_stepsTimer >= .1f && xAxis != 0)
        {
            Helpers.AudioManager.PlaySFX(_stepsSounds[_stepsIndex++ % _stepsSounds.Length]);
            _stepsTimer = 0;
        }
    }

    public void Jump()
    {
        Helpers.AudioManager.PlaySFX("Player_Jump");
    }

    public void Dash(float xAxis)
    {
        _dashParticle.Play();
        _dashParticle.transform.localScale = new Vector3(.7f * Mathf.Sign(xAxis), 1, 1);
        Helpers.AudioManager.PlaySFX("Player_Dash");
    }
    public void OnDeath()
    {
        _gameManager.EffectsManager.PlayerKilled(_transform.position + Vector3.up);
        Helpers.AudioManager.PlaySFX("PlayerDeath");
    }

    public void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, InvisibleConsumable);
    }
    void InvisibleConsumable(params object[] param)
    {
        foreach (var item in _playerSprites)
        {
            if ((bool)param[0]) item.ChangeAlpha(.5f);
            else item.color = Color.white;
        }
    }
}

