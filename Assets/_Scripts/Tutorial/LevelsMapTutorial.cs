using UnityEngine;
using TMPro;
using DG.Tweening;
public class LevelsMapTutorial : MonoBehaviour
{
    [SerializeField] LevelsMapTutorialData[] _tutorialsData;
    [SerializeField] float _typingSpeed;
    [SerializeField] GameObject _eventSystemGO;
    int _index;
    void Start()
    {
        if (!Helpers.PersistantData.gameData.firstTimeLevelsMap)
            Destroy(gameObject);

        Helpers.PersistantData.gameData.firstTimeLevelsMap = false;
        Invoke(nameof(InvokeTuto), .75f);
    }
    void InvokeTuto()
    {
        _tutorialsData[_index].Set(_typingSpeed);
        _eventSystemGO.SetActive(false);
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!_tutorialsData[_index].finish)
            {
                _tutorialsData[_index].Complete();
                return;
            }

            if (_index + 1 >= _tutorialsData.Length)
            {
                _eventSystemGO.SetActive(true);
                Destroy(gameObject);
                return;
            }

            _tutorialsData[_index].DisableGO();
            _index++;
            _tutorialsData[_index].Set(_typingSpeed);
        }
    }
}

[System.Serializable]
public class LevelsMapTutorialData
{
    public string ID;
    public GameObject _window;
    public TextMeshProUGUI _descriptionTMPRO;
    public bool finish;
    string description;
    Tween _tween;
    public void Set(float typingSpeed)
    {
        description = LanguageManager.Instance.GetTranslate(ID).Replace("-", ",").Replace("/", "\"");
        _window.SetActive(true);
        var text = "";
        _tween = DOTween.To(() => text, x => x = text = x, description, description.Length / typingSpeed).
        SetEase(Ease.Linear).
        OnUpdate(() => _descriptionTMPRO.text = text).
        OnComplete(() => finish = true);
    }
    public void Complete()
    {
        _tween.Kill();
        _descriptionTMPRO.text = description;
        finish = true;
    }
    public void DisableGO()
    {
        _window.SetActive(false);
    }
}