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
        protected SpriteRenderer _spriteRenderer;
        protected GameManager _gameManager;
        protected Sprite _equipedBullet, _equipedGrenade;
        public WeaponData GetWeaponData { get { return _weaponData; } }
        public bool CanPickUp => Mathf.Abs(_rb.velocity.magnitude) < .1f;
        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_droppableWeapon) _animator = GetComponent<Animator>();
            _equipedBullet = Helpers.PersistantData.bulletEquiped.shopSprite;
            _equipedGrenade = Helpers.PersistantData.grenadeEquiped.shopSprite;
        }
        private void OnEnable()
        {
            StartCoroutine(FindGameManager());
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
        public virtual Weapon PickUp(Vector3 rotation, bool knife = false)
        {
            _rb.simulated = knife;
            _rb.velocity = Vector2.zero;
            transform.eulerAngles = rotation;
            return this;
        }

        #endregion
    }
}
