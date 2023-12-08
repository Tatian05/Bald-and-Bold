using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaldAndBold.Consumables;
public class ConsumablesUI : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] TextMeshProUGUI _countdownTxt;

    float _countDown;
    float _minutes, _seconds;
    Consumables _consumable;

    public ConsumableData ConsumableData { get { return _consumable.ConsumableData; } }
    public float Time { get { return _countDown; } }
    private void Update()
    {
        _countDown -= CustomTime.DeltaTime;
        _minutes = (int)(_countDown / 60f);
        _seconds = (int)(_countDown - (int)(_countDown / 60f) * 60f);
        _countdownTxt.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);

        if (_countDown <= 0)
        {
            _consumable.ConsumableAction(false);
            Helpers.PersistantData.RemoveConsumableActivated(_consumable.ConsumableData);
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        if (_countDown > 0) Helpers.PersistantData.SaveConsumableActivated(_consumable.ConsumableData, _countDown);
    }

    #region BUILDER
    public ConsumablesUI SetConsumable(Consumables consumable)
    {
        _consumable = consumable;
        _img.sprite = _consumable.ConsumableData.shopSprite;
        return this;
    }
    public ConsumablesUI SetTime(float time)
    {
        _countDown = time;
        return this;
    }
    public ConsumablesUI SetParent(Transform parent)
    {
        transform.SetParent(parent, true);
        return this;
    }

    #endregion
}
