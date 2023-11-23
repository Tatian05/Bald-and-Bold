using UnityEngine;
using TMPro;
public class TaskUIManager : MonoBehaviour
{
    [SerializeField] UI_Task _uiTaskPrefab;
    [SerializeField] Transform _tasksContainer;
    [SerializeField] TextMeshProUGUI _goldenBaldTxt, _presiCoinTxt;
    PersistantData _persistantData;
    private void Start()
    {
        _persistantData = Helpers.PersistantData;
        UpdateCoins();

        foreach (var item in _persistantData.tasks.tasks)
            Instantiate(_uiTaskPrefab, _tasksContainer).SetTask(item).SetStats();
    }

    public void UpdateCoins()
    {
        _goldenBaldTxt.text = _persistantData.persistantDataSaved.goldenBaldCoins.ToString();
        _presiCoinTxt.text = _persistantData.persistantDataSaved.presiCoins.ToString();
    }
}

[System.Serializable]
public struct UI_TaskVariables
{
    public int currentStageGoal, currentStage, ID;
    public float currentProgress;
    [HideInInspector] public Vector3 randomRotation;
}
