using UnityEngine;
using UnityEngine.UI;
namespace BaldAndBold.Consumables
{
    public abstract class Consumables : MonoBehaviour
    {
        [SerializeField] ConsumableData _consumableData;
        [SerializeField] Button _triggerButton;
        public Sprite GetSprite => _consumableData.shoppableData.shopSprite;
        public float GetDuration => _consumableData.consumableDuration;
        private void Awake()
        {
            _triggerButton.interactable = false;
        }
        protected virtual void Start()
        {
            GetComponent<Image>().sprite = _consumableData.shoppableData.shopSprite;
            _triggerButton.onClick.AddListener(TriggerConsumable);
        }
        public void TriggerConsumable()
        {
            ConsumableAction(true);
            EventManager.TriggerEvent(Contains.CONSUMABLE, this);
            Destroy(gameObject);
        }
        public void SetInteractableButton(bool interactable) { _triggerButton.interactable = interactable; }
        public abstract void ConsumableAction(bool activate);

    }
}
