using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using DG.Tweening;
public class ReplayPanel : MonoBehaviour
{
    [SerializeField] Button[] _levelsButtons;
    [SerializeField] TextMeshProUGUI[] _levelsNumber;
    [SerializeField] TextMeshProUGUI[] _levelsDeaths;
    [SerializeField] Image[] _backgroundButtons;
    [SerializeField] Transform _mano;
    [SerializeField] GameObject _ascensorGO, _backButtonGO;
    [SerializeField] LevelsMap _levelsMap;

    [Header("Gamepad")]
    [SerializeField] string[] _onBackTMP;
    [SerializeField] TextMeshProUGUI _backTxt;
    [SerializeField] GameObject _gamepadContainer;
    [SerializeField] Button _backButton;

    Zone _zone;
    GameData _gameData;
    private void Start()
    {
        _backButton.onClick.AddListener(BackButton);
    }
    private void OnEnable()
    {
        NewInputManager.ActiveDeviceChangeEvent += Gamepad;

        Gamepad();
    }
    private void OnDisable()
    {
        NewInputManager.ActiveDeviceChangeEvent -= Gamepad;
    }
    public ReplayPanel SetZone(Zone zone)
    {
        _gameData = Helpers.PersistantData.gameData;
        _gameData.LoadLevelInfo();
        _zone = zone;

        for (int i = 0; i < _levelsButtons.Length; i++)
        {
            var levelName = _zone.levelsZone[i];
            var button = _levelsButtons[i];
            var levelButton = button.GetComponent<LevelSelectButtons>();
            button.interactable = _gameData.levels.Contains(levelName);

            _levelsNumber[i].text = _zone.levelsZone[i].Replace("Level ", "");
            if (!button.interactable)
            {
                _backgroundButtons[i].color = Color.red;
                continue;
            }

            if (i != 0) _backgroundButtons[i].color = Color.green;

            levelButton.enabled = true;
            levelButton.BackgroundColor = Color.green;
            button.GetComponent<OnMouseOverButton>().enabled = true;

            _levelsDeaths[i].text = _gameData.GetDeathsInLevel(_zone.levelsZone[i]).ToString();

            button.onClick.AddListener(() =>
            {
                _mano.DOMove(button.transform.position - new Vector3(0, .5f), 1f).SetEase(Ease.OutSine).
                OnComplete(() =>
                {
                    button.GetComponent<Animator>().Play("Pressed");
                    _mano.DOMoveY(_mano.position.y - 100, .5f).OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadAsyncFadeOut(levelName));
                });
            });
        }

        SetNavigation();

        return this;
    }

    void SetNavigation()
    {
        Button beforeButton = null;
        var buttonsFiltered = _levelsButtons.Where(x => x.interactable);
        foreach (var item in buttonsFiltered)
        {
            var navigation = item.navigation;

            navigation.selectOnLeft = beforeButton ? beforeButton : buttonsFiltered.Last();
            navigation.selectOnRight = buttonsFiltered.Next(item) ? buttonsFiltered.Next(item) : buttonsFiltered.First();

            beforeButton = item;
            item.navigation = navigation;
        }
    }
    void BackButton()
    {
        _mano.DOMove(_backButton.transform.position - new Vector3(0f, .5f), .5f).SetEase(Ease.OutSine).
        OnComplete(() =>
        {
            _backButton.GetComponent<Animator>().Play("Pressed");
            gameObject.SetActive(false);
            _ascensorGO.SetActive(true);
            _mano.DOMove(_levelsMap.ManoPos, .5f).SetEase(Ease.OutQuart);
        });
    }
    void Gamepad()
    {
        if (NewInputManager.activeDevice != DeviceType.Keyboard)
        {
            _gamepadContainer.SetActive(true);
            _backButtonGO.SetActive(false);

            _backTxt.text = _onBackTMP[(int)NewInputManager.activeDevice - 1];
        }
        else
        {
            _gamepadContainer.SetActive(false);
            _backButtonGO.SetActive(true);
        }
    }
}
