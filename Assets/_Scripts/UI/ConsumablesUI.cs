using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BaldAndBold.Consumables;
public class ConsumablesUI : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] TextMeshProUGUI _countdownTxt;

    float _countDownTimer;
    float _minutes, _seconds;
    Consumables _consumable;
    private void Update()
    {
        _countDownTimer -= CustomTime.DeltaTime;
        _minutes = (int)(_countDownTimer / 60f);
        _seconds = (int)(_countDownTimer - (int)(_countDownTimer / 60f) * 60f);
        _countdownTxt.text = string.Format("{0:00}:{1:00}", _minutes, _seconds);

        if (_countDownTimer <= 0)
        {
            _consumable.ConsumableAction(false);
            Destroy(gameObject);
        }
    }
    public ConsumablesUI SetConsumable(Consumables consumable)
    {
        _consumable = consumable;
        _countDownTimer = consumable.GetDuration;
        _img.sprite = consumable.GetSprite;
        return this;
    }
    public ConsumablesUI SetParent(Transform parent)
    {
        transform.SetParent(parent, true);
        return this;
    }
}
