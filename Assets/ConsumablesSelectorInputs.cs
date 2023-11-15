//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/ConsumablesSelectorInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ConsumablesSelectorInputs : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ConsumablesSelectorInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ConsumablesSelectorInputs"",
    ""maps"": [
        {
            ""name"": ""ConsumableSelector"",
            ""id"": ""1c61f787-dcad-4c0b-8cd3-0843a79adef0"",
            ""actions"": [
                {
                    ""name"": ""TriggerConsumable"",
                    ""type"": ""Button"",
                    ""id"": ""3c0f0c1c-2c71-4840-ac0f-3c4b4f4e09c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveNext"",
                    ""type"": ""Button"",
                    ""id"": ""8ba4e9cf-c166-4975-b5c6-9a3badf27c7a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveBefore"",
                    ""type"": ""Button"",
                    ""id"": ""f8b499d2-210a-4839-bdce-7585a708d7b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f3e25214-6be3-432d-9c2d-a45f4845eefe"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TriggerConsumable"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05e981f3-f241-4bd0-88ce-fc89c0994099"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""TriggerConsumable"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e6eb7f4-5dca-4636-be03-52d9bc9fd728"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4fb44870-a4de-4f11-bfc4-685f8da6ad8e"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""MoveNext"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7df802b-e078-4618-845a-89b348a7c20c"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveBefore"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f716fd4b-9daa-489d-aaf6-5f55a02caf66"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""xBoxController"",
                    ""action"": ""MoveBefore"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""xBoxController"",
            ""bindingGroup"": ""xBoxController"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // ConsumableSelector
        m_ConsumableSelector = asset.FindActionMap("ConsumableSelector", throwIfNotFound: true);
        m_ConsumableSelector_TriggerConsumable = m_ConsumableSelector.FindAction("TriggerConsumable", throwIfNotFound: true);
        m_ConsumableSelector_MoveNext = m_ConsumableSelector.FindAction("MoveNext", throwIfNotFound: true);
        m_ConsumableSelector_MoveBefore = m_ConsumableSelector.FindAction("MoveBefore", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // ConsumableSelector
    private readonly InputActionMap m_ConsumableSelector;
    private IConsumableSelectorActions m_ConsumableSelectorActionsCallbackInterface;
    private readonly InputAction m_ConsumableSelector_TriggerConsumable;
    private readonly InputAction m_ConsumableSelector_MoveNext;
    private readonly InputAction m_ConsumableSelector_MoveBefore;
    public struct ConsumableSelectorActions
    {
        private @ConsumablesSelectorInputs m_Wrapper;
        public ConsumableSelectorActions(@ConsumablesSelectorInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @TriggerConsumable => m_Wrapper.m_ConsumableSelector_TriggerConsumable;
        public InputAction @MoveNext => m_Wrapper.m_ConsumableSelector_MoveNext;
        public InputAction @MoveBefore => m_Wrapper.m_ConsumableSelector_MoveBefore;
        public InputActionMap Get() { return m_Wrapper.m_ConsumableSelector; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ConsumableSelectorActions set) { return set.Get(); }
        public void SetCallbacks(IConsumableSelectorActions instance)
        {
            if (m_Wrapper.m_ConsumableSelectorActionsCallbackInterface != null)
            {
                @TriggerConsumable.started -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnTriggerConsumable;
                @TriggerConsumable.performed -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnTriggerConsumable;
                @TriggerConsumable.canceled -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnTriggerConsumable;
                @MoveNext.started -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveNext;
                @MoveNext.performed -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveNext;
                @MoveNext.canceled -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveNext;
                @MoveBefore.started -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveBefore;
                @MoveBefore.performed -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveBefore;
                @MoveBefore.canceled -= m_Wrapper.m_ConsumableSelectorActionsCallbackInterface.OnMoveBefore;
            }
            m_Wrapper.m_ConsumableSelectorActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TriggerConsumable.started += instance.OnTriggerConsumable;
                @TriggerConsumable.performed += instance.OnTriggerConsumable;
                @TriggerConsumable.canceled += instance.OnTriggerConsumable;
                @MoveNext.started += instance.OnMoveNext;
                @MoveNext.performed += instance.OnMoveNext;
                @MoveNext.canceled += instance.OnMoveNext;
                @MoveBefore.started += instance.OnMoveBefore;
                @MoveBefore.performed += instance.OnMoveBefore;
                @MoveBefore.canceled += instance.OnMoveBefore;
            }
        }
    }
    public ConsumableSelectorActions @ConsumableSelector => new ConsumableSelectorActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_xBoxControllerSchemeIndex = -1;
    public InputControlScheme xBoxControllerScheme
    {
        get
        {
            if (m_xBoxControllerSchemeIndex == -1) m_xBoxControllerSchemeIndex = asset.FindControlSchemeIndex("xBoxController");
            return asset.controlSchemes[m_xBoxControllerSchemeIndex];
        }
    }
    public interface IConsumableSelectorActions
    {
        void OnTriggerConsumable(InputAction.CallbackContext context);
        void OnMoveNext(InputAction.CallbackContext context);
        void OnMoveBefore(InputAction.CallbackContext context);
    }
}