using System.Collections;
using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _UIcheckpointRenderTextureGO;

    BoxCollider2D _trigger;
    Player _player;
    GameObject _checkpointCamera;

    public bool IsCurrentCheckpoint { get { return _player != null && _player.Checkpoint == this; } private set { } }
    private void Start()
    {
        _trigger = GetComponent<BoxCollider2D>();
        _checkpointCamera = gameObject.GetComponentInChildren<Camera>(true, true).gameObject;
    }

    public void OnDestroyCheckpoint() { StartCoroutine(ShowCheckpointDestroying()); }
    void DestroyCheckpoint()
    {
        _trigger.enabled = false;
        _animator.Play("Destroy");
        if (_player && _player.Checkpoint == this) _player.ResetCheckPoint();
    }
    IEnumerator ShowCheckpointDestroying()
    {
        _checkpointCamera.gameObject.SetActive(true);
        _UIcheckpointRenderTextureGO.SetActive(true);
        yield return new WaitForSeconds(.5f);
        DestroyCheckpoint();
        yield return new WaitForSeconds(4f);
        _UIcheckpointRenderTextureGO.SetActive(false);
        _checkpointCamera.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _player = player;
            _player.Checkpoint = this;
            _animator.Play("Checkpoint");
        }
    }
}
