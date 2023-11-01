using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _zonesButtons;
    [SerializeField] TextMeshProUGUI[] _deathsZoneTxt;
    private void Start()
    {
        Helpers.AudioManager.PlayMusic("Elevator Music");
        foreach (var item in _zonesButtons) item.interactable = false;

        ZonesManager zonesManager = ZonesManager.Instance;
        GameData gameData = Helpers.PersistantData.gameData;

        //for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        //{
        //    if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
        //    deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        //}

        var coll = zonesManager.zones.Select(x => x.levelsZone).Take(gameData.unlockedZones + 1);
        int actualLevelsZone = 0;

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = gameData.levels.Any()  //Chequeo si jugo todos los niveles de la zona
            && gameData.currentDeaths <= zonesManager.zones[gameData.unlockedZones].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && gameData.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && gameData.unlockedZones < zonesManager.zones.Length - 1;

        if (canUnlockNewZone)
        {
            gameData.unlockedZones++;
            EventManager.TriggerEvent(Contains.MISSION_PROGRESS, "Get Over Zones", gameData.unlockedZones);
        }

        int deathsAmount = 0;
        for (int i = 0; i <= gameData.unlockedZones; i++)            //UPDATE DE MUERTES Y ZONAS
        {
            zonesManager.zones[i].SetCurrentDeaths();
            deathsAmount += zonesManager.zones[i].currentDeathsInZone;
            SetButton(_deathsZoneTxt[i], _zonesButtons[i], zonesManager.zones[i].deathsNeeded, ref deathsAmount, ZonesManager.Instance.zones[i].levelsZone.First());
            if (zonesManager.zones[i].currentDeathsInZone > zonesManager.zones[i].deathsNeeded) break;
        }
    }
    public void SetButton(TextMeshProUGUI deathsZoneTxt, Button buttonZone, int deathsNeeded, ref int deathsAmount, string sceneToLoad)
    {
        buttonZone.interactable = true;

        deathsZoneTxt.gameObject.SetActive(true);

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;

        buttonZone.onClick.AddListener(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(sceneToLoad, true));
    }
}