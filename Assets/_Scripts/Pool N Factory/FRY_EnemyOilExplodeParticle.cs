using UnityEngine;
public class FRY_EnemyOilExplodeParticle : MonoBehaviour
{
    static FRY_EnemyOilExplodeParticle _instance;
    public static FRY_EnemyOilExplodeParticle Instance { get { return _instance; } }

    [SerializeField] PS_EnemyOilExplode _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<PS_EnemyOilExplode> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<PS_EnemyOilExplode>(ObjectCreator, PS_EnemyOilExplode.TurnOn, PS_EnemyOilExplode.TurnOff, _stock);
    }

    public PS_EnemyOilExplode ObjectCreator()
    {
        var particle = Instantiate(_prefab);
        particle.transform.SetParent(GameObject.Find("MainGame").transform);
        return particle;
    }

    public void ReturnObject(PS_EnemyOilExplode b)
    {
        pool.ReturnObject(b);
    }
}
