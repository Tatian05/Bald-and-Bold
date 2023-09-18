using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_MovingSaw : MonoBehaviour
{
    IMovement _wayPointMovement;

    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float _waypointSpeed = 5;

    void Start()
    {
        _wayPointMovement = new Movement_BasicTransformWayPoint(this.transform, _waypointSpeed, _wayPoints, true);
    }

    private void Update()
    {
        _wayPointMovement.Move();
    }
}
