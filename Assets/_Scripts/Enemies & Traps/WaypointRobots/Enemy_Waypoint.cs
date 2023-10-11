using UnityEngine;
public class Enemy_Waypoint : Enemy
{
    [SerializeField] float _speed;
    [SerializeField] Transform _bodyToRotate;

    Vector3 _dir;
    bool _isFacingRight = true;
    public override void Start()
    {
        base.Start();
       
        if(!_bodyToRotate) _bodyToRotate = transform;
        Flip();
    }

    protected void Move()
    {
        transform.position += _dir * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InvisibleWall"))
            Flip();
    }
}
