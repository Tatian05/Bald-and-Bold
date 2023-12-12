using UnityEngine;
public class PlayerView
{
    Transform _transform;
    Animator _anim;
    ParticleSystem _dashParticle;
    SpriteRenderer[] _playerSprites;
    BoxCollider2D _bootsCollider;

    string[] _stepsSounds = new string[2] { "Footstep1", "Footstep2" };
    float _stepsTimer;
    int _stepsIndex;
    GameManager _gameManager;
    PersistantData _persistantData;
    bool _invisible;
    public PlayerView(Transform transform, Animator anim, ParticleSystem dashParticle, SpriteRenderer[] playerSprites, BoxCollider2D bootsCollider)
    {
        _transform = transform;
        _anim = anim;
        _dashParticle = dashParticle;
        _playerSprites = playerSprites;
        _bootsCollider = bootsCollider;

        _gameManager = Helpers.GameManager;
        _persistantData = Helpers.PersistantData;
        _bootsCollider.enabled = false;
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_INVISIBLE, InvisibleConsumable);
        EventManager.SubscribeToEvent(Contains.CONSUMABLE_BOOTS, BootConsumable);

        _invisible = _persistantData.consumablesData.invisible;
        if (_invisible) EventManager.TriggerEvent(Contains.CONSUMABLE_INVISIBLE, true);//InvisibleConsumable(true);
    }
    public void Run(float xAxis, bool inGround, float yAxis = 0)
    {
        _anim.SetInteger("xAxis", Mathf.Abs(Mathf.RoundToInt(xAxis)));
        _stepsTimer += Time.deltaTime;

        if (_stepsTimer >= .1f && xAxis != 0 && !inGround)
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
        EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_BOOTS, BootConsumable);
    }
    void InvisibleConsumable(params object[] param)
    {
        _persistantData.consumablesData.invisible = _invisible = (bool)param[0];
        foreach (var item in _playerSprites)
        {
            if ((bool)param[0]) item.ChangeAlpha(.5f);
            else item.color = Color.white;
        }
    }
    void BootConsumable(params object[] param)
    {
        _bootsCollider.enabled = (bool)param[0];
        _playerSprites[2].transform.GetChild(0).gameObject.SetActive((bool)param[0]);
        _playerSprites[3].transform.GetChild(0).gameObject.SetActive((bool)param[0]);
    }
}

