using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;
public class TaskUIManager : MonoBehaviour
{
    [SerializeField] UI_Task _uiTaskPrefab;
    [SerializeField] Transform _presiCoinsTransform, _goldenBaldCoinsTransform;
    [SerializeField] TextMeshProUGUI _goldenBaldTxt, _presiCoinTxt;
    [SerializeField] Transform[] _papersContent;
    [SerializeField] TaskPaper[] _taskPaper;
    [SerializeField] Button _nextPageButton, _previousPageButton;

    [Header("GamepadUI")]
    [SerializeField] string[] _backTMP, _nextPageTMP, _previousTMP;
    [SerializeField] TextMeshProUGUI _backTMPTxt, _nextPageTMPTxt, _previousPageTMPText;
    [SerializeField] GameObject _gamepadContainer, _backButtonGO;

    PersistantData _persistantData;
    int _index = 0;
    int _currentPageIndex;
    InputAction _gamepadNextPage, _gamepadPreviousPage;
    private void Awake()
    {
        _gamepadNextPage = NewInputManager.PlayerInputs.UI.Next;
        _gamepadPreviousPage = NewInputManager.PlayerInputs.UI.Before;
    }
    private void Start()
    {
        _nextPageButton.onClick.AddListener(NextPage);
        _previousPageButton.onClick.AddListener(BeforePage);

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

            if (_index >= taskSO.Length)
                break;
        }

        GamepadUI();
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += GamepadUI;
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= GamepadUI;
    }
    public void UpdateCoins()
    {
        _presiCoinTxt.text = _persistantData.persistantDataSaved.presiCoins.ToString();
        _goldenBaldTxt.text = _persistantData.persistantDataSaved.goldenBaldCoins.ToString();
    }
    public void UpdateCoinsAnimation()
    {
        _presiCoinTxt.text = _persistantData.persistantDataSaved.presiCoins.ToString();
        _goldenBaldTxt.text = _persistantData.persistantDataSaved.goldenBaldCoins.ToString();

        _presiCoinsTransform.DOScale(Vector3.one * 1.25f, .1f).OnComplete(() => _presiCoinsTransform.DOScale(Vector3.one, .1f));//.SetEase(Ease.InBack));
        _goldenBaldCoinsTransform.DOScale(Vector3.one * 1.25f, .1f).OnComplete(() => _goldenBaldCoinsTransform.DOScale(Vector3.one, .1f));//.SetEase(Ease.InBack));
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
    bool _first = true;
    void BeforePage()
    {
        if (_first) return;
        _taskPaper[_last ? _currentPageIndex : _currentPageIndex - 1].PlayBefore();
        _last = false;
        if (_currentPageIndex > 0) _currentPageIndex--;
        _first = _currentPageIndex == 0;
    }

    #region GAMEPAD
    void GamepadUI()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _nextPageButton.gameObject.SetActive(false);
            _previousPageButton.gameObject.SetActive(false);
            _backButtonGO.SetActive(false);
            _gamepadContainer.SetActive(true);

            _backTMPTxt.text = _backTMP[(int)NewInputManager.activeDevice - 1];
            _nextPageTMPTxt.text = _nextPageTMP[(int)NewInputManager.activeDevice - 1];
            _previousPageTMPText.text = _previousTMP[(int)NewInputManager.activeDevice - 1];

            _gamepadNextPage.performed += GamepadNextTrigger;
            _gamepadNextPage.Enable();
            _gamepadPreviousPage.performed += GamepadPreviousTrigger;
            _gamepadPreviousPage.Enable();
        }
        else
        {
            _gamepadContainer.SetActive(false);
            _nextPageButton.gameObject.SetActive(true);
            _previousPageButton.gameObject.SetActive(true);
            _backButtonGO.SetActive(true);

            _gamepadNextPage.performed -= GamepadNextTrigger;
            _gamepadNextPage.Disable();
            _gamepadPreviousPage.performed -= GamepadPreviousTrigger;
            _gamepadPreviousPage.Disable();
        }
    }

    void GamepadNextTrigger(InputAction.CallbackContext obj) => _nextPageButton.onClick.Invoke();
    void GamepadPreviousTrigger(InputAction.CallbackContext obj) => _previousPageButton.onClick.Invoke();

    #endregion

}

[System.Serializable]
public struct UI_TaskVariables
{
    public int currentStageGoal, currentStage;
    public float currentProgress;
    public Vector3 randomRotation;
    public bool completed;
}