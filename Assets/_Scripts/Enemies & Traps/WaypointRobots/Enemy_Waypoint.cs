using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Waypoint : Enemy
{
    [SerializeField] float _speed;
    [SerializeField] protected bool isStatic;

    [SerializeField] Transform _bodyToRotate;

    Vector3 _dir;
    bool _isFacingRight = true;


    public override void Start()
    {
        base.Start();
       
        if(!_bodyToRotate) _bodyToRotate = transform;

        OnUpdate += Move;

        Flip();
    }

    protected void Move()
    {
        if (isStatic) return;

        transform.position += _dir * _speed * Time.deltaTime;
    }

    void Flip()
    {
        Vector3 newScale = Vector3.one;

        if (_isFacingRight)
        {
            _isFacingRight = false;
            newScale.x = -1;

            _dir = -Vector3.right;
        }
        else
        {
            _isFacingRight = true;
            newScale.x = 1;
            _dir = Vector3.right;

        }

        _bodyToRotate.localScale = newScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InvisibleWall")
        {
            Flip();
        }
    }
}
