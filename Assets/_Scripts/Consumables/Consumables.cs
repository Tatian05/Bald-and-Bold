using UnityEngine;
using UnityEngine.UI;
namespace BaldAndBold.Consumables
{
    public abstract class Consumables : MonoBehaviour
    {
        [SerializeField] ConsumableData _consumableData;
        [SerializeField] Button _triggerButton;
        protected PersistantData _persistantData;
        public ConsumableData ConsumableData { get { return _consumableData; } }
        private void Awake()
        {
            _triggerButton.interactable = false;
        }
        protected virtual void Start()
        {
            _triggerButton.onClick.AddListener(TriggerConsumable);
            _persistantData = Helpers.PersistantData;
        }
        public void TriggerConsumable()
        {
            ConsumableAction(true);
            EventManager.TriggerEvent(Contains.CONSUMABLE, this, _consumableData.consumableDuration);
            Helpers.PersistantData.RemoveShoppableToCollection(_consumableData);
            Destroy(gameObject);
        }
        public void SetInteractableButton(bool interactable) { _triggerButton.interactable = interactable; }
        public virtual void ConsumableAction(bool activate)
        {
            EventManager.TriggerEvent(Contains.CONSUMABLE_VISUAL_EFFECTS, activate);
        }
        public Consumables SetParent(Transform parent)
        {
            transform.SetParent(parent);
            return this;
        }
    }
}
