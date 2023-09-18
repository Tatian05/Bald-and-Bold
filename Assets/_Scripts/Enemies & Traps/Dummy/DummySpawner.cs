using UnityEngine;
public class DummySpawner : MonoBehaviour
{
    [SerializeField] GameObject _dummyPrefab;
    void Start()
    {
        EventManager.SubscribeToEvent(Contains.DUMMY_SPAWN, InvokeSpawn);

        EventManager.TriggerEvent(Contains.DUMMY_SPAWN);
    }
    private void OnDisable()
    {
        EventManager.UnSubscribeToEvent(Contains.DUMMY_SPAWN, InvokeSpawn);
    }
    void InvokeSpawn(params object[] param) { Invoke(nameof(SpawnDummy), 1f); }
    void SpawnDummy() { Instantiate(_dummyPrefab, transform.position, Quaternion.identity); }

}
