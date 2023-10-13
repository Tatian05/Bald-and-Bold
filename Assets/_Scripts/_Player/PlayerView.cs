using UnityEngine;
public class PlayerView
{
    [SerializeField] Animator _anim;
    [SerializeField] ParticleSystem _dashParticle;

    string[] _stepsSounds = new string[2] { "Footstep1", "Footstep2" };
    float _stepsTimer;
    int _stepsIndex;
    public PlayerView(Animator anim, ParticleSystem dashParticle)
    {
        _anim = anim;
        _dashParticle = dashParticle;
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
}

