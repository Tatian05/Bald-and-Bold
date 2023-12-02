using UnityEngine;
using TMPro;
using DG.Tweening;
public class QuestCompleteNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _questNameTxt, _descriptionTxt;
    public bool canUse { get; private set; } = true;
    public QuestCompleteNotification SetQuestName(string questName)
    {
        _questNameTxt.text = questName;
        return this;
    }
    public QuestCompleteNotification SetQuestStage(bool completed, int stageCompleted, int totalStages)
    {
        var stage = completed ? stageCompleted + 1 : stageCompleted;
        _descriptionTxt.text = $"stage {stage}/ {totalStages} completed";
        return this;
    }
    public QuestCompleteNotification Init()
    {
        canUse = false;
        transform.DOMoveX(200, 2f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo).OnComplete(() => canUse = false);
        return this;
    }
}
