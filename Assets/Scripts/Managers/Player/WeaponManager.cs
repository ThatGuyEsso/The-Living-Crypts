using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour,IInitialisable
{
    public static WeaponManager _instance;

    public static BaseWeapon _equippedWeapon;
    private bool _isWeaponEquipped;
    //references
    private PlayerBehaviour _playerBehaviour;
    [SerializeField] private Transform _weaponEquipPoint;
    [SerializeField] private FPSMovement _ownerMovement;

    [Header("SFX")]
    [SerializeField] private string EquipSFX;

    private Controls _input;
    [SerializeField]  private AudioManager _audioManager;
    private HUDManager _hudManager;
    public System.Action<string> OnWeaponEquipped;
   
    public void Init()
    {
        if (!_instance)
        {
            _instance = this;
            _input = new Controls();
            _input.Attack.PrimaryAttack.started += _ =>OnPrimaryAttack();
            _input.Attack.PrimaryAttack.canceled += _ => OnStopPrimaryAttack();
            _input.Attack.SecondaryAttack.started += _ => OnSecondaryAttack();
            _input.Attack.SecondaryAttack.canceled += _ => OnStopSecondaryAttack();
            _input.Enable();

            _ownerMovement = GetComponentInParent<FPSMovement>();
            _ownerMovement.OnWalk += OnOwnerMove;
            _ownerMovement.OnStop += OnOwnerStop;

            if (!_playerBehaviour)
            {
                _playerBehaviour = GetComponentInParent<PlayerBehaviour>();
            }

            if (_playerBehaviour)
            {
                _playerBehaviour.OnPlayerDied += OnPlayerDeath;
            }


        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnOwnerStop()
    {
        if (_equippedWeapon) _equippedWeapon.SetIsMoving(false);
    }
    public void OnOwnerMove()
    {
        if (_equippedWeapon) _equippedWeapon.SetIsMoving(true);
    }
    public void SetEquippedWeaponEquipPoint(Transform equipPoint)
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.SetEquipPoint(equipPoint);

        }
    }

 
    public void SetUpUIBinds(BaseWeapon weapon)
    {
        if (weapon.gameObject)
        {
            weapon.OnNewPrimaryCooldown += UpdatePrimaryAttackCoolDown;
            weapon.OnNewSecondaryCooldown += UpdateSecondaryAttackCoolDown;
        }
      
    }
    public void RemoveUIBinds(BaseWeapon weapon)
    {
        if (weapon.gameObject)
        {
            weapon.OnNewPrimaryCooldown -= UpdatePrimaryAttackCoolDown;
            weapon.OnNewSecondaryCooldown -= UpdateSecondaryAttackCoolDown;
        }

    }

    public void EquipWeapon(BaseWeapon weapon)
    {
       
        if (!_equippedWeapon)
        {
            _equippedWeapon = weapon;

            OnWeaponEquipped?.Invoke(weapon.WeaponName);
            _isWeaponEquipped = true;
            _equippedWeapon.transform.position = _weaponEquipPoint.position;
            _equippedWeapon.SetEquipPoint(_weaponEquipPoint);
            _equippedWeapon.Init();
            PlayEquipSFX();
            UpdateWeaponDisplayed();
            SetUpUIBinds(_equippedWeapon);
            if (_audioManager)
            {
                _equippedWeapon.AudioManager = _audioManager;
            }
        }
        else
        {
            UnequipWeapon();
            EquipWeapon(weapon);
        }


   
    }
    private void PlayEquipSFX()
    {
        if (!_audioManager)
        {
            _audioManager = GetAudioManager();
        }

        if (!_audioManager)
        {
            return;
        }

        _audioManager.PlayThroughAudioPlayer(EquipSFX, _playerBehaviour.transform.position, true);

    }
    public void OnPlayerDeath()
    {
        if (_equippedWeapon)
        {
            OnStopPrimaryAttack();
            OnStopSecondaryAttack();
            DisableInput();
        }
    }

    public void UpdateWeaponDisplayed()
    {
        if (!_hudManager)
        {
            _hudManager = GetHUDManager();
        }

        if (_hudManager)
        {
            _hudManager.WeaponDisplayManager.SetUp(_equippedWeapon.PrimaryUISprite,
                _equippedWeapon.SecondaryUISprite, _equippedWeapon.MaxPrimaryCooldown, _equippedWeapon.MaxSecondaryCooldown);
        }
    }
    public void UpdatePrimaryAttackCoolDown(float newCooldown)
    {
        if (!_hudManager)
        {
            _hudManager = GetHUDManager();
        }

        if (_hudManager)
        {
            _hudManager.WeaponDisplayManager.UpdatePrimaryCooldown(newCooldown);
        }
    }
    public void UpdateSecondaryAttackCoolDown(float newCooldown)
    {
        if (!_hudManager)
        {
            _hudManager = GetHUDManager();
        }

        if (_hudManager)
        {
            _hudManager.WeaponDisplayManager.UpdateSecondaryCooldown(newCooldown);
        }
    }
    public void ResetManager()
    {
        UnequipWeapon();
        EnableInput();
    }
    public void OnPrimaryAttack()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.TryToPrimaryAttack();
        }
    }

    public void OnSecondaryAttack()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.TryToSecondaryAttack();
        }
    }

    public void OnStopPrimaryAttack()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.StopTryToPrimaryAttack();
        }
    }
    public void OnStopSecondaryAttack()
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.StopTryToSecondaryAttack();
        }
    }
    public void UnequipWeapon()
    {
    
        if (_equippedWeapon)
        {
            _isWeaponEquipped = false;
            RemoveUIBinds(_equippedWeapon);
            Destroy(_equippedWeapon.gameObject);
            _equippedWeapon = null;
        }
    }

    public void DisableInput()
    {
        if (_input != null) _input.Disable();
    }
    public void EnableInput()
    {
        if (_input != null) _input.Enable();
    }

    private void OnDisable()
    {
        if (_playerBehaviour)
        {
            _playerBehaviour.OnPlayerDied -= OnPlayerDeath;
        }
        DisableInput();
    }
    public bool IsWeaponEquipped { get { return _isWeaponEquipped; } }

    public GameObject Getowner()
    {
        return _ownerMovement.gameObject;
    }

    private AudioManager GetAudioManager()
    {
        if (_audioManager)
        {
            return _audioManager;
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }
            else
            {
                return GameStateManager.instance.AudioManager;
            }
        }
    }

    private HUDManager GetHUDManager()
    {
        if (_hudManager)
        {
            return _hudManager;
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
}
