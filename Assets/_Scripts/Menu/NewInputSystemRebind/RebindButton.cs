using UnityEngine;
using UnityEngine.UI;
public class RebindButton : MonoBehaviour
{
    [SerializeField] Image _buttonIMG;
    public void OnStartRebind() { _buttonIMG.color = Color.grey; }
    public void OnStopRebind() { _buttonIMG.color = Color.white; }
}
