using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class ButtonsNavigate : MonoBehaviour
{
    [SerializeField] Button[] _uiButtons;
    void Start()
    {
        Invoke(nameof(Buttons), .01f);
    }
    void Buttons()
    {
        _uiButtons = GetComponentsInChildren<Button>().Where(x => x.gameObject.activeSelf).ToArray();

        Button beforeButton = null;

        foreach (var button in _uiButtons)
        {
            Navigation navigation = button.navigation;
            navigation.selectOnDown = _uiButtons.Next(button);
            navigation.selectOnUp = beforeButton ? beforeButton : _uiButtons.Last();

            button.navigation = navigation;
            beforeButton = button;
        }
    }
}
