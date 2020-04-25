// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Utils/Misc/Controls.inputactions'

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
            ""name"": ""Default"",
            ""id"": ""61888e82-551f-474b-b61f-9a6afba572bf"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1f1acdf5-a184-4e12-9041-51f49b20a534"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""fc37e1d1-2698-44b3-bdcc-6e2fecec7940"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tilt"",
                    ""type"": ""Value"",
                    ""id"": ""db12f650-dec9-4c01-8d8b-c6ccadd87442"",
                    ""expectedControlType"": """",
                    ""processors"": ""Scale(factor=3)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScreenClick"",
                    ""type"": ""Button"",
                    ""id"": ""1121e82c-7cda-4b6b-bd52-2bb8a75380fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleOptions"",
                    ""type"": ""Button"",
                    ""id"": ""a5d5821c-f476-4899-b58c-cd54be09a121"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""69ac8e04-4f6d-4374-b2ce-55eb194d6f68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""f0b7063c-2f6f-4435-aec1-7ef4da07614a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""05c39f1c-a39f-4505-ad6f-dc288c712517"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""977fb97e-753b-4d05-afcc-43c31eb169c3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8dfe5217-89d5-4934-81ad-1a03bfc67722"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""49d6dc6a-d675-475b-b9cc-5a6fe3dcfaf5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""caf1fe8e-692c-42c5-819d-61aa27157e8d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""66c756b7-5d44-49f4-b734-087eade672e3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""5b7cb85e-109b-49e3-ad77-f942752bbebb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d9e37d01-9909-4df5-ae69-1b1b3955472d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""53c4a565-b8cb-4059-a3b6-0bf9fa500671"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""066b2143-ce9d-4227-96d9-f7f890ca3bf2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f76fcbbd-92a4-438d-8c8d-b80790f5a089"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""83ae62f1-6ffd-409a-af22-154de1b73ae9"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2b109b53-d543-403d-8d59-b286a4755b5e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=6,y=6)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8861316-f080-4d50-9c54-5d25d1216b2d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f481a9db-b02a-46b3-b3c5-73fff71e93fa"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScreenClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc563db0-c013-4f3a-aae0-8f8c92b2c580"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ScreenClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0968489a-560c-43d7-a907-ef6034c3f296"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db4ce959-785b-4d60-8227-929a21d2dd6c"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5754cd39-fba5-4d01-83cb-f1549a6efe49"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""625e436d-5651-4e10-869b-728934d8304b"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6bcc600-6a55-4ff7-9b7e-87b2eb9b7619"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""3b4f5ef6-cd74-4191-bae1-22969c118343"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""206d9ea2-859e-4fd1-91cf-d0327a03a0d7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0f1200af-9a6b-466d-afdb-1f4f2e583dd7"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Move = m_Default.FindAction("Move", throwIfNotFound: true);
        m_Default_Look = m_Default.FindAction("Look", throwIfNotFound: true);
        m_Default_Tilt = m_Default.FindAction("Tilt", throwIfNotFound: true);
        m_Default_ScreenClick = m_Default.FindAction("ScreenClick", throwIfNotFound: true);
        m_Default_ToggleOptions = m_Default.FindAction("ToggleOptions", throwIfNotFound: true);
        m_Default_Back = m_Default.FindAction("Back", throwIfNotFound: true);
        m_Default_Zoom = m_Default.FindAction("Zoom", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private IDefaultActions m_DefaultActionsCallbackInterface;
    private readonly InputAction m_Default_Move;
    private readonly InputAction m_Default_Look;
    private readonly InputAction m_Default_Tilt;
    private readonly InputAction m_Default_ScreenClick;
    private readonly InputAction m_Default_ToggleOptions;
    private readonly InputAction m_Default_Back;
    private readonly InputAction m_Default_Zoom;
    public struct DefaultActions
    {
        private @Controls m_Wrapper;
        public DefaultActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Default_Move;
        public InputAction @Look => m_Wrapper.m_Default_Look;
        public InputAction @Tilt => m_Wrapper.m_Default_Tilt;
        public InputAction @ScreenClick => m_Wrapper.m_Default_ScreenClick;
        public InputAction @ToggleOptions => m_Wrapper.m_Default_ToggleOptions;
        public InputAction @Back => m_Wrapper.m_Default_Back;
        public InputAction @Zoom => m_Wrapper.m_Default_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void SetCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnLook;
                @Tilt.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTilt;
                @Tilt.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTilt;
                @Tilt.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnTilt;
                @ScreenClick.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScreenClick;
                @ScreenClick.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScreenClick;
                @ScreenClick.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnScreenClick;
                @ToggleOptions.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleOptions;
                @ToggleOptions.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleOptions;
                @ToggleOptions.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnToggleOptions;
                @Back.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnBack;
                @Zoom.started -= m_Wrapper.m_DefaultActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_DefaultActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_DefaultActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_DefaultActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Tilt.started += instance.OnTilt;
                @Tilt.performed += instance.OnTilt;
                @Tilt.canceled += instance.OnTilt;
                @ScreenClick.started += instance.OnScreenClick;
                @ScreenClick.performed += instance.OnScreenClick;
                @ScreenClick.canceled += instance.OnScreenClick;
                @ToggleOptions.started += instance.OnToggleOptions;
                @ToggleOptions.performed += instance.OnToggleOptions;
                @ToggleOptions.canceled += instance.OnToggleOptions;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnTilt(InputAction.CallbackContext context);
        void OnScreenClick(InputAction.CallbackContext context);
        void OnToggleOptions(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}