using UnityEngine;
public class FRY_Granades : MonoBehaviour
{
    static FRY_Granades _instance;
    public static FRY_Granades Instance { get { return _instance; } }

    [SerializeField] Granade _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Granade> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<Granade>(BulletCreator, Granade.TurnOn, Granade.TurnOff, _stock);
    }

    public Granade BulletCreator()
    {
        var grenade = Instantiate(_prefab);
        grenade.transform.SetParent(GameObject.Find("MainGame").transform);
        return grenade;
    }

    public void ReturnBullet(Granade b)
    {
        pool.ReturnObject(b);
    }
}
