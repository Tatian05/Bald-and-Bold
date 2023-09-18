using UnityEngine;
public class SawPresident : MonoBehaviour
{
    [SerializeField] Transform _targetPosition;
    Transform _myTransform;
    Vector3 _initialPosition;
    private void Start()
    {
        _myTransform = transform;
        _initialPosition = transform.position;
    }
    void Update()
    {
        _myTransform.position = Vector3.Lerp(_initialPosition, _targetPosition.position, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
