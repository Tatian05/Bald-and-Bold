using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDecorations : MonoBehaviour
{
    Vector3 dir;
    [SerializeField]
    int _speed;
    float _timer=0;

    private void Start()
    {

        dir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1),0);
    }
    void Update()
    {

        transform.position +=_speed* dir*Time.deltaTime;
        _timer += Time.deltaTime;
        if(_timer>= 11)
        {
            Destroy(this.gameObject);
        }

    }
}
