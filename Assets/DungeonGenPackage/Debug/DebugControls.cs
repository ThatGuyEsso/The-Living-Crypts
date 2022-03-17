// GENERATED AUTOMATICALLY FROM 'Assets/DungeonGenPackage/Debug/DebugControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DebugControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DebugControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DebugControls"",
    ""maps"": [
        {
            ""name"": ""Console"",
            ""id"": ""68b9ed39-db82-4691-aca2-15727808ad48"",
            ""actions"": [
                {
                    ""name"": ""ToggleConsole"",
                    ""type"": ""Button"",
                    ""id"": ""4e010e3d-d543-4656-9fcd-c57896ed506b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Return"",
                    ""type"": ""Button"",
                    ""id"": ""965606d5-ca9d-447d-8d31-d02779517dbe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e916e9a7-5402-4e48-b4aa-4310d46a1b59"",
                    ""path"": ""<Keyboard>/equals"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71dbfae5-4bb5-4b70-9721-238908e1b1c5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Return"",
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
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Console
        m_Console = asset.FindActionMap("Console", throwIfNotFound: true);
        m_Console_ToggleConsole = m_Console.FindAction("ToggleConsole", throwIfNotFound: true);
        m_Console_Return = m_Console.FindAction("Return", throwIfNotFound: true);
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

    // Console
    private readonly InputActionMap m_Console;
    private IConsoleActions m_ConsoleActionsCallbackInterface;
    private readonly InputAction m_Console_ToggleConsole;
    private readonly InputAction m_Console_Return;
    public struct ConsoleActions
    {
        private @DebugControls m_Wrapper;
        public ConsoleActions(@DebugControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleConsole => m_Wrapper.m_Console_ToggleConsole;
        public InputAction @Return => m_Wrapper.m_Console_Return;
        public InputActionMap Get() { return m_Wrapper.m_Console; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ConsoleActions set) { return set.Get(); }
        public void SetCallbacks(IConsoleActions instance)
        {
            if (m_Wrapper.m_ConsoleActionsCallbackInterface != null)
            {
                @ToggleConsole.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggleConsole;
                @ToggleConsole.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggleConsole;
                @ToggleConsole.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnToggleConsole;
                @Return.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnReturn;
                @Return.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnReturn;
                @Return.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnReturn;
            }
            m_Wrapper.m_ConsoleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleConsole.started += instance.OnToggleConsole;
                @ToggleConsole.performed += instance.OnToggleConsole;
                @ToggleConsole.canceled += instance.OnToggleConsole;
                @Return.started += instance.OnReturn;
                @Return.performed += instance.OnReturn;
                @Return.canceled += instance.OnReturn;
            }
        }
    }
    public ConsoleActions @Console => new ConsoleActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IConsoleActions
    {
        void OnToggleConsole(InputAction.CallbackContext context);
        void OnReturn(InputAction.CallbackContext context);
    }
}
