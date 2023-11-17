using UnityEngine;
using TMPro;
public class TaskUIManager : MonoBehaviour
{
    Task[] _totalTasks;
    [SerializeField] UI_Task _uiTaskPrefab;
    [SerializeField] Transform _tasksContainer;
    [SerializeField] TextMeshProUGUI _goldenBaldTxt, _presiCoinTxt;
    PersistantData _persistantData;
    private void Start()
    {
        _persistantData = Helpers.PersistantData;
        _totalTasks = _persistantData.tasks.tasks;
        UpdateCoins();
        foreach (var item in _totalTasks)
            Instantiate(_uiTaskPrefab, _tasksContainer).SetTask(item).SetTaskStats();
    }

    public void UpdateCoins()
    {
        _goldenBaldTxt.text = _persistantData.persistantDataSaved.goldenBaldCoins.ToString();
        _presiCoinTxt.text = _persistantData.persistantDataSaved.presiCoins.ToString();
    }
}
