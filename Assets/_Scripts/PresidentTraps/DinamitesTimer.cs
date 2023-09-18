using UnityEngine;
using TMPro;
public class DinamitesTimer : MonoBehaviour
{
    TextMeshProUGUI _timerTxt;
    Color[] _colors = new Color[3] { Color.white, Color.yellow, Color.red };
    int _seconds, _cents;
    float _maxTime;
    void Start()
    {
        _timerTxt = GetComponentInChildren<TextMeshProUGUI>();
        _maxTime = Helpers.LevelTimerManager.LevelMaxTime;
    }

    float _timer;
    void Update()
    {
        _timer = _maxTime - Helpers.LevelTimerManager.Timer;
        _seconds = (int)(_timer - (int)(_timer / 60f) * 60f);
        _cents = (int)((_timer - (int)_timer) * 100f);
        _timerTxt.text = string.Format("{0:00}:{1:00}", _seconds, _cents);
        _timerTxt.color = MultiLerp(Helpers.LevelTimerManager.Timer / _maxTime, _colors);
    }
    Color MultiLerp(float time, Color[] colors)
    {
        if (colors.Length == 1)
            return colors[0];
        else if (colors.Length == 2)
            return Color.Lerp(colors[0], colors[1], time);

        if (time == 0)
            return colors[0];

        if (time == 1)
            return colors[colors.Length - 1];

        float t = time * (colors.Length - 1);

        Color pointA = Color.white;
        Color pointB = Color.white;

        for (int i = 0; i < colors.Length; i++)
        {
            if (t < i)
            {
                pointA = colors[i - 1];
                pointB = colors[i];
                return Color.Lerp(pointA, pointB, t - (i - 1));
            }
            else if (t == (float)i)
            {
                return colors[i];
            }
        }
        return Color.white;
    }
}
