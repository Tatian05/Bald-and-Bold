using UnityEngine;
public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float _parallaxMultiplier;
    [SerializeField] float _speed;

    Transform _cameraTransform;
    Vector3 _previousCameraPosition;
    float _spriteWidth, _startPosition;
    private void Start()
    {
        //_cameraTransform = Helpers.MainCamera.transform;
        //_previousCameraPosition = _cameraTransform.position;
        _spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        //_startPosition = transform.position.x;
    }

    private void LateUpdate()
    {
        float delta = (_speed * _parallaxMultiplier) * Time.deltaTime;
        transform.position -= new Vector3(delta, 0,0);

        if (Mathf.Abs(transform.position.x) - _spriteWidth > 0)
            transform.position = new Vector3(0, transform.position.y, transform.position.z);

        //float deltaX = (_cameraTransform.position.x - _previousCameraPosition.x) * _parallaxMultiplier;
        //float moveAmount = _cameraTransform.position.x * (1 - _parallaxMultiplier);
        //transform.Translate(new Vector3(deltaX, 0, 0));
        //_previousCameraPosition = _cameraTransform.position;
        //
        //if (moveAmount > _startPosition + _spriteWidth)
        //{
        //    transform.Translate(new Vector3(_spriteWidth, 0, 0));
        //    _startPosition += _spriteWidth;
        //}
        //else if (moveAmount < _startPosition - _spriteWidth)
        //{
        //    transform.Translate(new Vector3(-_spriteWidth, 0, 0));
        //    _startPosition -= _spriteWidth;
        //}
    }
}
