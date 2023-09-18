using UnityEngine;
public class FRY_EnemyBloodExplodeParticle : MonoBehaviour
{
    static FRY_EnemyBloodExplodeParticle _instance;
    public static FRY_EnemyBloodExplodeParticle Instance { get { return _instance; } }

    [SerializeField] PS_EnemyBloodExplode _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<PS_EnemyBloodExplode> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<PS_EnemyBloodExplode>(ObjectCreator, PS_EnemyBloodExplode.TurnOn, PS_EnemyBloodExplode.TurnOff, _stock);
    }

    public PS_EnemyBloodExplode ObjectCreator()
    {
        var particle = Instantiate(_prefab);
        particle.transform.SetParent(GameObject.Find("MainGame").transform);
        return particle;
    }

    public void ReturnObject(PS_EnemyBloodExplode b)
    {
        pool.ReturnObject(b);
    }
}
