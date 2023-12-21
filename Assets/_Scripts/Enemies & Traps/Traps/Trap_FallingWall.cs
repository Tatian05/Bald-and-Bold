using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingWall : MonoBehaviour
{
    enum FallingWall { Wait, Move }
    [SerializeField] Transform[] _wayPoints;
    int _index = 0;

    [SerializeField] float _speed;
    [SerializeField] float _waitSeconds;
    Vector3 _dir;

    EventFSM<FallingWall> _myFSM;

    private void Start()
    {
        var wait = new State<FallingWall>("wait");
        var move = new State<FallingWall>("Move");

        StateConfigurer.Create(wait).SetTransition(FallingWall.Move, move).Done();
        StateConfigurer.Create(move).SetTransition(FallingWall.Wait, wait).Done();

        float waitTimer = 0;
        wait.OnEnter += x =>
        {
            Helpers.GameManager.EnemyManager.HeavyAttack();
            _index++;
            if (_index > _wayPoints.Length - 1) _index = 0;

            _dir = _wayPoints[_index].position - transform.position;

            _dir.Normalize();
        };
        wait.OnUpdate += () =>
        {
            waitTimer += CustomTime.DeltaTime;
            if (waitTimer >= _waitSeconds)
            {
                waitTimer = 0;
                _myFSM.SendInput(FallingWall.Move);
            }
        };

        move.OnUpdate += () => Move();

        _myFSM = new EventFSM<FallingWall>(wait);
    }

    private void Update()
    {
        _myFSM?.Update();
    }
    void Move()
    {
        transform.position += _dir * _speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, _wayPoints[_index].transform.position) < .2f) _myFSM.SendInput(FallingWall.Wait);
    }
}