using System.Collections;
using UnityEngine;
using TMPro;
public class TutorialVillainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDispay;
    [SerializeField] float _typingSpeed;
    string _sentence;
    private void Start()
    {
        StartCoroutine(Type());
    }
    IEnumerator Type()
    {
        var wait = new WaitForSeconds(_typingSpeed);
        yield return new WaitUntil(() => _textDispay.text != string.Empty);
        _sentence = _textDispay.text.Replace("-", ",");
        _textDispay.text = string.Empty;
        foreach (char letter in _sentence)
        {
            _textDispay.text += letter;
            yield return wait;
        }
    }
}
