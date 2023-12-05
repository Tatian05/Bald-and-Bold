using UnityEngine;
public class SetTextToBoxText
{
    [SerializeField] ListOfTmpSpriteAssets _listOfTmpSpriteAssets;
    public SetTextToBoxText(ListOfTmpSpriteAssets listOfTmpSpriteAssets)
    {
        _listOfTmpSpriteAssets = listOfTmpSpriteAssets;
    }
    public string SetText(string actionName, DeviceType deviceType, int extraFrames, int startFrame = 0, int actionCompositeIndex = 0, int fontSize = 2, string message = "")
    {
        int currentDevice = (int)deviceType;
        if (currentDevice > _listOfTmpSpriteAssets.spriteAssets.Count - 1)
            Debug.Log($"Missing Sprite Asset for {deviceType}");

        return ReadAndReplaceBinding.ReplaceActiveBindings("{Player/" + actionName + "}", deviceType, _listOfTmpSpriteAssets, startFrame, actionCompositeIndex,extraFrames, fontSize, message);
    }
}
