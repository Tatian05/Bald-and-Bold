using UnityEngine;
public class FRY_EnemyBloodSplatter : MonoBehaviour
{
    static FRY_EnemyBloodSplatter _instance;
    public static FRY_EnemyBloodSplatter Instance { get { return _instance; } }

    [SerializeField] SR_BloodSplatter _prefab;
    [SerializeField] int _stock = 10;

    public ObjectPool<SR_BloodSplatter> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<SR_BloodSplatter>(ObjectCreator, SR_BloodSplatter.TurnOn, SR_BloodSplatter.TurnOff, _stock);
    }

    public SR_BloodSplatter ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }

    public void ReturnObject(SR_BloodSplatter b)
    {
        pool.ReturnObject(b);
    }
}
