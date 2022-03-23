// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/Controls.inputactions'

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
            ""name"": ""Movement"",
            ""id"": ""55f0e159-b64c-4c85-aaf1-53584e55c0d1"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b26abd51-651e-4b87-b1fc-13c2cc55f01f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""2eb73c77-38c0-4efa-85d3-a75c61acaf46"",
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
                    ""id"": ""777428ea-910e-48f9-99ec-0316954a8de7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""05439be2-77a6-4ea9-815f-653b189a0ef8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""576a14d3-eedc-4ab2-bf04-1debc62d5389"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9be12cda-85df-451a-b9d0-cab21805186a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""161032f7-181b-434c-b157-40760897129b"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a9806e57-2f8a-425b-9673-9d4fe529eee7"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c8a368a3-96ea-4026-8415-1ef30b506247"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2a8e33ef-ff61-4a69-8d92-579ac43811b3"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b02ff705-0925-43e8-a2b3-ab282cf3a83e"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Aiming"",
            ""id"": ""1b1c4932-0a6e-49b7-b0b4-3e39a0cf1df1"",
            ""actions"": [
                {
                    ""name"": ""AimX"",
                    ""type"": ""Value"",
                    ""id"": ""78abb448-4de8-47b0-9582-ffc31cfa23ba"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AimY"",
                    ""type"": ""Value"",
                    ""id"": ""1d9e1b7b-936f-448e-a116-0ea0d5e141f1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4d10599f-a47b-498e-b695-6d3c32307675"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AimX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd3a95f7-dbad-48bf-b85f-1a26d1b30514"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""AimY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""e86f0b0e-37db-4eaf-8baa-9df98b00309c"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f289175d-d07b-4523-886e-985e952ddc9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""a52fe07b-6389-410f-9166-b05a73da1434"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""fcce63bb-da40-4666-baba-dc42c90f9670"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""acd69523-7f66-4c4a-8689-2e5680b08e42"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b717df7-8189-48db-a990-c0c7f6486129"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d65f803f-de56-4b5d-b247-4846d4388d3e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f1adf3f-4469-4f03-b5a3-8996c678cbc4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""L3"",
                    ""id"": ""a29c8566-0539-42cd-9ba1-12b92da597e0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f74da92c-8e4a-4b74-8b10-c32bd85bdf99"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cf5e828c-7ead-4cb1-a06d-edc7dcfc77d7"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7191fa6a-5855-4eb6-86b8-f7b9c48664f0"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""78a063ba-03f8-4f93-975a-e9292af80cfd"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D_Pad"",
                    ""id"": ""1dc4bb90-0a5d-4fd7-a457-64b8573c6d29"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b68dd509-2ee5-4407-b052-3661b2098189"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4e5ac980-c039-4acf-a5f4-cf4aea5dc2bc"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d4915d9b-1af4-48b2-945b-8985ed2f92ab"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d152784b-51f9-448a-ab27-ac50ec62fe26"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""61162a1b-0ad2-4bae-b8df-c98a347d3f06"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6f710061-a159-4ece-bdb7-7228edbbbe50"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8a6f72d8-ac51-4334-aa95-d251be538d07"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b1817bd4-5aaf-40cc-a959-c240ff262445"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""582d3fbb-11df-41f6-b5a7-8b6ffb12977d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""a04960df-44ac-4b2e-80cd-165160e1617b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""750cd93a-dcbd-4429-923d-3fe9a7b0760c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""07c7d295-c84f-4e8f-83f5-01b3ac8820ba"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""50abac50-67fb-464e-a108-06bcd393d45a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bbc86bfd-03c4-4f4c-b04d-1a89a9afb176"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
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
                    ""path"": ""<Keyboard>/slash"",
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
        },
        {
            ""name"": ""Tabs"",
            ""id"": ""0a4159ac-9c72-458f-94be-bb6567bccfbc"",
            ""actions"": [
                {
                    ""name"": ""NextTab"",
                    ""type"": ""Button"",
                    ""id"": ""d576365e-c3a1-4d44-95ef-ec57c6b908b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrevTab"",
                    ""type"": ""Button"",
                    ""id"": ""e20eee16-d188-4ec5-8ae5-68d571149a0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a457d093-8415-4a1e-94e0-653204159927"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NextTab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f686f21a-0711-4023-a509-9869e5143386"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrevTab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Back"",
            ""id"": ""4f67e53e-b962-4fae-b7ff-f00c0720e8fc"",
            ""actions"": [
                {
                    ""name"": ""GoBack"",
                    ""type"": ""Button"",
                    ""id"": ""021f55b0-5349-42d7-ab92-af45362f8340"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f68e12b8-d2c5-4a22-9ca2-2b7c3adf98e9"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""GoBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Dash"",
            ""id"": ""19798cf4-2dc3-4ae1-96b0-1f2e53145b29"",
            ""actions"": [
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""bfd7d2e2-10a4-41ce-a3a6-8ded4dd798e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""92f4b30a-9bd7-41d3-94d3-e865c2c9c920"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Attack"",
            ""id"": ""47150234-1997-4012-b054-9ed50baa4f2b"",
            ""actions"": [
                {
                    ""name"": ""PrimaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""e93faa0d-78fd-475d-ab6a-a255bd764e94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""c302eedf-aeca-4e73-a063-6fc060dd91e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""50769425-fbc4-4b1b-8232-10b6979d5c0d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d7c0079-4a7f-4b57-af0e-874c42a548bc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Interact"",
            ""id"": ""08ea524c-aadd-4b02-954a-64dfe78d8231"",
            ""actions"": [
                {
                    ""name"": ""TryToInteract"",
                    ""type"": ""Button"",
                    ""id"": ""2d3afc8b-4391-413e-8a1f-05bec6ec8ec2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""765062b3-7790-4dfa-8c3c-12ad56573ae0"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""TryToInteract"",
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
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        // Aiming
        m_Aiming = asset.FindActionMap("Aiming", throwIfNotFound: true);
        m_Aiming_AimX = m_Aiming.FindAction("AimX", throwIfNotFound: true);
        m_Aiming_AimY = m_Aiming.FindAction("AimY", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        // Console
        m_Console = asset.FindActionMap("Console", throwIfNotFound: true);
        m_Console_ToggleConsole = m_Console.FindAction("ToggleConsole", throwIfNotFound: true);
        m_Console_Return = m_Console.FindAction("Return", throwIfNotFound: true);
        // Tabs
        m_Tabs = asset.FindActionMap("Tabs", throwIfNotFound: true);
        m_Tabs_NextTab = m_Tabs.FindAction("NextTab", throwIfNotFound: true);
        m_Tabs_PrevTab = m_Tabs.FindAction("PrevTab", throwIfNotFound: true);
        // Back
        m_Back = asset.FindActionMap("Back", throwIfNotFound: true);
        m_Back_GoBack = m_Back.FindAction("GoBack", throwIfNotFound: true);
        // Dash
        m_Dash = asset.FindActionMap("Dash", throwIfNotFound: true);
        m_Dash_Dash = m_Dash.FindAction("Dash", throwIfNotFound: true);
        // Attack
        m_Attack = asset.FindActionMap("Attack", throwIfNotFound: true);
        m_Attack_PrimaryAttack = m_Attack.FindAction("PrimaryAttack", throwIfNotFound: true);
        m_Attack_SecondaryAttack = m_Attack.FindAction("SecondaryAttack", throwIfNotFound: true);
        // Interact
        m_Interact = asset.FindActionMap("Interact", throwIfNotFound: true);
        m_Interact_TryToInteract = m_Interact.FindAction("TryToInteract", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    public struct MovementActions
    {
        private @Controls m_Wrapper;
        public MovementActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Aiming
    private readonly InputActionMap m_Aiming;
    private IAimingActions m_AimingActionsCallbackInterface;
    private readonly InputAction m_Aiming_AimX;
    private readonly InputAction m_Aiming_AimY;
    public struct AimingActions
    {
        private @Controls m_Wrapper;
        public AimingActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AimX => m_Wrapper.m_Aiming_AimX;
        public InputAction @AimY => m_Wrapper.m_Aiming_AimY;
        public InputActionMap Get() { return m_Wrapper.m_Aiming; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AimingActions set) { return set.Get(); }
        public void SetCallbacks(IAimingActions instance)
        {
            if (m_Wrapper.m_AimingActionsCallbackInterface != null)
            {
                @AimX.started -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimX;
                @AimX.performed -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimX;
                @AimX.canceled -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimX;
                @AimY.started -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimY;
                @AimY.performed -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimY;
                @AimY.canceled -= m_Wrapper.m_AimingActionsCallbackInterface.OnAimY;
            }
            m_Wrapper.m_AimingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AimX.started += instance.OnAimX;
                @AimX.performed += instance.OnAimX;
                @AimX.canceled += instance.OnAimX;
                @AimY.started += instance.OnAimY;
                @AimY.performed += instance.OnAimY;
                @AimY.canceled += instance.OnAimY;
            }
        }
    }
    public AimingActions @Aiming => new AimingActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Navigate;
    public struct UIActions
    {
        private @Controls m_Wrapper;
        public UIActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Console
    private readonly InputActionMap m_Console;
    private IConsoleActions m_ConsoleActionsCallbackInterface;
    private readonly InputAction m_Console_ToggleConsole;
    private readonly InputAction m_Console_Return;
    public struct ConsoleActions
    {
        private @Controls m_Wrapper;
        public ConsoleActions(@Controls wrapper) { m_Wrapper = wrapper; }
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

    // Tabs
    private readonly InputActionMap m_Tabs;
    private ITabsActions m_TabsActionsCallbackInterface;
    private readonly InputAction m_Tabs_NextTab;
    private readonly InputAction m_Tabs_PrevTab;
    public struct TabsActions
    {
        private @Controls m_Wrapper;
        public TabsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextTab => m_Wrapper.m_Tabs_NextTab;
        public InputAction @PrevTab => m_Wrapper.m_Tabs_PrevTab;
        public InputActionMap Get() { return m_Wrapper.m_Tabs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TabsActions set) { return set.Get(); }
        public void SetCallbacks(ITabsActions instance)
        {
            if (m_Wrapper.m_TabsActionsCallbackInterface != null)
            {
                @NextTab.started -= m_Wrapper.m_TabsActionsCallbackInterface.OnNextTab;
                @NextTab.performed -= m_Wrapper.m_TabsActionsCallbackInterface.OnNextTab;
                @NextTab.canceled -= m_Wrapper.m_TabsActionsCallbackInterface.OnNextTab;
                @PrevTab.started -= m_Wrapper.m_TabsActionsCallbackInterface.OnPrevTab;
                @PrevTab.performed -= m_Wrapper.m_TabsActionsCallbackInterface.OnPrevTab;
                @PrevTab.canceled -= m_Wrapper.m_TabsActionsCallbackInterface.OnPrevTab;
            }
            m_Wrapper.m_TabsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NextTab.started += instance.OnNextTab;
                @NextTab.performed += instance.OnNextTab;
                @NextTab.canceled += instance.OnNextTab;
                @PrevTab.started += instance.OnPrevTab;
                @PrevTab.performed += instance.OnPrevTab;
                @PrevTab.canceled += instance.OnPrevTab;
            }
        }
    }
    public TabsActions @Tabs => new TabsActions(this);

    // Back
    private readonly InputActionMap m_Back;
    private IBackActions m_BackActionsCallbackInterface;
    private readonly InputAction m_Back_GoBack;
    public struct BackActions
    {
        private @Controls m_Wrapper;
        public BackActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @GoBack => m_Wrapper.m_Back_GoBack;
        public InputActionMap Get() { return m_Wrapper.m_Back; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BackActions set) { return set.Get(); }
        public void SetCallbacks(IBackActions instance)
        {
            if (m_Wrapper.m_BackActionsCallbackInterface != null)
            {
                @GoBack.started -= m_Wrapper.m_BackActionsCallbackInterface.OnGoBack;
                @GoBack.performed -= m_Wrapper.m_BackActionsCallbackInterface.OnGoBack;
                @GoBack.canceled -= m_Wrapper.m_BackActionsCallbackInterface.OnGoBack;
            }
            m_Wrapper.m_BackActionsCallbackInterface = instance;
            if (instance != null)
            {
                @GoBack.started += instance.OnGoBack;
                @GoBack.performed += instance.OnGoBack;
                @GoBack.canceled += instance.OnGoBack;
            }
        }
    }
    public BackActions @Back => new BackActions(this);

    // Dash
    private readonly InputActionMap m_Dash;
    private IDashActions m_DashActionsCallbackInterface;
    private readonly InputAction m_Dash_Dash;
    public struct DashActions
    {
        private @Controls m_Wrapper;
        public DashActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Dash => m_Wrapper.m_Dash_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Dash; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DashActions set) { return set.Get(); }
        public void SetCallbacks(IDashActions instance)
        {
            if (m_Wrapper.m_DashActionsCallbackInterface != null)
            {
                @Dash.started -= m_Wrapper.m_DashActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_DashActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_DashActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_DashActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public DashActions @Dash => new DashActions(this);

    // Attack
    private readonly InputActionMap m_Attack;
    private IAttackActions m_AttackActionsCallbackInterface;
    private readonly InputAction m_Attack_PrimaryAttack;
    private readonly InputAction m_Attack_SecondaryAttack;
    public struct AttackActions
    {
        private @Controls m_Wrapper;
        public AttackActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PrimaryAttack => m_Wrapper.m_Attack_PrimaryAttack;
        public InputAction @SecondaryAttack => m_Wrapper.m_Attack_SecondaryAttack;
        public InputActionMap Get() { return m_Wrapper.m_Attack; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AttackActions set) { return set.Get(); }
        public void SetCallbacks(IAttackActions instance)
        {
            if (m_Wrapper.m_AttackActionsCallbackInterface != null)
            {
                @PrimaryAttack.started -= m_Wrapper.m_AttackActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.performed -= m_Wrapper.m_AttackActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.canceled -= m_Wrapper.m_AttackActionsCallbackInterface.OnPrimaryAttack;
                @SecondaryAttack.started -= m_Wrapper.m_AttackActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.performed -= m_Wrapper.m_AttackActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.canceled -= m_Wrapper.m_AttackActionsCallbackInterface.OnSecondaryAttack;
            }
            m_Wrapper.m_AttackActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PrimaryAttack.started += instance.OnPrimaryAttack;
                @PrimaryAttack.performed += instance.OnPrimaryAttack;
                @PrimaryAttack.canceled += instance.OnPrimaryAttack;
                @SecondaryAttack.started += instance.OnSecondaryAttack;
                @SecondaryAttack.performed += instance.OnSecondaryAttack;
                @SecondaryAttack.canceled += instance.OnSecondaryAttack;
            }
        }
    }
    public AttackActions @Attack => new AttackActions(this);

    // Interact
    private readonly InputActionMap m_Interact;
    private IInteractActions m_InteractActionsCallbackInterface;
    private readonly InputAction m_Interact_TryToInteract;
    public struct InteractActions
    {
        private @Controls m_Wrapper;
        public InteractActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TryToInteract => m_Wrapper.m_Interact_TryToInteract;
        public InputActionMap Get() { return m_Wrapper.m_Interact; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InteractActions set) { return set.Get(); }
        public void SetCallbacks(IInteractActions instance)
        {
            if (m_Wrapper.m_InteractActionsCallbackInterface != null)
            {
                @TryToInteract.started -= m_Wrapper.m_InteractActionsCallbackInterface.OnTryToInteract;
                @TryToInteract.performed -= m_Wrapper.m_InteractActionsCallbackInterface.OnTryToInteract;
                @TryToInteract.canceled -= m_Wrapper.m_InteractActionsCallbackInterface.OnTryToInteract;
            }
            m_Wrapper.m_InteractActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TryToInteract.started += instance.OnTryToInteract;
                @TryToInteract.performed += instance.OnTryToInteract;
                @TryToInteract.canceled += instance.OnTryToInteract;
            }
        }
    }
    public InteractActions @Interact => new InteractActions(this);
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
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IAimingActions
    {
        void OnAimX(InputAction.CallbackContext context);
        void OnAimY(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
    }
    public interface IConsoleActions
    {
        void OnToggleConsole(InputAction.CallbackContext context);
        void OnReturn(InputAction.CallbackContext context);
    }
    public interface ITabsActions
    {
        void OnNextTab(InputAction.CallbackContext context);
        void OnPrevTab(InputAction.CallbackContext context);
    }
    public interface IBackActions
    {
        void OnGoBack(InputAction.CallbackContext context);
    }
    public interface IDashActions
    {
        void OnDash(InputAction.CallbackContext context);
    }
    public interface IAttackActions
    {
        void OnPrimaryAttack(InputAction.CallbackContext context);
        void OnSecondaryAttack(InputAction.CallbackContext context);
    }
    public interface IInteractActions
    {
        void OnTryToInteract(InputAction.CallbackContext context);
    }
}
