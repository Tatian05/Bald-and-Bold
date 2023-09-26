using UnityEngine;
public class Movement_RotateObject : IMovement
{
    Transform _armPivot, _weaponSprite, _sprite, _playerCenterTransform;

    public Movement_RotateObject(Transform armPivot, Transform playerCenterTransform, Transform weaponSprite = null, Transform sprite = null)
    {
        _armPivot = armPivot;
        _playerCenterTransform = playerCenterTransform;
        _weaponSprite = weaponSprite;
        _sprite = sprite;
    }
    public void Move()
    {
        Vector3 dirToLookAt = (_playerCenterTransform.position - _armPivot.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _armPivot.eulerAngles = new Vector3(0, 0, angle);

        Vector3 newWeaponLocalScale = Vector3.one;
        Vector3 newScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            newScale.x = -1;
            newWeaponLocalScale.y = -1;

        }
        else
        {
            newScale.x = 1;
            newWeaponLocalScale.x = 1;
        }

        if (_weaponSprite) _weaponSprite.localScale = newWeaponLocalScale;
        if (_sprite) _sprite.localScale = newScale;
    }
}
