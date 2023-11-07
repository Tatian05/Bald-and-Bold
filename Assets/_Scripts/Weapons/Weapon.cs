using System.Collections;
using UnityEngine;
using System.Linq;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected bool _droppableWeapon;

        protected float _weaponTimer;
        protected Rigidbody2D _rb;
        protected Animator _animator;
        protected WeaponManager _weaponManager;
        protected SpriteRenderer _spriteRenderer;
        protected GameManager _gameManager;
        public WeaponData GetWeaponData { get { return _weaponData; } }
        public bool CanPickUp => Mathf.Abs(_rb.velocity.magnitude) < .1f;
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (_droppableWeapon) _animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            StartCoroutine(FindGameManager());
        }
        protected virtual void Start()
        {
            _weaponManager = FindObjectOfType<WeaponManager>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _weaponData.mainSprite;
            EventManager.SubscribeToEvent(Contains.CONSUMABLE_CADENCE, CadenceConsumable);
        }

        private void OnDestroy()
        {
            EventManager.UnSubscribeToEvent(Contains.CONSUMABLE_CADENCE, CadenceConsumable);
        }

        //private void OnMouseEnter()
        //{
        //    if (!CanPickUp) return;
        //    _spriteRenderer.sprite = _weaponData.selectedSprite;
        //    transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        //}
        //private void OnMouseExit()
        //{
        //    if (!CanPickUp) return;
        //    _spriteRenderer.sprite = _weaponData.mainSprite;
        //    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //}
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * 1f);
        }
        public void Attack()
        {
            if (Time.time >= _weaponTimer)
            {
                _weaponTimer = Time.time + 1 / _weaponData.currentCadence;
                WeaponAction();
            }
        }

        public abstract void WeaponAction();
        IEnumerator FindGameManager()
        {
            while (GameManager.instance == null) yield return null;

            _gameManager = GameManager.instance;
        }

        void CadenceConsumable(params object[] param)
        {
            _weaponData.currentCadence = (bool)param[0] ? _weaponData.fireRate * (float)param[1] : _weaponData.fireRate;
        }

        #region BUILDER

        public Weapon SetParent(Transform parent)
        {
            transform.SetParent(parent);
            return this;
        }

        public Weapon SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public Weapon PickUp(bool knife = false)
        {
            _rb.simulated = knife;
            _rb.velocity = Vector2.zero;
            transform.eulerAngles = Vector3.zero;
            if (_animator) _animator.enabled = false;
            if (_droppableWeapon) _spriteRenderer.sprite = _weaponData.handWeapon;
            return this;
        }

        public Weapon ThrowOut(Vector2 direction)
        {
            _rb.simulated = true;
            _rb.AddForce(direction * 3, ForceMode2D.Impulse);
            if (_animator) _animator.enabled = true;
            _spriteRenderer.sprite = _weaponData.mainSprite;
            return this;
        }

        #endregion
    }
}
