using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Moving : MonoBehaviour
{
    IMovement _wayPointMovement;

    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float _speed;
    Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _wayPointMovement = new Movement_BasicTransformWayPoint(transform, _speed, _wayPoints);
    }

    private void FixedUpdate()
    {
        _wayPointMovement.Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.gameObject.transform.parent = this.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.gameObject.transform.parent = null;
    }
}
