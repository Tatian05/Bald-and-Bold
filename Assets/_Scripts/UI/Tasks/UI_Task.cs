using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UI_Task : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _taskName, _taskDescription, _taskStage, _taskCurrentProgress, _taskGoldenBaldAward, _taskPresiCoinAward;
    [SerializeField] Image _backgroundImg, _progressBar;
    [SerializeField] Button _reclaimButton;
    [SerializeField] Task _task;

    TaskUIManager _taskUIManager;
    UI_TaskVariables _variables;

    Vector3 _randomRot;
    private void Start()
    {
        _reclaimButton.onClick.AddListener(ReclaimButton);
        _taskUIManager = GetComponentInParent<TaskUIManager>();

        Invoke(nameof(SetTaskStats), 1f);

        Helpers.PersistantData.tasks.SetTaskProgress(_variables.ID, _task, _variables);
    }
    public UI_Task SetTask(Task task)
    {
        _task = task;
        _variables = Helpers.PersistantData.tasks.GetTaskProgress(_task);
        _reclaimButton.interactable = _task.CanReclaimMision();
        if(_variables.randomRotation == Vector3.zero) _variables.randomRotation = new Vector3(0, 0, Random.Range(-10, 11));
        transform.eulerAngles = _variables.randomRotation;
        return this;
    }
    public void SetTaskStats()
    {
        _taskName.text = _task.taskName;
        _taskDescription.text = _task.taskDescription;
        _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
        _taskCurrentProgress.text = $"{_variables.currentProgress}/ {_variables.currentStageGoal}";
        _taskGoldenBaldAward.text = _task.goldenBaldCoinsAward[_task.currentStageIndex].ToString();
        _taskPresiCoinAward.text = _task.presiCoinsAward[_task.currentStageIndex].ToString();

        _backgroundImg.color = _task.noteColor;
        _progressBar.fillAmount = (float)_variables.currentProgress / (float)_variables.currentStageGoal;
        float currentProgress = _variables.currentProgress;
        var tweenTime = (_task.progress - currentProgress) / 10;

        if (tweenTime <= 0) return;

        DOTween.To(() => _variables.currentProgress, x => x = _variables.currentProgress = x, _task.progress, tweenTime).SetEase(Ease.InOutQuart).OnUpdate(() =>
        {
            if (_variables.currentProgress >= _variables.currentStageGoal)
            {
                _variables.currentStage++;
                _variables.currentStageGoal = _task.stages[_variables.currentStage];
                _taskStage.text = $"{_variables.currentStage + 1}/ {_task.stages.Length}";
            }
            _taskCurrentProgress.text = $"{_variables.currentProgress}/ {_variables.currentStageGoal}";
            _progressBar.fillAmount = (float)_variables.currentProgress / _variables.currentStageGoal;
        });
    }
    void ReclaimButton()
    {
        _task.ReclaimMision();
        SetTaskStats();
        _taskUIManager.UpdateCoins();
    }
}
