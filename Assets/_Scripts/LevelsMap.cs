using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _allButtons;
    [SerializeField] Button[] _zonesButtons;
    [SerializeField] Image[] _buttonsBackground;
    [SerializeField] Button _menuButton, _shopButton;
    [SerializeField] Image _menuBackground, _shopBackground;
    [SerializeField] GameObject[] _deathsZoneGO;
    [SerializeField] Transform _mano;
    [SerializeField] Ease _easeIn, _easeOut;
    [SerializeField] Color _buttonsBackgroundColor;


    [SerializeField] List<Button> _parButtons;
    [SerializeField] List<Button> _unParButtons;
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
            SetButton(_deathsZoneGO[i], _zonesButtons[i], zonesManager.zones[i].deathsNeeded, ref deathsAmount, ZonesManager.Instance.zones[i].levelsZone.First(), _buttonsBackground[i]);
            if (zonesManager.zones[i].currentDeathsInZone > zonesManager.zones[i].deathsNeeded) break;
        }

        Button[] allSelectables = _allButtons.Where(x => x.interactable).ToArray();
        _unParButtons = allSelectables.Where((elemento, indice)=> indice % 2 == 0).ToList();
        _parButtons = allSelectables.Where((elemento, indice) => indice % 2 != 0).ToList();

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
    }
    public void SetButton(GameObject deathsZoneGO, Button buttonZone, int deathsNeeded, ref int deathsAmount, string sceneToLoad, Image buttonBackground)
    {
        buttonZone.interactable = true;

        deathsZoneGO.SetActive(true);
        buttonBackground.color = Color.green;
        var deathZoneText = deathsZoneGO.GetComponentInChildren<TextMeshProUGUI>();

        deathZoneText.text = $"{deathsAmount} / {deathsNeeded}";

        deathZoneText.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;

        buttonZone.onClick.AddListener(() =>
        {
            buttonZone.interactable = false;
            _mano.DOMove(buttonZone.transform.position - new Vector3(0f, .5f), 1f).SetEase(_easeIn).
            OnComplete(() =>
            {
                buttonZone.GetComponent<Animator>().Play("Pressed");
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