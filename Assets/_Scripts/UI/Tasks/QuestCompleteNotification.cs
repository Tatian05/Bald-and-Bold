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
    public QuestCompleteNotification SetQuestStage(int stageCompleted, int totalStages)
    {
        _descriptionTxt.text = $"stage {stageCompleted + 1}/ {totalStages} completed";
        return this;
    }
    public QuestCompleteNotification Init()
    {
        canUse = false;
        transform.DOMoveX(200, 2f).SetEase(Ease.OutBack).SetLoops(2, LoopType.Yoyo).OnComplete(() => canUse = false);
        return this;
    }
}
