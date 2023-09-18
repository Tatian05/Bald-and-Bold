using UnityEngine;
namespace Droppables
{
    public abstract class Droppable : MonoBehaviour, IBreakable, IDamageable
    {
        [SerializeField] protected GameObject _destroyedVersion;
        [SerializeField] protected Vector3 _offset;
        protected Transform _dropPosition;

        [Range(0, 1)]
        protected float _probabilityDrop;

        protected KeysUI _keyUI;
        public void Break()
        {
            if (_destroyedVersion != null)
                Instantiate(_destroyedVersion, transform.position, Quaternion.identity);

            DropObject();
            Destroy(gameObject);
        }
        protected abstract void DropObject();
        public void TakeDamage(float dmg)
        {
            Die();
        }

        public void Die()
        {
            Break();
        }
    }
}
