using System.Collections;
using System.Linq;
using UnityEngine;
public class WavesDoor : MonoBehaviour
{
    [SerializeField] Animator _anim;
    Collider2D _collider;
    WavesEnemyManager _enemyManager;

    [SerializeField] int _allLevels;
    [SerializeField] int[] _newLevelOrder;

    private void Start()
    {
        _enemyManager = FindObjectOfType<WavesEnemyManager>();
        if (!_enemyManager) Debug.LogError("NO WavesEnemyManager");

        _enemyManager.OnWaveWon += ShowExit;

        _collider = GetComponent<Collider2D>();

        StartCoroutine(HideExit());
        StartCoroutine(NewLevels());
    }

    IEnumerator NewLevels()
    {
        yield return new WaitForEndOfFrame();
        if (Helpers.GameManager.SaveDataManager.GetBool("LevelsSetted", false))
            GetNewLevelOrder();
        else
            SetNewOrderOfLevels();

    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);
    }
    void SetNewOrderOfLevels()
    {
        PlayerPrefs.DeleteKey("LevelsWinned");

        _newLevelOrder = new int[_allLevels];
        System.Random rnd = new System.Random();
        _newLevelOrder = Enumerable.Range(0, _allLevels).OrderBy(_ => rnd.Next()).ToArray();
        for (int i = 0; i < _allLevels; i++)
        {
            Helpers.GameManager.SaveDataManager.SaveInt(i + 1.ToString(), _newLevelOrder[i] + 1);
            Debug.Log(PlayerPrefs.GetInt(i + 1.ToString()));
        }

        _enemyManager.currentLevel = 0;
        Helpers.GameManager.SaveDataManager.SaveInt("CurrentLevel", _enemyManager.currentLevel);

        Helpers.GameManager.SaveDataManager.SaveBool("LevelsSetted", true);

        //GameManager.instance.SaveData();
    }

    void GetNewLevelOrder()
    {
        _newLevelOrder = new int[_allLevels];

        _enemyManager.currentLevel = Helpers.GameManager.SaveDataManager.GetInt("CurrentLevel", 0);
        _enemyManager.currentLevel++;
        Helpers.GameManager.SaveDataManager.SaveInt("CurrentLevel", _enemyManager.currentLevel);

        for (int i = 0; i < _allLevels; i++)
            _newLevelOrder[i] = Helpers.GameManager.SaveDataManager.GetInt(i + 1.ToString(), 0);
    }

    public void NextLevel()
    {
        int fixedCurrentLevel = _enemyManager.currentLevel;

        if (fixedCurrentLevel < _newLevelOrder.Length)
        {
            int levels = PlayerPrefs.GetInt("LevelsWinned") + 1;
            PlayerPrefs.SetInt("LevelsWinned", levels);
            Helpers.GameManager.LoadSceneManager.LoadLevel("MiniGame 1 " + _newLevelOrder[_enemyManager.currentLevel]);
        }
        else
            EventManager.TriggerEvent(Contains.WIN_WAVESGAME); //CUANDO TERMINA TODOS LOS NIVELES EJECUTAMOS ESTE EVENTO
    }

    private void OnTriggerEnter2D()
    {
        NextLevel();
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
