using System.Collections.Generic;
public class EventManager
{
    public delegate void EventReceiver(params object[] parameterContainer);

    public static Dictionary<string, EventReceiver> _events;

    public static void SubscribeToEvent(string eventType, EventReceiver listener)
    {
        if (_events == null)
            _events = new Dictionary<string, EventReceiver>();

        if (!_events.ContainsKey(eventType))
            _events.Add(eventType, null);

        _events[eventType] += listener;
    }
    public static void UnSubscribeToEvent(string eventType, EventReceiver listener)
    {
        if (_events != null)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] -= listener;
        }
    }
    public static void TriggerEvent(string eventType)
    {
        TriggerEvent(eventType, null);
    }
    public static void TriggerEvent(string eventType, params object[] parametersWrapper)
    {
        if (_events == null)
        {
            UnityEngine.Debug.LogWarning("No events subscribed");
            return;
        }

        if (_events.ContainsKey(eventType))
        {
            if (_events[eventType] != null)
                _events[eventType](parametersWrapper);
        }
    }
}
public class Contains
{
    public const string WIN_WAVESGAME = "Cuando el player termina el minijuego de oleadas";
    public const string LOSE_WAVESGAME = "Cuando el player pierde el minijuego de oleadas";
    public const string DUMMY_SPAWN = "Spawn Dummy";
    public const string PLAYER_DEAD = "Player Dead";
    public const string WAIT_PLAYER_DEAD = "Wait Player Dead";
    public const string ON_ENEMIES_KILLED = "On Enemies Killed";
    public const string ON_LEVEL_START = "On level start";
    public const string PAUSE_GAME = "Pause_Game";
    public const string ON_ROOM_WON = "On Room Won";
    public const string MISSION_PROGRESS = "Mission Progress";
}