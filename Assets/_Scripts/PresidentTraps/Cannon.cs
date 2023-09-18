using System.Collections;
using System.Linq;
using UnityEngine;
public class Cannon : MonoBehaviour
{
    [SerializeField] Transform[] _points;
    [SerializeField] Transform _sparks;
    [SerializeField] Transform _ballSpawn;
    [SerializeField] GameObject _ball;
    [SerializeField] float _ballForce;

    LineRenderer _lineRenderer;
    Vector3 _offset = new Vector3(-.15f, 0);
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _points.Length;
        for (int i = 0; i <= _points.Length - 1; i++)
            _lineRenderer.SetPosition(i, _points[i].position);

        StartCoroutine(Mecha());
    }
    IEnumerator Mecha()
    {
        int index = 0;
        Vector3[] positions = _points.Select(x => x.position).ToArray();
        while (Helpers.LevelTimerManager.Timer <= Helpers.LevelTimerManager.LevelMaxTime)
        {
            for (int i = 0; i <= index; i++)
            {
                _points[i].position = MultiLerp(Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime, positions);
                _lineRenderer.SetPosition(i, _points[i].position);
            }
            _sparks.position = _points[index].position + _offset;
            if (Vector3.Distance(_points[index].position, _points[index + 1].position) <= 0.01f) index++;
            if (index + 1 == _points.Length) break;
            yield return null;
        }
        _lineRenderer.positionCount = 0;
    }
    public void Shoot()
    {
        GameObject ball = Instantiate(_ball, _ballSpawn.position, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(Vector2.right * _ballForce, ForceMode2D.Impulse);
    }
    Vector3 MultiLerp(float time, Vector3[] points)
    {
        if (points.Length == 1)
            return points[0];
        else if (points.Length == 2)
            return Vector3.Lerp(points[0], points[1], time);

        if (time == 0)
            return points[0];

        if (time == 1)
            return points[points.Length - 1];

        float t = time * (points.Length - 1);

        Vector3 pointA = Vector3.zero;
        Vector3 pointB = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            if (t < i)
            {
                pointA = points[i - 1];
                pointB = points[i];
                return Vector3.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
            {
                return points[i];
            }
        }
        return Vector3.zero;
    }
}
