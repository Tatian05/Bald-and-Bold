using UnityEngine;
public class Inflador : MonoBehaviour
{
    Vector3 _initialScale;
    SpriteRenderer _spriteRenderer;

    [SerializeField] Vector3 _targetScale;
    [SerializeField] Color _targetColor;
    private void Start()
    {
        _initialScale = transform.localScale;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        transform.localScale = Vector3.Lerp(_initialScale, _targetScale, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
        _spriteRenderer.color = Color.Lerp(Color.white, _targetColor, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
