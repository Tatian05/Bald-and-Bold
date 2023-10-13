using System.Collections.Generic;
using UnityEngine;
public class PauseManager : MonoBehaviour
{
    bool _paused = false;
    bool _tutorialPause;
    Stack<IScreen> _stack;
    CinematicManager _cinematicManager;
    InputManager _inputManager;

    [SerializeField] Transform _mainGame;
    [SerializeField] ScreenPause _pauseGO;
    public bool Paused { get { return _paused; } }
    private void Awake()
    {
        _stack = new Stack<IScreen>();
        _inputManager = InputManager.Instance;
        Push(new PauseGO(_mainGame));
    }
    private void Start()
    {
        _cinematicManager = Helpers.GameManager.CinematicManager;
        Helpers.LevelTimerManager.OnLevelDefeat += PauseObjectsInCinematic;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_cinematicManager.playingCinematic && !_tutorialPause)
        {
            if (_paused) Pop();
            else Push(Instantiate(_pauseGO));

            TurnPause();
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

    public void TurnPause() => _paused = !_paused;

    public void PauseObjectsInCinematic()
    {
        _stack.Peek().PauseObjectsInCinematic();
    }
    public void UnpauseObjectsInCinematic()
    {
        _stack.Peek().UnpauseObjectsInCinematic();
    }
}
