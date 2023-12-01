using System.Linq;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{
    [SerializeField] QuestUINotificationManager _questUINotificationManager;
    TaskSO[] tasks;
    private void Start()
    {
        tasks = Helpers.PersistantData.tasksSO;
        EventManager.SubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
    public void SetProgressInMision(params object[] param)
    {
        var index = Array.IndexOf(tasks, tasks.FirstOrDefault(x => x.taskName.Equals((string)param[0])));
        if (tasks[index].AddProgressAndStageCompleted((int)param[1])) _questUINotificationManager.GetNotification().
                                                      SetQuestName(tasks[index].taskName).
                                                      SetQuestStage(tasks[index].taskProgress.currentStage, tasks[index].stages.Length).
                                                      Init();
    }
    private void OnDestroy()
    {
        if (!enabled) return;
        EventManager.UnSubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
}