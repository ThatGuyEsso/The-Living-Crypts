using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayRoomManager : MonoBehaviour, Controls.IInteractActions
{
    [Header("Pick Ups")]
    [SerializeField] private WeaponPickUp TheLegacyPickUp;
    [SerializeField] private WeaponPickUp LeavateinnPickUp;
    [SerializeField] private WeaponPickUp TheBalancePickUp;

    [Header("Door")]
    [SerializeField] private Door DungeonEntrance;
    private List<WeaponPickUp> _pickUps = new List<WeaponPickUp>();

    //States
    private bool _inRange;

    [Header("Interaction")]
    [SerializeField] private string InteractPrompt;

    private HUDPrompt Prompt;
    private Collider _rangeTrigger;
    private GameManager GameManager;


    //Input
    private Controls _input;
    private bool _isInitialised;
    private void Awake()
    {
        if (GameStateManager.instance)
        {
            if (GameStateManager.instance.GameManager)
            {
                GameManager = GameStateManager.instance.GameManager;
                GameManager.OnNewGamplayEvent += EvaluateGameplayEvent;
            }
        }

        _rangeTrigger = GetComponent<Collider>();
        if (_rangeTrigger)
        {
            _rangeTrigger.enabled = false;
        }

        _input = new Controls();
        _input.Interact.SetCallbacks(this);
    }

    public void EvaluateGameplayEvent(GameplayEvents newEvent){
        switch (newEvent)
        {
            case GameplayEvents.WeaponSelected:
                Init();
                break;
            case GameplayEvents.DungeonGenComplete:
                DungeonEntrance.OpenDoor();
                break;
            case GameplayEvents.GameComplete:
                break;
            case GameplayEvents.PlayerDied:
                break;
            case GameplayEvents.PlayerRespawned:
                DungeonEntrance.CloseDoor();

                foreach (WeaponPickUp pickup in _pickUps)
                {
                 
                    if (!pickup.IsEnabled())
                    {
                        pickup.EnablePickUp();
                    }
                  
                }
              
                break;
        }
    }


    public void EvalauteWeaponSelected(string weapon)
    {
        foreach(WeaponPickUp pickup in _pickUps)
        {
            if(pickup.Name != weapon)
            {
                if (!pickup.IsEnabled())
                {
                    pickup.EnablePickUp();
                }
            }
        }
    }
    public void Init()
    {
        if (!_isInitialised)
        {
            _isInitialised = true;
            if (_pickUps.Count == 0)
            {
                _pickUps.Add(TheLegacyPickUp);
                _pickUps.Add(LeavateinnPickUp);
                _pickUps.Add(TheBalancePickUp);
            }


            if (WeaponManager._instance)
            {
                WeaponManager._instance.OnWeaponEquipped += EvalauteWeaponSelected;
            }
        }
    

        _rangeTrigger.enabled = true;
    }

    public void OnTryToInteract(InputAction.CallbackContext context)
    {
        if (context.performed && _inRange&& GameManager.Event == GameplayEvents.WeaponSelected)
        {
            InvokeDungeon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Event != GameplayEvents.WeaponSelected)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            _inRange = true;
            if (_input != null)
            {
                _input.Enable();
            }

            if (Prompt)
            {
                Prompt.ShowPrompt(InteractPrompt);
            }
            else
            {
                Prompt = GetPromptFromGameManager();
                if (Prompt)
                {
                    Prompt.ShowPrompt(InteractPrompt);
                }

            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Event != GameplayEvents.WeaponSelected)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            _inRange = false;
            if (_input != null)
            {
                _input.Disable();
            }
            if (Prompt)
            {
                Prompt.RemovePrompt(InteractPrompt);
            }
            else
            {
                Prompt = GetPromptFromGameManager();
                if (Prompt)
                {
                    Prompt.RemovePrompt(InteractPrompt);
                }

            }
        }
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Disable();
        }
        if (GameStateManager.instance)
        {
            if (GameStateManager.instance.GameManager)
            {
                GameStateManager.instance.GameManager.OnNewGamplayEvent -= EvaluateGameplayEvent;
            }
        }

        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped -= EvalauteWeaponSelected;
        }
    }


    public HUDPrompt GetPromptFromGameManager()
    {
        if (!GameStateManager.instance)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager.HUDManager)
        {
            return null;
        }
        return GameStateManager.instance.GameManager.HUDManager.PromptManager;
    }

    public void InvokeDungeon()
    {
        _rangeTrigger.enabled = false;
        Prompt.RemovePrompt(InteractPrompt);
        if (GameManager)
        {
            GameManager.BeginNewGameplayEvent(GameplayEvents.DungeonInvoked);
        }
        else
        {
            if (!GameStateManager.instance)
            {
                return ;
            }
            if (!GameStateManager.instance.GameManager)
            {
                return ;
            }
            GameManager = GameStateManager.instance.GameManager;
            GameManager.BeginNewGameplayEvent(GameplayEvents.DungeonInvoked);
        }
    }
}
