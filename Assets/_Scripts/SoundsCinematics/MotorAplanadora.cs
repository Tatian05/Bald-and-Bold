using UnityEngine;
public class MotorAplanadora : MonoBehaviour
{
    [SerializeField] AudioSource _motorLoop;

    void Start()
    {
        Invoke(nameof(StartMotor), 1f);
    }
    void StartMotor() { _motorLoop.Play(); }
}
