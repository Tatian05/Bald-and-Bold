using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_Shooting : Enemy
{
    protected Action OnAttack;
    IMovement _armRotation;

    [SerializeField] protected Transform bulletSpawnPosition;
    [SerializeField] protected Transform armPivot;
    [SerializeField] protected Transform weaponSprite;

    [SerializeField] protected float bulletDamage = 1f;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float attackSpeed = 2f;
    protected float currentAttackSpeed;

    public override void Start()
    {
        base.Start();
        OnUpdate += Attack;
        _armRotation = new Movement_RotateObject(armPivot, _playerCenterPivot, weaponSprite, sprite);
    }

    public virtual void Attack()
    {
        if (!CanSeePlayer()) return;

        _armRotation.Move();
        //LookAtPlayer();

        currentAttackSpeed += Time.deltaTime;

        if (currentAttackSpeed > attackSpeed)
        {
            OnAttack();
            currentAttackSpeed = 0;
        }
    }

    protected void LookAtPlayer()
    {
        Vector3 dirToLookAt = (gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, Mathf.Abs(dirToLookAt.x)) * Mathf.Rad2Deg;

        armPivot.localEulerAngles = new Vector3(0, 0, angle);

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

        weaponSprite.localScale = newWeaponLocalScale;
        sprite.localScale = newScale;
    }

}
