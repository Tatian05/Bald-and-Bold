using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "New Task")]
[System.Serializable]
public class TaskSO : ScriptableObject
{
    public TaskProgress taskProgress;

    [Header("Persistant data")]
    public string taskName;
    public string taskDescription;
    public int[] stages;
    public int[] presiCoinsAward;
    public int[] goldenBaldCoinsAward;
    public int ID;
    public Color noteColor;
    public bool AddProgressAndStageCompleted(int amount)
    {
        if (taskProgress.completed) return false;
        taskProgress.progress += amount;
        taskProgress.totalProgress += amount;
        if (taskProgress.progress >= stages[taskProgress.currentStage])
        {
            if (taskProgress.currentStage >= stages.Length - 1)
            {
                taskProgress.completed = true;
                return true;
            };
            var diff = taskProgress.progress - stages[taskProgress.currentStage];
            taskProgress.progress = 0;
            taskProgress.progress += diff;
            taskProgress.currentStage++;
            return true;
        }
        return false;
    }
    public bool CanReclaimMision() => taskProgress.progress >= stages[taskProgress.currentStage];
    public void ReclaimMision(int presiCoinsReward, int goldenBaldCoinsReward)
    {
        Helpers.PersistantData.persistantDataSaved.presiCoins += presiCoinsReward;
        Helpers.PersistantData.persistantDataSaved.goldenBaldCoins += goldenBaldCoinsReward;
    }
}

[System.Serializable]
public struct TaskProgress
{
    public int progress, totalProgress, currentStage;
    public bool completed;
}
