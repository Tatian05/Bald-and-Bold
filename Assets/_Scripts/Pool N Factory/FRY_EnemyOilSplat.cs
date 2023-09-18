using UnityEngine;
public class FRY_EnemyOilSplat : MonoBehaviour
{
    static FRY_EnemyOilSplat _instance;
    public static FRY_EnemyOilSplat Instance { get { return _instance; } }

    [SerializeField] SR_OilSplat _prefab;
    [SerializeField] int _stock = 10;

    public ObjectPool<SR_OilSplat> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<SR_OilSplat>(ObjectCreator, SR_OilSplat.TurnOn, SR_OilSplat.TurnOff, _stock);
    }
    public SR_OilSplat ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }
    public void ReturnObject(SR_OilSplat b)
    {
        pool.ReturnObject(b);
    }
}
