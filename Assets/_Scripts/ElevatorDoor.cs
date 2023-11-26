using UnityEngine;
public class ElevatorDoor : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] string _introAnimName;
    void Start()
    {
        _animator.Play(_introAnimName);
    }
}
