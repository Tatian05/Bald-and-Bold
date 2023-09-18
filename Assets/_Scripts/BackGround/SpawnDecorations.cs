using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDecorations : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _decorations = new List<GameObject>();
    [SerializeField]
    List<Transform> _spawn = new List<Transform>();
    float _timer;
   

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 10)
        {
            Spawn();
            _timer = 0;
        }
    }

    void Spawn()
    {
        Instantiate(_decorations[Random.Range(0, _decorations.Count)], _spawn[Random.Range(0, _spawn.Count)]);
    }
}
