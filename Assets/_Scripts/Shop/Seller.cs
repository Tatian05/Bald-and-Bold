using UnityEngine;
public class Seller : MonoBehaviour
{
    PlayerShop _playerShop;
    void Start()
    {
        _playerShop = FindObjectOfType<PlayerShop>();
    }
    void Update()
    {
        float playerDist = _playerShop.transform.position.x - transform.position.x;

        transform.eulerAngles = Mathf.Sign(playerDist) < 0 ? new Vector3(0, 180, 0) : Vector3.zero;
    }
}
