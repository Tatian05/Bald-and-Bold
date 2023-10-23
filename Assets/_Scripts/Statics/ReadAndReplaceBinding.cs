using UnityEngine.InputSystem;
using TMPro;
using System.Text.RegularExpressions;

public static class ReadAndReplaceBinding
{
    static string ACTION_PATTERN = @"\{(.*)\}";
    static Regex REGEX = new Regex(ACTION_PATTERN, RegexOptions.IgnoreCase);
    public static string ReadAndReplaceString(string textToDisplay, InputBinding actionNeeded, TMP_SpriteAsset spriteAsset)
    {
        string stringButtonName = actionNeeded.effectivePath;
        stringButtonName = RenameInput(stringButtonName);
        textToDisplay = textToDisplay.Replace("BUTTONPROMPT", $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");
        return textToDisplay;
    }


    //ESTA FUNCION ES MEJOR QUE LA DE ARRIBA
    public static string GetSpriteTag(string actionName, DeviceType deviceType, ListOfTmpSpriteAssets spriteAssets)
    {
        InputBinding dynamicBinding = NewInputManager.GetBinding(actionName, deviceType);

        TMP_SpriteAsset spriteAsset = spriteAssets.spriteAssets[(int)deviceType];

        string stringButtonName = dynamicBinding.effectivePath;
        stringButtonName = RenameInput(stringButtonName);

        return $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">";
    }
    public static string ReplaceActiveBindings(string textWithActions, ListOfTmpSpriteAssets spritesAssets)
    {
        return ReplaceBindings(textWithActions, NewInputManager.activeDevice, spritesAssets);
    }
    public static string ReplaceBindings(string textWithActions, DeviceType deviceType, ListOfTmpSpriteAssets spriteAssets)
    {
        MatchCollection matches = REGEX.Matches(textWithActions);

        var replacedText = textWithActions;

        foreach (Match match in matches)
        {
            var withBraces = match.Groups[0].Captures[0].Value;
            var innerPart = match.Groups[1].Captures[0].Value;

            var tagText = GetSpriteTag(innerPart, deviceType, spriteAssets);

            replacedText = replacedText.Replace(withBraces, tagText);
        }

        return replacedText;
    }
    static string RenameInput(string stringButtonName)
    {
        stringButtonName = stringButtonName.Replace("Interact:", string.Empty);
        stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
        stringButtonName = stringButtonName.Replace("[Keyboard&Mouse]", string.Empty);
        stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");
        stringButtonName = stringButtonName.Replace("[Gamepad]", string.Empty);

        return stringButtonName;
    }
}
