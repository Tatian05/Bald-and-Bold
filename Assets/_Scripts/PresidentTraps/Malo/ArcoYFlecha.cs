using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ArcoYFlecha : MonoBehaviour
{
    [SerializeField] GameObject _arrowGO;
    [SerializeField] Transform[] _targets;
    [SerializeField] Transform _arrowParent, _presidentHead;
    [SerializeField] float _arrowSpeed;

    int _counter;
    GameObject _defeatArrow;
    Dictionary<GameObject, Transform> _arrows = new Dictionary<GameObject, Transform>();
    System.Action _arrowState = delegate { };
    private void Start()
    {
        _arrowState = LevelStart;
    }
    private void Update()
    {
        _arrowState?.Invoke();
    }

    void LevelStart()
    {
        if (_arrows.Count <= 0) return;

        foreach (var item in _arrows.ToDictionary(x => x.Key, y => y.Value))
        {
            Vector3 distance = item.Value.position - item.Key.transform.position;
            item.Key.transform.right = distance;
            item.Key.transform.position += distance.normalized * _arrowSpeed * Time.deltaTime;
            if (distance.magnitude <= .1f)
            {
                item.Key.transform.SetParent(item.Value);
                _arrows.Remove(item.Key);
                continue;
            }
        }
    }
    public void Shoot()
    {
        var arrow = Instantiate(_arrowGO, _arrowParent.position, _arrowParent.rotation);
        arrow.transform.SetParent(GameObject.Find("MainGame").transform);
        _arrows[arrow] = _targets[_counter++ % _targets.Length];
    }
    public void SetDefeatArrow()
    {
        _defeatArrow = Instantiate(_arrowGO, _arrowParent.position, _arrowParent.rotation);
        _defeatArrow.transform.SetParent(GameObject.Find("Cinematic").transform);
        _arrowState = DefeatArrow;
    }
    void DefeatArrow()
    {
        Vector3 distance = _presidentHead.position - _defeatArrow.transform.position;
        _defeatArrow.transform.position += distance.normalized * (_arrowSpeed - 2.4f) * Time.deltaTime;

        if (distance.magnitude < .05f)
        {
            Destroy(_defeatArrow);
            _arrowState = delegate { };
        }
    }
}
