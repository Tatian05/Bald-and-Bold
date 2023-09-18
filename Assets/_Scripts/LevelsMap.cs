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
        foreach (var item in _zonesButtons) item.interactable = false;

        ZonesManager zonesManager = ZonesManager.Instance;
        GameData persistantDataSaved = Helpers.PersistantData.gameData;

        //for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        //{
        //    if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
        //    deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        //}

        var coll = zonesManager.zones.Select(x => x.levelsZone).Take(persistantDataSaved.unlockedZones + 1);
        int actualLevelsZone = 0;

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = persistantDataSaved.levels.Any()  //Chequeo si jugo todos los niveles de la zona
            && persistantDataSaved.currentDeaths <= zonesManager.zones[persistantDataSaved.unlockedZones].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && persistantDataSaved.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && persistantDataSaved.unlockedZones < zonesManager.zones.Length - 1;

        if (canUnlockNewZone) persistantDataSaved.unlockedZones++;

        int deathsAmount = 0;
        for (int i = 0; i <= persistantDataSaved.unlockedZones; i++)            //UPDATE DE MUERTES Y ZONAS
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

        buttonZone.onClick.AddListener(() => Helpers.GameManager.LoadSceneManager.LoadLevel(sceneToLoad));
    }
}