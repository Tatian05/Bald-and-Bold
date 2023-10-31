using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    [SerializeField] Mission[] _misions;

    Dictionary<string, Mission> _questsCollection = new Dictionary<string, Mission>();
    PersistantDataSaved _persistantDataSaved;
    private void Start()
    {
        _persistantDataSaved = Helpers.PersistantData.persistantDataSaved;
        if (_persistantDataSaved.misions.Any()) _misions = _persistantDataSaved.misions;

        foreach (var item in _misions)
            _questsCollection.Add(item.questName, item);
    }
    public void SetProgressInMision(string questName, ref int amount) { _misions.First(x => x.questName.Equals(questName)).AddProgress(ref amount); Debug.Log(_questsCollection[questName].questName); }
    void SetMisions()
    {
        Helpers.PersistantData.persistantDataSaved.misions = _misions;
    }
    private void OnDestroy()
    {
        SetMisions();
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
    public void AddProgress(ref int amount) { progress += amount; Debug.Log(progress); }
    public bool CanReclaimMision() => progress >= stages[currentStageIndex];
    public void ReclaimMision()
    {
        Helpers.PersistantData.persistantDataSaved.presiCoins += presiCoinsAward[currentStageIndex];
        Helpers.PersistantData.persistantDataSaved.baldCoins += goldenBaldCoinsAward[currentStageIndex];
        currentStageIndex++;
    }
}