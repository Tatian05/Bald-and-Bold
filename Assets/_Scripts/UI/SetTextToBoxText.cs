using TMPro;
using UnityEngine;
public class SetTextToBoxText 
{
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    [SerializeField] TextMeshProUGUI _textBox;
    int _actionCompositeIndex;
    public SetTextToBoxText(ListOfTmpSpriteAssets listOfTmpSpriteAssets, TextMeshProUGUI textBox, int actionCompositeIndex = 0)
    {
        _listOfTmpSpriteAssets = listOfTmpSpriteAssets;
        _textBox = textBox;
        _actionCompositeIndex = actionCompositeIndex;
    }
    public void SetText(string actionName)
    {
        int currentDevice = (int)NewInputManager.activeDevice;

        if (currentDevice > _listOfTmpSpriteAssets.spriteAssets.Count - 1)
            Debug.Log($"Missing Sprite Asset for {NewInputManager.activeDevice}");

        _textBox.text = ReadAndReplaceBinding.ReplaceActiveBindings("{Player/" + actionName + "}", _listOfTmpSpriteAssets, _actionCompositeIndex);
    }
}
