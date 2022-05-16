// GENERATED AUTOMATICALLY FROM 'Assets/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""ChessBoard"",
            ""id"": ""7aa0c504-7e57-4ce0-8755-ec3670f7bfe2"",
            ""actions"": [
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""6cd253fc-e090-4e7c-95e2-217f26c86e8c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""8b8c1f25-868d-4f4b-8d21-d95c4c3c5842"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""15e37f7a-5f4b-4ef1-98b5-dc20f9987192"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44dc53f7-890d-4177-a44d-840eea108acd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ChessBoard
        m_ChessBoard = asset.FindActionMap("ChessBoard", throwIfNotFound: true);
        m_ChessBoard_Confirm = m_ChessBoard.FindAction("Confirm", throwIfNotFound: true);
        m_ChessBoard_MousePosition = m_ChessBoard.FindAction("MousePosition", throwIfNotFound: true);
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

    // ChessBoard
    private readonly InputActionMap m_ChessBoard;
    private IChessBoardActions m_ChessBoardActionsCallbackInterface;
    private readonly InputAction m_ChessBoard_Confirm;
    private readonly InputAction m_ChessBoard_MousePosition;
    public struct ChessBoardActions
    {
        private @Controls m_Wrapper;
        public ChessBoardActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Confirm => m_Wrapper.m_ChessBoard_Confirm;
        public InputAction @MousePosition => m_Wrapper.m_ChessBoard_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_ChessBoard; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ChessBoardActions set) { return set.Get(); }
        public void SetCallbacks(IChessBoardActions instance)
        {
            if (m_Wrapper.m_ChessBoardActionsCallbackInterface != null)
            {
                @Confirm.started -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnConfirm;
                @MousePosition.started -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_ChessBoardActionsCallbackInterface.OnMousePosition;
            }
            m_Wrapper.m_ChessBoardActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public ChessBoardActions @ChessBoard => new ChessBoardActions(this);
    public interface IChessBoardActions
    {
        void OnConfirm(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
