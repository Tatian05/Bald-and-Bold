using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    bool _paused = false;
    bool _tutorialPause;
    Stack<IScreen> _stack;
    CinematicManager _cinematicManager;
    InputAction _pauseAction;

    [SerializeField] Transform _mainGame;
    [SerializeField] ScreenPause _pauseGO;
    public bool Paused { get { return _paused; } }
    public bool TurnPause => _paused = !_paused;
    private void Awake()
    {
        _stack = new Stack<IScreen>();
        _pauseAction = NewInputManager.PlayerInputs.Player.Pause;
        Push(new PauseGO(_mainGame));
    }
    private void OnEnable()
    {
        _pauseAction.Enable();
        _pauseAction.performed += Pause;
        EventManager.SubscribeToEvent(Contains.PAUSE_GAME, PauseGame);
    }
    private void OnDisable()
    {
        _pauseAction.Disable();
        _pauseAction.performed -= Pause;
        EventManager.UnSubscribeToEvent(Contains.PAUSE_GAME, PauseGame);
    }
    private void Start()
    {
        _cinematicManager = Helpers.GameManager.CinematicManager;
        Helpers.LevelTimerManager.OnLevelDefeat += PauseObjectsInCinematic;
    }
    void Pause(InputAction.CallbackContext obj)
    {
        EventManager.TriggerEvent(Contains.PAUSE_GAME, TurnPause);
    }
    void PauseGame(params object[] param)
    {
        if (!_cinematicManager.playingCinematic && !_tutorialPause)
        {
            if((bool)param[0]) Push(Instantiate(_pauseGO));
            else Pop();
        }
    }
    public void Pop()
    {
        if (_stack.Count <= 1) return;
        _stack.Pop().Free();
        if (_stack.Count > 0) _stack.Peek().Activate();
    }
    public void Push(IScreen screen)
    {
        if (_stack.Count > 0) _stack.Peek().Deactivate();
        _stack.Push(screen);
        screen.Activate();
    }
    public void PauseObjectsInCinematic()
    {
        _stack.Peek().PauseObjectsInCinematic();
    }
    public void UnpauseObjectsInCinematic()
    {
        _stack.Peek().UnpauseObjectsInCinematic();
    }
}
