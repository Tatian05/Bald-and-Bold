using UnityEngine;
public static class CustomTime
{
    public static float LocalTimeScale = 1f;
    public static float DeltaTime { get { return Time.deltaTime * LocalTimeScale; } }
    public static float TimeScale { get { return Time.timeScale * LocalTimeScale; } }
    public static bool IsPaused { get { return LocalTimeScale <= 0; } }

    public static void SetTimeScale(int newTimeScale) { LocalTimeScale = newTimeScale; }
}
