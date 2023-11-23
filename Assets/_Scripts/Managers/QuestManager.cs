using System.Linq;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestUINotificationManager _questUINotificationManager;

    PersistantData _persistantData;
    [SerializeField] Task[] _tasks;
    private void Start()
    {
        _persistantData = Helpers.PersistantData;
        if (_persistantData.tasks.tasks != null && _persistantData.tasks.tasks.Any()) _tasks = _persistantData.tasks.tasks;

        EventManager.SubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
    public void SetProgressInMision(params object[] param)
    {
        var index = Array.IndexOf(_tasks, _tasks.FirstOrDefault(x => x.taskName.Equals((string)param[0])));
        if (_tasks[index].AddProgressAndStageCompleted((int)param[1])) _questUINotificationManager.GetNotification().
                                                      SetQuestName(_tasks[index].taskName).
                                                      SetQuestStage(_tasks[index].currentStageIndex - 1, _tasks[index].stages.Length).
                                                      Init();
    }
    void SetMisions()
    {
        Helpers.PersistantData.tasks.tasks = _tasks;
    }
    private void OnDestroy()
    {
        if (!enabled) return;
        SetMisions();
        EventManager.UnSubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
}

[Serializable]
public struct Task
{
    public string taskName, taskDescription;
    public int progress, totalProgress;
    public int[] stages;
    public int[] presiCoinsAward;
    public int[] goldenBaldCoinsAward;
    public int currentStageIndex;
    public Color noteColor;
    public bool AddProgressAndStageCompleted(int amount)
    {
        progress += amount;
        totalProgress += amount;
        if (progress >= stages[currentStageIndex])
        {
            var diff = progress - stages[currentStageIndex];
            progress += diff;
            currentStageIndex++;
            return true;
        }
        return false;
    }
    public bool CanReclaimMision() => progress >= stages[currentStageIndex];
    public void ReclaimMision()
    {
        Helpers.PersistantData.persistantDataSaved.presiCoins += presiCoinsAward[currentStageIndex];
        Helpers.PersistantData.persistantDataSaved.goldenBaldCoins += goldenBaldCoinsAward[currentStageIndex];
        currentStageIndex++;
    }
    public float GetStageProgress() => progress / stages[currentStageIndex];
}