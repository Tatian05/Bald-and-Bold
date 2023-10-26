using UnityEngine.InputSystem;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine;
public static class ReadAndReplaceBinding
{
    static string ACTION_PATTERN = @"\{(.*?)\}";
    static Regex REGEX = new Regex(ACTION_PATTERN, RegexOptions.IgnoreCase);
    public static string ReadAndReplaceString(string textToDisplay, InputBinding actionNeeded, TMP_SpriteAsset spriteAsset)
    {
        string stringButtonName = actionNeeded.effectivePath;
        stringButtonName = RenameInput(stringButtonName, actionNeeded.action);
        textToDisplay = textToDisplay.Replace("BUTTONPROMPT", $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");
        return textToDisplay;
    }


    //ESTA FUNCION ES MEJOR QUE LA DE ARRIBA
    public static string GetSpriteTag(string actionName, DeviceType deviceType, ListOfTmpSpriteAssets spriteAssets, int compositeBind, int extraFrames, string message = "")
    {
        InputBinding dynamicBinding = NewInputManager.GetBinding(actionName, deviceType, compositeBind);
        TMP_SpriteAsset spriteAsset = spriteAssets.spriteAssets[(int)deviceType];

        string stringButtonName = dynamicBinding.effectivePath;
        stringButtonName = RenameInput(stringButtonName, dynamicBinding.action);

        if (spriteAsset == null)
        {
            Debug.Log($"SpriteSheet for {stringButtonName} not found!");
            return message;
        }

        int startFrameIndex = spriteAsset.GetSpriteIndexFromName(stringButtonName + "0");
        return $"<sprite=\"{spriteAsset.name}\" anim=\"{startFrameIndex}, {startFrameIndex + extraFrames}, {3}\">";
    }
    public static string ReplaceActiveBindings(string textWithActions, DeviceType deviceType, ListOfTmpSpriteAssets spritesAssets, int compositeBind, int extraFrames, string message = "")
    {
        return ReplaceBindings(textWithActions, deviceType, spritesAssets, compositeBind, extraFrames, message);
    }
    public static string ReplaceBindings(string textWithActions, DeviceType deviceType, ListOfTmpSpriteAssets spriteAssets, int compositeBind, int extraFrames, string message = "")
    {
        MatchCollection matches = REGEX.Matches(textWithActions);

        var replacedText = textWithActions;

        foreach (Match match in matches)
        {
            var withBraces = match.Groups[0].Captures[0].Value;
            var innerPart = match.Groups[1].Captures[0].Value;

            var tagText = GetSpriteTag(innerPart, deviceType, spriteAssets, compositeBind, extraFrames, message);

            replacedText = replacedText.Replace(withBraces, tagText);
        }

        return replacedText;
    }
    static string RenameInput(string stringButtonName, string actionName)
    {
        stringButtonName = stringButtonName.Replace(actionName, string.Empty);
        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
        stringButtonName = stringButtonName.Replace("[Keyboard&Mouse]", string.Empty);
        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");
        stringButtonName = stringButtonName.Replace("[Gamepad]", string.Empty);
        stringButtonName = stringButtonName.Replace("<XInputController>/", "XInputController_");
        stringButtonName = stringButtonName.Replace("[xBoxController]", string.Empty);
        stringButtonName = stringButtonName.Replace("<Mouse>/", "Mouse_");

        return stringButtonName;
    }
}
