using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConsumableManager : MonoBehaviour, Controls.IConsumableSlotsActions, Controls.IInteractActions, IInitialisable
{
    public static ConsumableManager _instance;
    [SerializeField] private HUDManager HUD;

    private BaseConsumable _primaryConsumable;
    private BaseConsumable _secondaryConsumable;
    private ConsumablePickUp _currentPickUp;

    private Controls _input;

    public void Init()
    {
        if (!_instance)
        {
            _instance = this;
            _input = new Controls();
            _input.ConsumableSlots.SetCallbacks(this);
            _input.Interact.SetCallbacks(this);
            _input.Enable();




        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnNewConsumableAvailable(string consumableName, ConsumablePickUp pickUp)
    {
        if (!HUD)
        {
            HUD = GetHUDManager();
        }
        _currentPickUp = pickUp;
        if (!_primaryConsumable &&!_secondaryConsumable)
        {
            if (HUD)
            {
                HUD.PromptManager.ShowPrompt("[E] to equip - " + consumableName);
            }
         
        }else if(_primaryConsumable && !_secondaryConsumable)
        {
            HUD.PromptManager.ShowPrompt("[E] To equip - " + consumableName);
        }
        else if (!_primaryConsumable && _secondaryConsumable)
        {
            HUD.PromptManager.ShowPrompt("[E] to equip - " + consumableName);
        }
        else
        {
            HUD.PromptManager.ShowPrompt("Replace [1] or [2] to equip - " + consumableName);
        }
    }

    public void OnNoConsumableAvailable()
    {
        if (HUD)
        {
            HUD.PromptManager.RemovePrompt();
        }
        _currentPickUp = null;
    }

    public void OnTrySlotOne(InputAction.CallbackContext context)
    {
        if(!_primaryConsumable && !_currentPickUp)
        {
          
            return;
        }
        if (!HUD)
        {
            HUD = GetHUDManager();
        }
        HUD.ItemDisplayManager.gameObject.SetActive(true);
        if (context.performed)
        {
            if (_primaryConsumable && !_currentPickUp)
            {
                if (_primaryConsumable.UseConsumable())
                {

                    _primaryConsumable.Remove();
                    _primaryConsumable = null;
                    HUD.ItemDisplayManager.SetPrimaryIcon(null);
                    return;
                }
                else
                {
                    return;
                }
          

            }
            else if (_primaryConsumable && _currentPickUp)
            {
                GameObject previos = _primaryConsumable.gameObject;
                if (ObjectPoolManager.instance)
                {
                    _primaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(),Vector3.zero,Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _primaryConsumable =Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
          
                _currentPickUp.UpdatePickUp(previos);
                _primaryConsumable.Owner = transform.parent.gameObject;
            }
            else
            {

                if (ObjectPoolManager.instance)
                {
                    _primaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _primaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                 
                }
                _primaryConsumable.Owner = transform.parent.gameObject;
                _currentPickUp.UpdatePickUp(null);
            }
            HUD.ItemDisplayManager.SetPrimaryIcon(_primaryConsumable.Sprite);
        }


    }

    public void OnTrySlotTwo(InputAction.CallbackContext context)
    {
        if (!_secondaryConsumable && !_currentPickUp)
        {

            return;
        }
        if (!HUD)
        {
            HUD = GetHUDManager();
        }
        HUD.ItemDisplayManager.gameObject.SetActive(true);
        if (context.performed)
        {
            if (_secondaryConsumable && !_currentPickUp)
            {
                if (_secondaryConsumable.UseConsumable())
                {
                    _secondaryConsumable.Remove();
                    _secondaryConsumable = null;
                    HUD.ItemDisplayManager.SetSecondary(null);
                    return;
                }
                else
                {
                    return;
                }
             
      
          


            }
            else if (_secondaryConsumable && _currentPickUp)
            {
                GameObject previos = _secondaryConsumable.gameObject;
                if (ObjectPoolManager.instance)
                {
                    _secondaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _secondaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
                _currentPickUp.UpdatePickUp(previos);
                _secondaryConsumable.Owner = transform.parent.gameObject;
            }
            else
            {
                if (ObjectPoolManager.instance)
                {
                    _secondaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _secondaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
                _currentPickUp.UpdatePickUp(null);
                _secondaryConsumable.Owner = transform.parent.gameObject;
            }

            HUD.ItemDisplayManager.SetSecondary(_secondaryConsumable.Sprite);
        }
 
    }

    public void OnTryToInteract(InputAction.CallbackContext context)
    {
        if (!_currentPickUp)
        {
            return;
        }
        if (!HUD)
        {
            HUD = GetHUDManager();
        }
        HUD.ItemDisplayManager.gameObject.SetActive(true);
        if (context.performed)
        {
            
            if (!_primaryConsumable && !_secondaryConsumable)
            {
                if (ObjectPoolManager.instance)
                {
                    _primaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _primaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
                _currentPickUp.UpdatePickUp(null);
                HUD.ItemDisplayManager.SetPrimaryIcon(_primaryConsumable.Sprite);
                _primaryConsumable.Owner = transform.parent.gameObject;
            }
            else if (_primaryConsumable && !_secondaryConsumable)
            {
                if (ObjectPoolManager.instance)
                {
                    _secondaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _secondaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
    
                _currentPickUp.UpdatePickUp(null);
                HUD.ItemDisplayManager.SetSecondary(_secondaryConsumable.Sprite);
                _secondaryConsumable.Owner = transform.parent.gameObject;
            }
            else if (!_primaryConsumable && _secondaryConsumable)
            {
                if (ObjectPoolManager.instance)
                {
                    _primaryConsumable = ObjectPoolManager.Spawn
                        (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                        .GetComponent<BaseConsumable>();
                }
                else
                {
                    _primaryConsumable = Instantiate
                       (_currentPickUp.GetConsumable(), Vector3.zero, Quaternion.identity)
                       .GetComponent<BaseConsumable>();
                }
                HUD.ItemDisplayManager.SetPrimaryIcon(_primaryConsumable.Sprite);
                _currentPickUp.UpdatePickUp(null);
                _primaryConsumable.Owner = transform.parent.gameObject;
            }
        }

    }

    private HUDManager GetHUDManager()
    {
        if (HUD)
        {
            return HUD;
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.GameManager || !GameStateManager.instance.GameManager.HUDManager)
            {
                return null;
            }
            else
            {
                return GameStateManager.instance.GameManager.HUDManager;
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
    }
}
