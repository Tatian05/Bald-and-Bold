using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "New Task")]
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
        taskProgress.progress += amount;
        taskProgress.totalProgress += amount;
        if (taskProgress.progress >= stages[taskProgress.currentStage])
        {
            if (taskProgress.currentStage >= stages.Length - 1) return true;
            var diff = taskProgress.progress - stages[taskProgress.currentStage];
            taskProgress.progress = 0;
            taskProgress.progress += diff;
            taskProgress.currentStage++;
            return true;
        }
        return false;
    }
    public bool CanReclaimMision() => taskProgress.progress >= stages[taskProgress.currentStage];
    public void ReclaimMision()
    {
        Helpers.PersistantData.persistantDataSaved.presiCoins += presiCoinsAward[taskProgress.currentStage];
        Helpers.PersistantData.persistantDataSaved.goldenBaldCoins += goldenBaldCoinsAward[taskProgress.currentStage];
        taskProgress.currentStage++;
    }
}

[System.Serializable]
public struct TaskProgress
{
    public int progress, totalProgress, currentStage;
}
