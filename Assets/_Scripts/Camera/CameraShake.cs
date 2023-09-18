using System.Collections;
using UnityEngine;
public class CameraShake : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] AnimationCurve _curve;
    bool _isInCoroutine;

    private void Start()
    {
        _gameManager = GameManager.instance;

        _gameManager.EnemyManager.OnEnemyKilled += () => StartCoroutine(Shaking());
        _gameManager.EnemyManager.OnHeavyAttack += () => StartCoroutine(Shaking());
    }

    public void Shake()
    {
        if (!_isInCoroutine) StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Debug.Log("Shaking");

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _gameManager.CameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = _curve.Evaluate(elapsedTime / _gameManager.CameraShakeDuration);
            transform.position = startPosition + Random.insideUnitSphere * strenght;
            yield return null;
        }

        transform.position = startPosition;
        _isInCoroutine = false;
    }


}
