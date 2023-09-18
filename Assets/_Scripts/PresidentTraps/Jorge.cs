using System.Collections.Generic;
using UnityEngine;
public class Jorge : MonoBehaviour
{
    List<GameObject> _objects = new List<GameObject>();

    [SerializeField] float _objectsSpeed;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Transform _targetObjetsPos;

    float _timer;
    void Update()
    {
        if (_objects.Count <= 0) return;

        _timer += Time.deltaTime;

        foreach (var item in _objects)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, _targetObjetsPos.position, _animationCurve.Evaluate(_timer / _objectsSpeed));
            item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.zero, _animationCurve.Evaluate(_timer / _objectsSpeed));

            if (item.transform.localScale == Vector3.zero)
            {
                _objects.Remove(item);
                Destroy(item);
                break;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            _objects.Add(collision.gameObject);
            _timer = 0;
            if (collision.name == "PresidentAnimation 15") GetComponent<SawPresident>().enabled = false;
        }
    }
}
