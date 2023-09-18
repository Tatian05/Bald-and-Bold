using UnityEngine;
public class ShowKeyUI : MonoBehaviour
{
    KeysUI _keyUI;
    [SerializeField] string _keyName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI = FRY_KeysUI.Instance.pool.GetObject().SetPosition(transform.position + Vector3.up).SetButtonSprite(_keyName);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI.SetPosition(transform.position + Vector3.up);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() && _keyUI || collision.GetComponent<PlayerShop>() && _keyUI) _keyUI.ReturnObject();
    }
    private void OnDestroy()
    {
        if (_keyUI) _keyUI.ReturnObject();
    }
}
