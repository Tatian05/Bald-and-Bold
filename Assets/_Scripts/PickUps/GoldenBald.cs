using UnityEngine;
using DG.Tweening;
public class GoldenBald : MonoBehaviour
{
    [SerializeField] int _coinsAward;
    Tween _tween;
    Vector3 _initialPos;
    bool _playerHasIt;
    private void Start()
    {
        _initialPos = transform.position;
        _tween = transform.DOMoveY(transform.position.y + 1, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, BoldenBaldAction);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, BoldenBaldAction);
    }
    public void BoldenBaldAction(params object[] param)
    {
        if (!_playerHasIt) return;

        Helpers.PersistantData.persistantDataSaved.AddGoldenBaldCoins(_coinsAward);
        EventManager.TriggerEvent(Contains.MISSION_PROGRESS, "Golden Balds", 1);
    }
    public GoldenBald SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }
    public GoldenBald SetOwner(bool player)
    {
        _playerHasIt = player;
        if (player) _tween.Pause();
        else _tween.Restart();
        return this;
    }
    public GoldenBald ResetGoldenBald()
    {
        SetPosition(_initialPos);
        SetOwner(false);
        return this;
    }
}
