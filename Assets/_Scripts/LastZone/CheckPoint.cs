using System.Collections;
using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _UIcheckpointRenderTextureGO;

    BoxCollider2D _trigger;
    Player _player;
    bool _isThisCheckpoint;
    GameObject _checkpointCamera;
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
        if (_player && _isThisCheckpoint) _player.ResetCheckPoint();
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
            _player.Checkpoint = transform.position;
            _isThisCheckpoint = true;
            _animator.Play("Checkpoint");
        }
    }
}
