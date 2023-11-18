using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Task : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _taskName, _taskDescription, _taskStage, _taskCurrentProgress, _taskGoldenBaldAward, _taskPresiCoinAward;
    [SerializeField] Image _backgroundImg, _progressBar;
    [SerializeField] Button _reclaimButton;

    [SerializeField] Task _task;
    TaskUIManager _taskUIManager;
    Color _noteColor;
    private void Start()
    {
        _reclaimButton.onClick.AddListener(ReclaimButton);
        _taskUIManager = GetComponentInParent<TaskUIManager>();
    }
    public UI_Task SetTask(Task task)
    {
        _task = task;
        _reclaimButton.interactable = _task.CanReclaimMision();
        if (_noteColor == default) _noteColor = Random.ColorHSV();
        return this;
    }
    public UI_Task SetTaskStats()
    {
        _taskName.text = _task.taskName;
        _taskDescription.text = _task.taskDescription;
        _taskStage.text = $"{_task.currentStageIndex + 1}/ {_task.stages.Length}";
        _taskCurrentProgress.text = $"{_task.progress}/ {_task.stages[_task.currentStageIndex]}";
        _taskGoldenBaldAward.text = _task.goldenBaldCoinsAward[_task.currentStageIndex].ToString();
        _taskPresiCoinAward.text = _task.presiCoinsAward[_task.currentStageIndex].ToString();

        _progressBar.fillAmount = (float)(_task.progress / (float)_task.stages[_task.currentStageIndex]);
        _backgroundImg.color = _noteColor;
        return this;
    }
    void ReclaimButton()
    {
        _task.ReclaimMision();
        SetTaskStats();
        _taskUIManager.UpdateCoins();
    }
}
