using UnityEngine;
using TMPro;
using DG.Tweening;
public class LevelsMapTutorial : MonoBehaviour
{
    [SerializeField] LevelsMapTutorialData[] _tutorialsData;
    [SerializeField] float _typingSpeed;
    int _index;
    void Start()
    {
        if (!Helpers.PersistantData.gameData.firstTimeLevelsMap) Destroy(gameObject);
        Helpers.PersistantData.gameData.firstTimeLevelsMap = false;

        Invoke(nameof(InvokeTuto), 1f);
    }
    void InvokeTuto() { _tutorialsData[_index].Set(_typingSpeed); }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!_tutorialsData[_index].finish) return;

            if (_index + 1 >= _tutorialsData.Length)
            {
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
    public void Set(float typingSpeed)
    {
        description = LanguageManager.Instance.GetTranslate(ID).Replace("-", ",").Replace("/", "\"");
        _window.SetActive(true);
        var text = "";
        DOTween.To(() => text, x => x = text = x, description, description.Length / typingSpeed).
        SetEase(Ease.Linear).
        OnUpdate(() => _descriptionTMPRO.text = text).
        OnComplete(() => finish = true);
    }
    public void DisableGO()
    {
        _window.SetActive(false);
    }
}