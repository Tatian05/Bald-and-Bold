using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _allButtons;
    [SerializeField] Button[] _zonesButtons;
    [SerializeField] Image[] _buttonsBackground;
    [SerializeField] Button _menuButton, _shopButton, _alarmButton;
    [SerializeField] GameObject[] _deathsZoneGO;
    [SerializeField] Transform _mano;
    [SerializeField] Ease _easeIn, _easeOut;
    [SerializeField] ReplayPanel _replayPanel;

    [SerializeField] Button[] _parButtons;
    [SerializeField] Button[] _unParButtons;
    [SerializeField] GameObject _ascensorParentGO;

    ZonesManager _zonesManager;
    GameData _gameData;
    Vector3 _manoPos;
    public Vector3 ManoPos { get { return _manoPos; } }
    private void Start()
    {
        _manoPos = _mano.position;
        foreach (var item in _zonesButtons) item.interactable = false;

        _zonesManager = ZonesManager.Instance;
        _gameData = Helpers.PersistantData.gameData;

        //for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        //{
        //    if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
        //    deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        //}

        var coll = _zonesManager.zones.Select(x => x.levelsZone).Take(_gameData.unlockedZones + 1);

        //int actualLevelsZone = 0;

        //foreach (var item in coll)
        //    actualLevelsZone += item.Count();

        bool canUnlockNewZone = _gameData.levels.Any()  //Chequeo si jugo todos los niveles de la zona
            && _gameData.currentDeaths <= _zonesManager.zones[_gameData.unlockedZones].deathsNeeded  //Chequeo si murio menos veces que lo requerido
                                                                                                     //&& _gameData.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && _gameData.levelsDeathCompleted.All(x => x.Item3)
            && _gameData.unlockedZones < _zonesManager.zones.Length - 1;

        if (canUnlockNewZone)
        {
            _gameData.unlockedZones++;
            EventManager.TriggerEvent(Contains.MISSION_PROGRESS, "Get Over Zones", _gameData.unlockedZones);
        }

        int deathsAmount = 0;
        for (int i = 0; i <= _gameData.unlockedZones; i++)            //UPDATE DE MUERTES Y ZONAS
        {
            _zonesManager.zones[i].SetCurrentDeaths();
            deathsAmount += _zonesManager.zones[i].currentDeathsInZone;
            SetButton(i, deathsAmount);
            if (_zonesManager.zones[i].currentDeathsInZone > _zonesManager.zones[i].deathsNeeded) break;
        }

        Button[] allSelectables = _allButtons.Where(x => x.interactable).ToArray();

        var buttonsIndex = allSelectables.Zip(allSelectables.Select(x => Array.IndexOf(_allButtons, x)), (button, index) => Tuple.Create(button, index));

        int[] buttonIndexes = allSelectables.Select(x => Array.IndexOf(_allButtons, x)).ToArray();

        _parButtons = buttonsIndex.Where(x => x.Item2 % 2 == 0).Select(x => x.Item1).Skip(1).ToArray();
        _unParButtons = buttonsIndex.Where(x => x.Item2 % 2 != 0).Select(x => x.Item1).ToArray();

        foreach (var item in allSelectables)
        {
            var navigation = item.navigation;
            if (navigation.selectOnRight == null) navigation.selectOnRight = allSelectables.Next(item);
            if (navigation.selectOnLeft == null) navigation.selectOnLeft = allSelectables.Previous(item);

            item.navigation = navigation;
        }

        foreach (var item in _parButtons)
        {
            var navigation = item.navigation;
            if (navigation.selectOnDown == null) navigation.selectOnDown = _parButtons.Next(item);
            if (navigation.selectOnUp == null) navigation.selectOnUp = _parButtons.Previous(item);

            item.navigation = navigation;
        }

        foreach (var item in _unParButtons)
        {
            var navigation = item.navigation;
            if (navigation.selectOnDown == null) navigation.selectOnDown = _unParButtons.Next(item);
            if (navigation.selectOnUp == null) navigation.selectOnUp = _unParButtons.Previous(item);

            item.navigation = navigation;
        }

        if (_deathsZoneGO[_gameData.unlockedZones] == _deathsZoneGO.Last()) return;

        var nextZoneDeaths = _deathsZoneGO.Next(_deathsZoneGO[_gameData.unlockedZones]);
        if (nextZoneDeaths)
        {
            nextZoneDeaths.SetActive(true);
            var text = nextZoneDeaths.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.yellow;
            text.text = $"{deathsAmount} / {_zonesManager.zones.Next(_zonesManager.zones[_gameData.unlockedZones]).deathsNeeded}";
        }

        //_menuButton.GetComponent<LevelSelectButtons>().OnStart();
        //_shopButton.GetComponent<LevelSelectButtons>().OnStart();
        //_alarmButton.GetComponent<LevelSelectButtons>().OnStart();
    }
    public void SetButton(int index, int deathsAmount)
    {
        var zone = _zonesManager.zones[index];
        var button = _zonesButtons[index];

        button.interactable = true;
        _deathsZoneGO[index].SetActive(true);
        _buttonsBackground[index].color = Color.green;

        button.GetComponent<LevelSelectButtons>().enabled = true;
        button.GetComponent<LevelSelectButtons>().BackgroundColor = Color.green;
        button.GetComponent<OnMouseOverButton>().enabled = true;

        var deathZoneText = _deathsZoneGO[index].GetComponentInChildren<TextMeshProUGUI>();

        deathZoneText.text = $"{deathsAmount} / {zone.deathsNeeded}";

        deathZoneText.color = deathsAmount <= zone.deathsNeeded ? Color.green : Color.red;

        button.onClick.AddListener(() =>
        {
            Vector3 manoPos = _mano.position;
            button.interactable = false;
            _mano.DOMove(button.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
            OnComplete(() =>
            {
                button.GetComponent<Animator>().Play("Pressed");

                if (_gameData.zonesPlayed[index])
                {
                    _ascensorParentGO.GetComponent<OnEnableGridButtons>().SetLastSelectedButton(button.gameObject);
                    _mano.DOMove(manoPos, 1f).SetEase(_easeOut);
                    _ascensorParentGO.SetActive(false);
                    _replayPanel.gameObject.SetActive(true);
                    button.interactable = true;
                    _replayPanel.SetZone(zone);
                }
                else
                {
                    _gameData.zonesPlayed[index] = true;
                    _mano.DOMoveY(_mano.transform.position.y - 100, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(zone.levelsZone.First(), true));
                }
            });
        });
    }
    public void MenuButton(string menuSceneName)
    {
        _mano.DOMove(_menuButton.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
        OnComplete(() =>
        {
            _menuButton.GetComponent<Animator>().Play("Pressed");
            _mano.DOMoveY(_mano.transform.position.y - 100f, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(menuSceneName, true));
        });
    }
    public void ShopButton(string shopSceneName)
    {
        _mano.DOMove(_shopButton.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
        OnComplete(() =>
        {
            _shopButton.GetComponent<Animator>().Play("Pressed");
            _mano.DOMoveY(_mano.transform.position.y - 100f, 1f).SetEase(_easeOut).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync(shopSceneName, true));
        });
    }
}