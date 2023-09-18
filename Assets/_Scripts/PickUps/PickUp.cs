using UnityEngine;
namespace PickUps
{
    public abstract class PickUp : MonoBehaviour
    {
        [SerializeField] protected string _uiText;

        Transform _UISignPosition;
        PickUpUI _pickUpUI;
        private void Awake()
        {
            _pickUpUI = FindObjectOfType<PickUpUI>();
        }
        protected virtual void Start()
        {
            _UISignPosition = transform.GetChild(0);
        }
        protected void OnMouseEnter()
        {
            _pickUpUI.SetActive(true).SetPosition(_UISignPosition.position).SetCanvas(_uiText);
        }
        protected void OnMouseExit()
        {
            _pickUpUI.SetActive(false);
        }
        public virtual void PickUpAction()
        {
            FRY_PickUps.Instance.ReturnObject(this);
            _pickUpUI.SetActive(false);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player) PickUpAction();
        }

        #region BUILDER
        public PickUp SetPosition(Vector2 position)
        {
            transform.position = position;
            return this;
        }

        #endregion

        public static void TurnOn(PickUp p)
        {
            p.gameObject.SetActive(true);
        }
        public static void TurnOff(PickUp p)
        {
            p.gameObject.SetActive(false);
        }
    }
}
