// GENERATED AUTOMATICALLY FROM 'Assets/Packages/Input/InputChess.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputChess : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputChess()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputChess"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""93c93088-e10f-4248-9e02-a898a8d8d86f"",
            ""actions"": [
                {
                    ""name"": ""Highlight"",
                    ""type"": ""Value"",
                    ""id"": ""bdf2ff4e-2fc9-473a-ac81-25d16f80053c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""de5d87f3-b62d-4913-9fcb-ceee418fdf8c"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Highlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_Highlight = m_gameplay.FindAction("Highlight", throwIfNotFound: true);
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

    // gameplay
    private readonly InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_gameplay_Highlight;
    public struct GameplayActions
    {
        private @InputChess m_Wrapper;
        public GameplayActions(@InputChess wrapper) { m_Wrapper = wrapper; }
        public InputAction @Highlight => m_Wrapper.m_gameplay_Highlight;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Highlight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHighlight;
                @Highlight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHighlight;
                @Highlight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHighlight;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Highlight.started += instance.OnHighlight;
                @Highlight.performed += instance.OnHighlight;
                @Highlight.canceled += instance.OnHighlight;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnHighlight(InputAction.CallbackContext context);
    }
}
