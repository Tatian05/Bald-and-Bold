using UnityEngine;
public class FRY_EnemyBullet : MonoBehaviour
{
    static FRY_EnemyBullet _instance;
    public static FRY_EnemyBullet Instance { get { return _instance; } }

    [SerializeField] EnemyBullet _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<EnemyBullet> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<EnemyBullet>(BulletCreator, EnemyBullet.TurnOn, EnemyBullet.TurnOff, _stock);
    }

    public EnemyBullet BulletCreator()
    {
        var bullet = Instantiate(_prefab);
        bullet.transform.SetParent(GameObject.Find("MainGame").transform);
        return bullet;
    }

    public void ReturnBullet(EnemyBullet b)
    {
        pool.ReturnObject(b);
    }
}
