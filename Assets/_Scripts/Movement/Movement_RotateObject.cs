using UnityEngine;
public class Movement_RotateObject : IMovement
{
    Transform _armPivot, _weaponSprite, _playerPosition;
    public Movement_RotateObject(Transform armPivot, Transform playerPosition, Transform weaponSprite = null)
    {
        _armPivot = armPivot;
        _playerPosition = playerPosition;
        _weaponSprite = weaponSprite;
    }
    public void Move()
    {
        Vector3 dirToLookAt = (_playerPosition.position - _armPivot.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _armPivot.eulerAngles = new Vector3(0, 0, angle);
        Vector3 newWeaponLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
            newWeaponLocalScale.y = -1;
        else
            newWeaponLocalScale.x = 1;

        if (_weaponSprite) _weaponSprite.localScale = newWeaponLocalScale;
    }
}
