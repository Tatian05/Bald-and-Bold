using UnityEngine;
using TMPro;
public class TextToTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    [SerializeField] TextMeshProUGUI _myView;

    public string ID { get { return _ID; } set { _ID = value; } }
    private void Start()
    {
        LanguageManager.Instance.OnUpdate += ChangeLang;
        ChangeLang();
    }
    private void OnEnable()
    {
        ChangeLang();
    }
    void ChangeLang()
    {
        string text = LanguageManager.Instance.GetTranslate(_ID);
        _myView.text = text;
    }
    private void OnDisable()
    {
        LanguageManager.Instance.OnUpdate -= ChangeLang;
    }
    private void OnDestroy()
    {
        LanguageManager.Instance.OnUpdate -= ChangeLang;
    }
    public void UpdateText(string id)
    {
        _ID = id;
        string text  = LanguageManager.Instance.GetTranslate(_ID);
        _myView.text = text;
    }
}
