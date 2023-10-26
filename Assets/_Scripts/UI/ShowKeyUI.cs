using UnityEngine;
public class ShowKeyUI : MonoBehaviour
{
    [SerializeField] KeysUI _keyUI;
    [SerializeField] string _actionName;
    [SerializeField, Tooltip("OPTIONAL, SI DESEA USARLO DEBE CONTENER EL NOMBRE DEL TMP_SPRITE CORRESPONDIENTE EN EL MENSAJE")] string _message;
    [SerializeField] int _spriteAnimationExtraFrames = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI = FRY_KeysUI.Instance.pool.GetObject().
                SetPosition(transform.position + Vector3.up).SetAction(_actionName).SetExtraFramesAndMessage(_spriteAnimationExtraFrames, _message).SetText();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() || collision.GetComponent<PlayerShop>()) _keyUI.SetPosition(transform.position + Vector3.up);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>() && _keyUI != null || collision.GetComponent<PlayerShop>() && _keyUI != null) _keyUI.ReturnObject();
    }
    private void OnDestroy()
    {
        if (_keyUI) _keyUI.ReturnObject();
    }
}
