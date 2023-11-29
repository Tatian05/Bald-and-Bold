using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _zonesButtons;
    [SerializeField] Image[] _buttonsBackground;
    [SerializeField] Button _menuButton, _shopButton;
    [SerializeField] Image _menuBackground, _shopBackground;
    [SerializeField] TextMeshProUGUI[] _deathsZoneTxt;
    [SerializeField] Transform _mano;
    [SerializeField] Ease _easeIn, _easeOut;
    [SerializeField] Color _buttonsBackgroundColor;
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
            SetButton(_deathsZoneTxt[i], _zonesButtons[i], zonesManager.zones[i].deathsNeeded, ref deathsAmount, ZonesManager.Instance.zones[i].levelsZone.First(), _buttonsBackground[i]);
            if (zonesManager.zones[i].currentDeathsInZone > zonesManager.zones[i].deathsNeeded) break;
        }
    }
    public void SetButton(TextMeshProUGUI deathsZoneTxt, Button buttonZone, int deathsNeeded, ref int deathsAmount, string sceneToLoad, Image buttonBackground)
    {
        buttonZone.interactable = true;

        deathsZoneTxt.gameObject.SetActive(true);

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;

        buttonZone.onClick.AddListener(() =>
        {
            _mano.DOMove(buttonZone.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
            OnComplete(() =>
            {
                buttonBackground.color = _buttonsBackgroundColor;
                _mano.DOMoveY(_mano.transform.position.y - 100f, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(sceneToLoad, true));
            });
        });
    }
    public void MenuButton(string MenuSceneName)
    {
        _mano.DOMove(_menuButton.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
        OnComplete(() =>
        {
            _menuBackground.color = _buttonsBackgroundColor;
            _mano.DOMoveY(_mano.transform.position.y - 100f, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(MenuSceneName, true));
        });
    }
    public void ShopButton(string ShopSceneName)
    {
        _mano.DOMove(_shopButton.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
        OnComplete(() =>
        {
            _shopBackground.color = _buttonsBackgroundColor;
            _mano.DOMoveY(_mano.transform.position.y - 100f, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(ShopSceneName, true));
        });
    }
}