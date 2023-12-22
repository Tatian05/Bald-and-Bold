using UnityEngine;
using System;
public class GameManager : Singleton<GameManager>
{
    public Action SetPlayerSkin = delegate { }, SetPresidentSkin = delegate { };

    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer, _playerLayer, _weaponLayer, _dynamicBodies, _borderLayer, _invisibleWallLayer, _enemyBulletLayer;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }
    public LayerMask BorderLayer { get { return _borderLayer; } private set { } }
    public LayerMask PlayerLayer { get { return _playerLayer; } private set { } }
    public LayerMask WeaponLayer { get { return _weaponLayer; } private set { } }
    public LayerMask InvisibleWallLayer { get { return _invisibleWallLayer; } private set { } }
    public LayerMask DynamicBodiesLayer { get { return _dynamicBodies; } private set { } }
    public LayerMask EnemyBulletLayer { get { return _enemyBulletLayer; } private set { } }

    GeneralPlayer _player;
    public GeneralPlayer Player { get { return _player; } private set { } }

    [Header("GameModes")]
    [SerializeField] int _defaultHardLives = 3;
    public int DefaultHardLives { get { return _defaultHardLives; } private set { } }

    [SerializeField] LoadSceneManager _loadSceneManager;

    public LoadSceneManager LoadSceneManager { get { return _loadSceneManager; } private set { } }

    private EnemyManager _enemyManager;
    public EnemyManager EnemyManager { get { return _enemyManager; } private set { } }

    private EffectsManager _effectsManager;
    public EffectsManager EffectsManager { get { return _effectsManager; } private set { } }

    private SaveDataManager _saveDataManager;
    public SaveDataManager SaveDataManager { get { return _saveDataManager; } private set { } }

    private PauseManager _pauseManager;
    public PauseManager PauseManager { get { return _pauseManager; } private set { } }

    DropManager _dropManager;
    public DropManager DropManager { get { return _dropManager; } }

    CinematicManager _cinematicManager;
    public CinematicManager CinematicManager { get { return _cinematicManager; } }

    QuestManager _questManager;
    public QuestManager QuestManager { get { return _questManager; } }

    ConsumablesCanvasContainer _consumableCanvasContainer;
    public ConsumablesCanvasContainer ConsumableCanvas { get { return _consumableCanvasContainer; } }
    override protected void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<GeneralPlayer>();
        _enemyManager = GetComponent<EnemyManager>();
        if (_enemyManager == null) _enemyManager = FindObjectOfType<EnemyManager>();
        _effectsManager = GetComponent<EffectsManager>();
        _saveDataManager = GetComponent<SaveDataManager>();
        _pauseManager = GetComponent<PauseManager>();
        _dropManager = GetComponent<DropManager>();
        _cinematicManager = GetComponent<CinematicManager>();
        _questManager = GetComponent<QuestManager>();
        _consumableCanvasContainer = GetComponent<ConsumablesCanvasContainer>();
        Cursor.lockState = CursorLockMode.Confined;
        if (!Helpers.LevelTimerManager || !Helpers.LevelTimerManager.enabled) CustomTime.SetTimeScale(1);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4)) _loadSceneManager.LoadAsyncFadeOut("LevelFinal");
    }
}
