using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingWall : MonoBehaviour
{
    [SerializeField] Transform[] _wayPoints;
    int _index = 0;

    [SerializeField] float _startDelay = .5f;
    [SerializeField] float _speed;
    [SerializeField] float _waitSeconds;
    Vector3 _dir;
    bool _canMove;

    public void Start()
    {
        StartCoroutine(StartDelay());
    }

    private void Update()
    {
        if(_canMove) Move();
    }

    IEnumerator Wait()
    {
        _canMove = false;

        yield return new WaitForSeconds(_waitSeconds);

        _canMove = true;
    }

    void Move()
    {
        transform.position += _dir * _speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, _wayPoints[_index].transform.position) < .2f) ChangeDir();
    }

    void ChangeDir()
    {
        Helpers.GameManager.EnemyManager.HeavyAttack();
        _index++;

        if (_index > _wayPoints.Length - 1)
        {
            _index = 0;
        }

        _dir = (_wayPoints[_index].position - transform.position);

        _dir.Normalize();
        StartCoroutine(Wait());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(_startDelay);
        _canMove = true;
        ChangeDir();
    }
}