using System.Linq;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{
    [SerializeField] Mission[] _misions;

    PersistantDataSaved _persistantDataSaved;
    private void OnEnable()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        if (_persistantDataSaved.misions.Any()) _misions = _persistantDataSaved.misions;

        EventManager.SubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
    public void SetProgressInMision(params object[] param)
    {
        var quest = _misions.FirstOrDefault(x => x.questName.Equals((string)param[0]));
        _misions[Array.IndexOf(_misions, quest)].AddProgress((int)param[1]);
    }
    void SetMisions()
    {
        Helpers.PersistantData.persistantDataSaved.misions = _misions;
    }
    private void OnDestroy()
    {
        SetMisions();
        EventManager.UnSubscribeToEvent(Contains.MISSION_PROGRESS, SetProgressInMision);
    }
}

[System.Serializable]
public struct Mission
{
    public string questName;
    public int progress;
    public int[] stages;
    public int[] presiCoinsAward;
    public int[] goldenBaldCoinsAward;
    [HideInInspector] public int currentStageIndex;
    public void AddProgress(int amount) { progress += amount; }
    public bool CanReclaimMision() => progress >= stages[currentStageIndex];
    public void ReclaimMision()
    {
        Helpers.PersistantData.persistantDataSaved.presiCoins += presiCoinsAward[currentStageIndex];
        Helpers.PersistantData.persistantDataSaved.goldenBaldCoins += goldenBaldCoinsAward[currentStageIndex];
        currentStageIndex++;
    }
}