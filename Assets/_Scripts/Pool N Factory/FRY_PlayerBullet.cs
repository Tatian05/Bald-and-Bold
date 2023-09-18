using UnityEngine;
public class FRY_PlayerBullet : MonoBehaviour
{
    static FRY_PlayerBullet _instance;
    public static FRY_PlayerBullet Instance { get { return _instance; } }

    [SerializeField] PlayerBullet _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<PlayerBullet> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<PlayerBullet>(BulletCreator, PlayerBullet.TurnOn, PlayerBullet.TurnOff, _stock);
    }

    public PlayerBullet BulletCreator()
    {
        var bullet = Instantiate(_prefab);
        bullet.transform.SetParent(GameObject.Find("MainGame").transform);
        return bullet;
    }

    public void ReturnBullet(PlayerBullet b)
    {
        pool.ReturnObject(b);
    }
}
