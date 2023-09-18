using UnityEngine;
using TMPro;

public class EnemyFeedback : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    [SerializeField] float _maxLifeCount = 1;
    float _currentLifeCount;

    private void Update()
    {
        _currentLifeCount += Time.deltaTime;
        if (_currentLifeCount > _maxLifeCount) ReturnObject();
    }

    public EnemyFeedback SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public EnemyFeedback SetText(string txt)
    {
        _text.text = txt;
        return this;
    }

    private void Reset()
    {
        _currentLifeCount = 0;
    }

    public static void TurnOn(EnemyFeedback b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyFeedback b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        FRY_EnemyFeedback.Instance.ReturnObject(this);
    }
}
