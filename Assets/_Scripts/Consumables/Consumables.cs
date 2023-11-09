using UnityEngine;
using UnityEngine.InputSystem;

namespace BaldAndBold.Consumables
{
    public abstract class Consumables : MonoBehaviour
    {
        [SerializeField] protected float _consumableDuration;
        protected System.Action _countDown;

        protected float _countdownTimer;
        PlayerInputs _playerInputs;
        InputAction _triggerConsumableAction;
        protected virtual void Start()
        {
            _countdownTimer = _consumableDuration;
            _playerInputs = NewInputManager.PlayerInputs;
            _triggerConsumableAction = _playerInputs.Player.TriggerConsumable;
            _triggerConsumableAction.performed += TriggerConsumable;
            _triggerConsumableAction.Enable();
        }
        protected virtual void Update()
        {
            _countDown?.Invoke();
        }
        protected void TriggerConsumable(InputAction.CallbackContext obj)
        {
            ConsumableAction(true);
            _countDown = CountDown;
        }
        protected abstract void ConsumableAction(bool activate);

        void CountDown()
        {
            _countdownTimer -= CustomTime.DeltaTime;
            if (_countdownTimer <= 0)
            {
                ConsumableAction(false);
                Destroy(gameObject);
            }
        }
        private void OnDestroy()
        {
            _countDown = delegate { };
            _triggerConsumableAction.performed -= TriggerConsumable;
            _triggerConsumableAction.Disable();
        }
    }
}
