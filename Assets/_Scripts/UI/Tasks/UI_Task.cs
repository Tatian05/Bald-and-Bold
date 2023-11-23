using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class UI_Task : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _taskName, _taskDescription, _taskStage, _taskCurrentProgress, _taskGoldenBaldAward, _taskPresiCoinAward;
    [SerializeField] Image _backgroundImg, _progressBar;
    [SerializeField] Button _reclaimButton;
    [SerializeField] AnimationCurve _animationCurve;

    TaskUIManager _taskUIManager;
    Task _task;
    UI_TaskVariables _variables;
    int[] _points;
    System.Action _uiTaskAnimation;
    float _lerpTime, _lerpGoal;
    void Start()
    {
        _reclaimButton.onClick.AddListener(ReclaimButton);
        _taskUIManager = GetComponentInParent<TaskUIManager>();
    }
    public UI_Task SetTask(Task task)
    {
        _task = task;
        _variables = Helpers.PersistantData.tasks.GetTaskProgress(_task);
        _reclaimButton.interactable = _task.CanReclaimMision();
        if (_variables.currentStageGoal <= 0) _variables.currentStageGoal = _task.stages[_variables.currentStage];
        if (_variables.randomRotation == Vector3.zero) _variables.randomRotation = new Vector3(0, 0, Random.Range(-10, 11));
        transform.eulerAngles = _variables.randomRotation;

        return this;
    }
    public UI_Task SetStats()
    {
        _taskName.text = _task.taskName;
        _taskDescription.text = _task.taskDescription;
        _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
        _taskCurrentProgress.text = $"{(int)_variables.currentProgress}/ {_variables.currentStageGoal}";
        _taskGoldenBaldAward.text = _task.goldenBaldCoinsAward[_task.currentStageIndex].ToString();
        _taskPresiCoinAward.text = _task.presiCoinsAward[_task.currentStageIndex].ToString();

        _backgroundImg.color = _task.noteColor;
        _progressBar.fillAmount = _variables.currentProgress / _variables.currentStageGoal;

        _points = _task.stages.Skip(_variables.currentStage).Take(_task.currentStageIndex.Equals(_variables.currentStage) ? 1 : _task.currentStageIndex - _variables.currentStage).ToArray();
        _points[_points.Length - 1] = _task.progress;

        _lerpGoal = _task.totalProgress - _variables.currentProgress;
        _lerpTime = _lerpGoal * .5f;

        _uiTaskAnimation = UIAnimation;

        return this;
    }
    private void Update()
    {
        _uiTaskAnimation?.Invoke();
    }

    float _elapsedTime = 0;
    void UIAnimation()
    {
        if (_lerpGoal == 0) return;

        _elapsedTime += Time.deltaTime;
        _variables.currentProgress = Mathf.Lerp(_variables.currentProgress, _lerpGoal, _animationCurve.Evaluate(_elapsedTime / _lerpTime));

        _progressBar.fillAmount = _variables.currentProgress / _variables.currentStageGoal;
        _taskCurrentProgress.text = $"{(int)_variables.currentProgress}/ {_variables.currentStageGoal}";

        if (_variables.currentProgress >= _variables.currentStageGoal)
        {
            _variables.currentStage++;
            _variables.currentStageGoal = _task.stages[_variables.currentStage];
            _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
            _variables.currentProgress = 0;
        }

        if (_variables.currentStage >= _points.Length - 1 && _variables.currentProgress >= _points[_points.Length - 1])
        {
            Helpers.PersistantData.tasks.SetTaskProgress(_variables.ID, _task, _variables);
            _uiTaskAnimation = null;
        }
    }
    void ReclaimButton()
    {
        _task.ReclaimMision();
        SetStats();
        _taskUIManager.UpdateCoins();
    }
}
