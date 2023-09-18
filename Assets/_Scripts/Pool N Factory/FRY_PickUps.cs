using UnityEngine;
using PickUps;
public class FRY_PickUps : MonoBehaviour
{
    static FRY_PickUps _instance;
    public static FRY_PickUps Instance { get { return _instance; } }

    [SerializeField] PickUp _prefab;
    [SerializeField] int _stock = 3;

    public ObjectPool<PickUp> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<PickUp>(ObjectCreator, PickUp.TurnOn, PickUp.TurnOff, _stock);
    }

    public PickUp ObjectCreator()
    {
        var pickUp = Instantiate(_prefab);
        pickUp.transform.SetParent(transform);
        return pickUp;
    }

    public void ReturnObject(PickUp p)
    {
        pool.ReturnObject(p);
    }
}
