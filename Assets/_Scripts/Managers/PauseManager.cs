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
        Push(new PauseGO(_mainGame));
        _pauseAction = NewInputManager.PlayerInputs.Player.Pause;
    }
    private void OnEnable()
    {
        _pauseAction.performed += Pause;
    }
    private void OnDisable()
    {
        _pauseAction.performed -= Pause;
    }
    private void Start()
    {
        _cinematicManager = Helpers.GameManager.CinematicManager;
        Helpers.LevelTimerManager.OnLevelDefeat += PauseObjectsInCinematic;
    }
    void Pause(InputAction.CallbackContext obj)
    {
        PauseGame();
    }
    public void PauseGame()
    {
        if (!_cinematicManager.playingCinematic && !_tutorialPause)
        {
            if (TurnPause) Push(Instantiate(_pauseGO));
            else Pop();
        }
    }
    public void Pop()
    {
        if (_stack.Count <= 1) return;
        _stack.Pop().Free();
        if (_stack.Count > 0)
        {
            _stack.Peek().Activate();
            CustomTime.SetTimeScale(1);
            _pauseAction?.Disable();
            NewInputManager.EnablePlayer();
        }
    }
    public void Push(IScreen screen)
    {
        if (_stack.Count > 0)
        {
            _stack.Peek().Deactivate();
            CustomTime.SetTimeScale(0);
            NewInputManager.DisablePlayer();
            _pauseAction?.Enable();
        }

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
