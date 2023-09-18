using System.Collections;
using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    float _count;
    [SerializeField] TextMeshProUGUI _text;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    IEnumerator Start()
    {
        var waitForSeconds = new WaitForSeconds(.1f);
        GUI.depth = 2;
        while (true)
        {
            _count = 1f / Time.unscaledDeltaTime;
            _text.text = Mathf.RoundToInt(_count).ToString();
            yield return waitForSeconds;
        }
    }
}
