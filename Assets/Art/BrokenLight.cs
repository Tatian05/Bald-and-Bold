using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class BrokenLight : MonoBehaviour, IDamageable
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _brokenLightSprite;
    [SerializeField] ParticleSystem _brokenLightPT;

    #region NO BROKEN LIGHT VARIABLES

    [Header("BROKEN LIGHT VARIABLES")]

    [SerializeField] float _minTime = 0.07f;
    [SerializeField] float _maxTime = 0.3f;
    [SerializeField] GameObject _targetObject;
    [SerializeField] float _blinkCdTime = 2;
    [SerializeField] float _realCdTimeMultiplier = 2;

    float _realBlinkCdTime;
    float _currentBlinkCdTime;
    float _blinkTime;
    float _currentBlinkingTimer;
    float _blinkCount;

    #endregion

    [Space(20f)]

    [Header("BROKEN LIGHT VARIABLES")]
    [SerializeField] float _brokenBlinkTime;
    [SerializeField] Color Lightcolor;
    [SerializeField] bool _startBlinking;
    float _brokenBlinkTimer;

    enum LightStates { IDLE, BLINK, RETURN_BLINK, BROKEN }
    EventFSM<LightStates> _myFSM;
    LevelLightsManager _levelLightsManager;
    void Start()
    {
        _levelLightsManager = GetComponentInParent<LevelLightsManager>();
        var idle = new State<LightStates>("IDLE");
        var blink = new State<LightStates>("BLINK");
        var returnBlink = new State<LightStates>("RETURN_BLINK");
        var broken = new State<LightStates>("BROKEN");

        StateConfigurer.Create(idle).SetTransition(LightStates.BROKEN, broken).SetTransition(LightStates.BLINK, blink).Done();
        StateConfigurer.Create(blink).SetTransition(LightStates.BROKEN, broken).SetTransition(LightStates.RETURN_BLINK, returnBlink).Done();
        StateConfigurer.Create(returnBlink).SetTransition(LightStates.BROKEN, broken).SetTransition(LightStates.BLINK, blink).Done();
        StateConfigurer.Create(broken).Done();

        #region BLINK

        //blink.OnEnter += x => _targetObject.GetComponent<Light2D>().color = Color.white;
        blink.OnUpdate += CheckBlink;

        #endregion

        #region RETURN BLINK

        returnBlink.OnUpdate += ReturnBlink;

        #endregion

        #region BROKEN

        broken.OnEnter += x =>
        {
            GetComponent<Collider2D>().enabled = false;
            var light2D = _targetObject.GetComponent<Light2D>();
            _spriteRenderer.sprite = _brokenLightSprite;
            light2D.color = Lightcolor;
            light2D.blendStyleIndex = 2;
            light2D.pointLightInnerAngle = 30;
            light2D.pointLightOuterRadius = 3;
        };
        broken.OnUpdate += BrokenLightState;

        #endregion

        _myFSM = new EventFSM<LightStates>(_startBlinking ? blink : idle);
    }
    private void Update()
    {
        _myFSM?.Update();
    }

    void CheckBlink()
    {
        _currentBlinkingTimer += Time.deltaTime;
        if (_currentBlinkingTimer < _blinkTime) return;

        _targetObject.SetActive(!_targetObject.activeSelf);

        _blinkCount++;
        _currentBlinkingTimer = 0;
        _blinkTime = Random.Range(_minTime, _maxTime);

        if (_blinkCount == 2)
        {
            _blinkCount = 0;
            _realBlinkCdTime = Random.Range(_blinkCdTime / _realCdTimeMultiplier, _blinkCdTime * _realCdTimeMultiplier);
            _myFSM.SendInput(LightStates.RETURN_BLINK);
        }
    }

    void ReturnBlink()
    {
        _currentBlinkCdTime += Time.deltaTime;

        if (_currentBlinkCdTime < _realBlinkCdTime) return;

        _currentBlinkCdTime = 0;

        _myFSM.SendInput(LightStates.BLINK);
    }

    public void CanBlink()
    {
        _myFSM.SendInput(LightStates.BLINK);
    }

    public void CantBlink()
    {
        TurnOnLights();
        _currentBlinkingTimer = 0;
    }

    void TurnOnLights()
    {
        _targetObject.SetActive(true);
    }

    void BrokenLightState()
    {
        _brokenBlinkTimer += Time.deltaTime;

        if (_brokenBlinkTimer >= _brokenBlinkTime)
        {
            _targetObject.SetActive(!_targetObject.activeSelf);
            _brokenBlinkTimer = 0;
        }
    }
    public void TakeDamage(float dmg)
    {
        Die();
    }

    public void Die()
    {
        _levelLightsManager.RemoveBrokenLight(this, _targetObject.GetComponent<Light2D>());
        _brokenLightPT.Play();
        _myFSM.SendInput(LightStates.BROKEN);
    }
}
