using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections;
using DG.Tweening;
public class UI_Task : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _taskName, _taskDescription, _taskStage, _taskCurrentProgress, _taskGoldenBaldAward, _taskPresiCoinAward;
    [SerializeField] Image _backgroundImg, _fakeMaskImg, _progressBar;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] Animator _animator;

    TaskUIManager _taskUIManager;
    TaskSO _task;
    UI_TaskVariables _variables;
    System.Action _uiTaskAnimation;
    float _lerpTime, _lerpGoal;
    bool _endGoal;
    void Start()
    {
        _taskUIManager = GetComponentInParent<TaskUIManager>();
    }
    public UI_Task SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }
    public UI_Task SetTask(TaskSO task)
    {
        _task = task;
        _variables = Helpers.PersistantData.tasks.GetUITaskProgress(_task.ID);
        if (_variables.completed)
        {
            gameObject.SetActive(false);
            return this;
        }
        if (_variables.currentStageGoal <= 0) _variables.currentStageGoal = _task.stages[_variables.currentStage];
        if (_variables.randomRotation == Vector3.zero) _variables.randomRotation = new Vector3(0, 0, Random.Range(-10, 11));
        transform.eulerAngles = _variables.randomRotation;

        return this;
    }
    public UI_Task SetStats()
    {
        if (_variables.completed) return this;
        _taskName.text = _task.taskName;
        _taskDescription.text = _task.taskDescription;
        _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
        _taskCurrentProgress.text = $"{(int)_variables.currentProgress}/ {_variables.currentStageGoal}";
        _taskGoldenBaldAward.text = _task.goldenBaldCoinsAward[_variables.currentStage].ToString();
        _taskPresiCoinAward.text = _task.presiCoinsAward[_variables.currentStage].ToString();

        _backgroundImg.color = _task.noteColor;
        _fakeMaskImg.color = _task.noteColor;

        _progressBar.fillAmount = _variables.currentProgress / _variables.currentStageGoal;

        _endGoal = _variables.currentProgress >= _task.taskProgress.progress && _variables.currentStage >= _task.taskProgress.currentStage;

        if (_endGoal) return this;

        _lerpGoal = _variables.currentProgress + (_task.taskProgress.totalProgress - _variables.currentProgress);
        _lerpTime = .1f + (_lerpGoal - _variables.currentProgress) * .5f;

        _uiTaskAnimation = UIAnimation;
        Helpers.PersistantData.tasks.SetUITaskProgress(_task.ID, _variables);
        return this;
    }
    private void Update()
    {
        _uiTaskAnimation?.Invoke();
    }

    float _timeElapsed = 0;
    void UIAnimation()
    {
        _endGoal = _variables.currentProgress >= _task.taskProgress.progress && _variables.currentStage >= _task.taskProgress.currentStage;

        _timeElapsed += Time.deltaTime;
        _variables.currentProgress = Mathf.Lerp(_variables.currentProgress, _lerpGoal, _animationCurve.Evaluate(_timeElapsed / _lerpTime));

        _progressBar.fillAmount = _variables.currentProgress / _variables.currentStageGoal;
        _taskCurrentProgress.text = $"{(int)_variables.currentProgress}/ {_variables.currentStageGoal}";

        if (_endGoal)
        {
            if (_variables.currentProgress >= _task.stages.Last())
            {
                _variables.completed = true;
                StartCoroutine(PlayAnimation());
            }
            Helpers.PersistantData.tasks.SetUITaskProgress(_task.ID, _variables);
            _uiTaskAnimation = null;
            return;
        }

        if (_variables.currentProgress >= _variables.currentStageGoal && _variables.currentStage != _task.taskProgress.currentStage)
        {
            _lerpGoal -= _variables.currentStageGoal;
            _variables.currentStage++;
            _variables.currentStageGoal = _task.stages[_variables.currentStage];
            _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
            _variables.currentProgress = 0;
            _taskGoldenBaldAward.text = _task.goldenBaldCoinsAward[_variables.currentStage].ToString();
            _taskPresiCoinAward.text = _task.presiCoinsAward[_variables.currentStage].ToString();
            _task.ReclaimMision();
            _taskUIManager.UpdateCoins();
        }
    }
    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1f);
        _animator.Play("NoteTakeOff");
    }
    void ReclaimButton()
    {
        _task.ReclaimMision();
        SetStats();
        _taskUIManager.UpdateCoins();
    }
}
