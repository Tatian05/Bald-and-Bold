using UnityEngine;
using TMPro;
public class RebindButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _buttonText;
    public void OnStartRebind() { _buttonText.color = Color.grey; }
    public void OnStopRebind() { _buttonText.color = Color.white; }
}
