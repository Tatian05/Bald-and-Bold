using UnityEngine;
public class TrainMovement : MonoBehaviour
{
    [SerializeField] float _speed;
    void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }
}
