using UnityEngine;
public class InundationWater : MonoBehaviour
{
    [SerializeField] float _zoneTime;
    [SerializeField] Transform _endPoint;
    [SerializeField] float _timer;
    Vector3 _waterInitialPos;
    private void Start()
    {
        _waterInitialPos = transform.position;
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        transform.position = Vector3.Lerp(_waterInitialPos, _endPoint.position, _timer / _zoneTime);

        if (_timer >= _zoneTime) Helpers.GameManager.LoadSceneManager.ReloadLevel();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            player.PauseInDeath();
            Helpers.GameManager.LoadSceneManager.ReloadLevel();
        }
        if (collision.TryGetComponent(out CheckPoint checkPoint))
        {
            if (checkPoint.IsCurrentCheckpoint)
                checkPoint.OnDestroyCheckpoint();
        }
    }
}
