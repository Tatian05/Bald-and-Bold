using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class TaskUIManager : MonoBehaviour
{
    [SerializeField] UI_Task _uiTaskPrefab;
    [SerializeField] TextMeshProUGUI _goldenBaldTxt, _presiCoinTxt;
    [SerializeField] Transform[] _papersContent;
    [SerializeField] TaskPaper[] _taskPaper;

    PersistantData _persistantData;
    int _index = 0;
    int _currentPageIndex;
    InputAction _nextBeforePage;
    Vector2 _navigateInput;
    private void Awake()
    {
        _nextBeforePage = EventSystemScript.Instance.UIInputs.UI.Navigate;
    }
    private void Start()
    {
        _persistantData = Helpers.PersistantData;
        UpdateCoins();

        var taskSO = Helpers.PersistantData.tasksSO;
        for (int i = 0; i < _papersContent.Length; i++)
        {
            for (int t = 0; t < 4; t++)
            {
                Instantiate(_uiTaskPrefab, _papersContent[i]).SetPosition(_taskPaper[i].Spawns[t].position).SetTask(taskSO[_index]).SetStats();
                if (_index >= taskSO.Length - 1)
                    break;
                _index++;
            }

            if (_index >= taskSO.Length - 1)
                break;
        }
    }
    private void OnEnable()
    {
        _nextBeforePage.performed += PassPage;
        _nextBeforePage.Enable();
    }
    private void OnDisable()
    {
        _nextBeforePage.performed -= PassPage;
        _nextBeforePage.Disable();
    }
    public void UpdateCoins()
    {
        _goldenBaldTxt.text = _persistantData.persistantDataSaved.goldenBaldCoins.ToString();
        _presiCoinTxt.text = _persistantData.persistantDataSaved.presiCoins.ToString();
    }

    int _inputCounter;
    void PassPage(InputAction.CallbackContext obj)
    {
        if (_inputCounter >= 1)
        {
            _inputCounter = 0;
            return;
        }

        _navigateInput = new Vector2 { x = Mathf.RoundToInt(_nextBeforePage.ReadValue<Vector2>().x), y = Mathf.RoundToInt(_nextBeforePage.ReadValue<Vector2>().y) };
        if (_navigateInput.x > 0 || _navigateInput.y > 0) NextPage();
        else BeforePage();

        _inputCounter++;
    }
    bool _last;
    void NextPage()
    {
        if (_currentPageIndex > _taskPaper.Length - 2) return;
        _last = _currentPageIndex >= _taskPaper.Length - 1;
        _taskPaper[_currentPageIndex].PlayNext();
        _first = false;
        if (_currentPageIndex < _taskPaper.Length - 1) _currentPageIndex++;
    }
    bool _first;
    void BeforePage()
    {
        if (_first) return;
        _taskPaper[_last ? _currentPageIndex : _currentPageIndex - 1].PlayBefore();
        _last = false;
        if (_currentPageIndex > 0) _currentPageIndex--;
        _first = _currentPageIndex == 0;
    }
}

[System.Serializable]
public struct UI_TaskVariables
{
    public int currentStageGoal, currentStage;
    public float currentProgress;
    public Vector3 randomRotation;
    public bool completed;
}