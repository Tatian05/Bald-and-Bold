using UnityEngine;
using DG.Tweening;
public class GoldenBald : MonoBehaviour
{
    [SerializeField] int _coinsAward;
    Tween _tween;
    private void Start()
    {
        _tween = transform.DOMoveY(transform.position.y + 1, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(Contains.PLAYER_DEAD, RestartTween);
        EventManager.SubscribeToEvent(Contains.ON_ROOM_WON, BoldenBaldAction);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_DEAD, RestartTween);     
        EventManager.UnSubscribeToEvent(Contains.ON_ROOM_WON, BoldenBaldAction);     
    }
    void RestartTween(params object[] param) { _tween.Play(); }
    public void BoldenBaldAction(params object[] param) { Helpers.PersistantData.persistantDataSaved.coins += _coinsAward; }
    public GoldenBald SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }
    public GoldenBald KillTween()
    {
        _tween.Kill();
        return this;
    }
}
