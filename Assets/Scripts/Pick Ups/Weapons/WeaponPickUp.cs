using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickUp : BasePickUp, Controls.IInteractActions
{

    [SerializeField] private GameObject _weaponPrefab;
    [SerializeField] private string InteractPrompt;
    [SerializeField] private string PickUpName;
    private List<GameObject> _children = new List<GameObject>();
    private bool _inRange;
    private HUDPrompt Prompt;
    private Collider _rangeTrigger;

    private Controls _input;
    private bool _isEnabled;


    private void Awake()
    {
        _rangeTrigger = GetComponent<Collider>();

        if(transform.childCount > 0)
        {
            for (int i = 0; i< transform.childCount; i++)
            {
                _children.Add(transform.GetChild(i).gameObject);
            }
        
        }


        _input = new Controls();
        _input.Interact.SetCallbacks(this);
        _isEnabled = true;
        PickUpName = _weaponPrefab.GetComponent<BaseWeapon>().WeaponName;


    }

    public void OnTryToInteract(InputAction.CallbackContext context)
    {
        if (context.performed &&_inRange)
        {
            DoPickUp();
        }
    }

    protected override void DisablePickUp()
    {
        _rangeTrigger.enabled = false;
        _inRange = false;
        if (_input != null)
        {
            _input.Disable();
        }
        _isEnabled = false;
        if (_children.Count == 0) return;
        foreach(GameObject child  in _children){
            child.SetActive(false);
        }
    }

    protected override void DoPickUp()
    {
        BaseWeapon weapon = Instantiate(_weaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseWeapon>();
 
        if (weapon)
        {
            WeaponManager._instance.EquipWeapon(weapon);
            DisablePickUp();
          
        }

        if (Prompt)
        {
            Prompt.RemovePrompt(InteractPrompt);
        }
    }

    public override void EnablePickUp()
    {
        _rangeTrigger.enabled = true;
        _isEnabled = true;

        if (_children.Count == 0) return;
        foreach (GameObject child in _children)
        {
            child.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
        if(_input != null)
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

    public override bool IsEnabled()
    {
        return _isEnabled;
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


    public string Name { get { return PickUpName; } }
}
