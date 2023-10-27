using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Linq;
using UnityEngine.InputSystem.XInput;
public enum DeviceType { Keyboard, GeneralGamepad, xBoxGamepad }
public class NewInputManager : MonoBehaviour
{
    public static PlayerInputs PlayerInputs;

    public static event Action RebindComplete;
    public static event Action RebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    public static DeviceType activeDevice = DeviceType.Keyboard;

    public static event Action ActiveDeviceChangeEvent;

    static InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private void Awake()
    {
        if (PlayerInputs == null) PlayerInputs = new PlayerInputs();

        foreach (var action in PlayerInputs)
            LoadUserBindings(action.name);
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += TrackActions;
    }
    private void OnDisable()
    {
        InputSystem.onActionChange -= TrackActions;
    }
    private void Start()
    {
        ActiveDeviceChangeEvent?.Invoke();
    }
    public void TrackActions(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            InputAction inputAction = (InputAction)obj;
            InputControl activeControl = inputAction.activeControl;

            var newDevice = DeviceType.Keyboard;

            if (activeControl.device is Keyboard)
                newDevice = DeviceType.Keyboard;

            if (activeControl.device is Gamepad)
                newDevice = activeControl.device is XInputController ? DeviceType.xBoxGamepad : DeviceType.GeneralGamepad;

            if (activeDevice != newDevice)
            {
                activeDevice = newDevice;
                ActiveDeviceChangeEvent?.Invoke();
            }
        }
    }
    public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse)
    {
        InputAction action = PlayerInputs.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                DoRebind(action, bindingIndex, statusText, true, excludeMouse);
        }
        else DoRebind(action, bindingIndex, statusText, false, excludeMouse);
    }
    static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse)
    {
        if (actionToRebind == null || bindingIndex < 0) return;

        statusText.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? "Waiting for input" : "Esperando entrada";

        actionToRebind.Disable();

        rebindingOperation = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebindingOperation.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
            SaveUserBindings(actionToRebind);
            RebindComplete?.Invoke();
        });

        rebindingOperation.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            RebindCanceled?.Invoke();
        });
        rebindingOperation.WithCancelingThrough("/Keyboard/escape").
               WithControlsExcluding("/Gamepad/Start").
               WithCancelingThrough("/Keyboard/escape").
               WithCancelingThrough("/Gamepad/Start");
               //OnPotentialMatch(operation => { Debug.Log(operation.selectedControl.path); if (operation.selectedControl.path is "/Keyboard/escape") rebindingOperation.Cancel(); });

        if (excludeMouse) rebindingOperation.WithControlsExcluding("Mouse");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebindingOperation.Start();
    }
    public static void DisablePlayer() { PlayerInputs.Disable(); }
    public static void EnablePlayer() { PlayerInputs.Enable(); }
    public static InputBinding GetBinding(string actionName, DeviceType deviceType, int compositeBind)
    {
        InputAction action = PlayerInputs.FindAction(actionName);
        bool isCompositeOrPartOfComposite = action.bindings[(int)deviceType].isComposite || action.bindings[(int)deviceType].isPartOfComposite;
        InputBinding toReturn = isCompositeOrPartOfComposite ? action.bindings[compositeBind] : action.bindings[(int)deviceType];
        return toReturn;
    }
    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if (PlayerInputs == null) PlayerInputs = new PlayerInputs();

        InputAction action = PlayerInputs.FindAction(actionName);

        return action.GetBindingDisplayString(bindingIndex);
    }
    static void SaveUserBindings(InputAction action)
    {
        var persistandDataSaved = Helpers.PersistantData.persistantDataSaved;
        for (int i = 0; i < action.bindings.Count; i++)
            persistandDataSaved.AddBinding(action.name + i, action.bindings[i].overridePath);
    }
    public static void LoadUserBindings(string actionName)
    {
        var persistandDataSaved = Helpers.PersistantData.persistantDataSaved;

        if (actionName == null || !persistandDataSaved.userBindingKeys.Any()) return;

        if (PlayerInputs == null) PlayerInputs = new PlayerInputs();

        InputAction action = PlayerInputs.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            var key = action.name + i;
            var bind = persistandDataSaved.GetBind(key);
            if (!string.IsNullOrEmpty(bind))
                action.ApplyBindingOverride(i, bind);
        }
    }
    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = PlayerInputs.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                action.RemoveBindingOverride(i);
        }
        else
            action.RemoveBindingOverride(bindingIndex);
    }
}
