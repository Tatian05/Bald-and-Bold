using UnityEngine;
public class FRY_GrenadeExplosion : MonoBehaviour
{
    static FRY_GrenadeExplosion _instance;
    public static FRY_GrenadeExplosion Instance { get { return _instance; } }


    [SerializeField] GrenadeExplosion _prefab;
    [SerializeField] int _stock = 3;

    public ObjectPool<GrenadeExplosion> pool;


    void Awake()
    {
        _instance = this;
        pool = new ObjectPool<GrenadeExplosion>(ObjectCreator, GrenadeExplosion.TurnOn, GrenadeExplosion.TurnOff, _stock);
    }

    public GrenadeExplosion ObjectCreator()
    {
        var o = Instantiate(_prefab);
        o.transform.SetParent(GameObject.Find("MainGame").transform);
        return o;
    }

    public void ReturnObject(GrenadeExplosion b)
    {
        pool.ReturnObject(b);
    }
}
