using System.Collections;
using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponData _weaponData;
        [SerializeField] protected bool _droppableWeapon, _animated;
        [SerializeField] protected Vector2 _equipedWeaponOffset = Vector2.zero;

        protected float _weaponTimer;
        protected Rigidbody2D _rb;
        protected Animator _animator;
        protected WeaponManager _weaponManager;
        protected SpriteRenderer _spriteRenderer;
        protected GameManager _gameManager;
        protected Sprite _equipedBulletx16, _equipedBulletx32;
        public WeaponData GetWeaponData { get { return _weaponData; } }
        public bool CanPickUp => Mathf.Abs(_rb.velocity.magnitude) < .1f;
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_droppableWeapon) _animator = GetComponent<Animator>();
            _equipedBulletx16 = Helpers.PersistantData.persistantDataSaved.bulletEquiped.shopSprite();
            _equipedBulletx32 = Helpers.PersistantData.persistantDataSaved.bulletEquiped.bulletX32();
        }
        private void OnEnable()
        {
            StartCoroutine(FindGameManager());
        }
        protected virtual void Start()
        {
            _weaponManager = FindObjectOfType<WeaponManager>();
            _spriteRenderer.sprite = _weaponData.mainSprite;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Vector2.down * 1f);
        }
        public void Attack()
        {
            if (Time.time >= _weaponTimer)
            {
                _weaponTimer = Time.time + 1 / _weaponData.fireRate;
                WeaponAction();
            }
        }
        public void Attack(float bulletScale, float cadenceBoost, bool recoil)
        {
            if (Time.time >= _weaponTimer)
            {
                _weaponTimer = Time.time + 1 / (_weaponData.fireRate * cadenceBoost);
                WeaponAction(bulletScale, cadenceBoost, recoil);
            }
        }
        public abstract void WeaponAction(float bulletScale, float cadenceBoost, bool recoil);
        public abstract void WeaponAction();
        IEnumerator FindGameManager()
        {
            while (Helpers.GameManager == null) yield return null;

            _gameManager = Helpers.GameManager;
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
        public Weapon SetOffset()
        {
            transform.localPosition = _equipedWeaponOffset;
            return this;
        }
        public Weapon PickUp(bool knife = false)
        {
            _rb.simulated = knife;
            _rb.velocity = Vector2.zero;
            transform.eulerAngles = Vector3.zero;
            if (_animator && !_animated) _animator.enabled = false;
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
