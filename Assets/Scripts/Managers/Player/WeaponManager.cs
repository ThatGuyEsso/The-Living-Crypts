using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager _instance;

    public static BaseWeapon _equippedWeapon;
    private bool _isWeaponEquipped;

    [SerializeField] private Transform _weaponEquipPoint;

    private Controls _input;
    private void Awake()
    {
        Init();
    }
    private void Init()
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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEquippedWeaponEquipPoint(Transform equipPoint)
    {
        if (_equippedWeapon)
        {
            _equippedWeapon.SetEquipPoint(equipPoint);

        }
    }

    public void EquipWeapon(BaseWeapon weapon)
    {
       
        if (!_equippedWeapon)
        {
            _equippedWeapon = weapon;
            _isWeaponEquipped = true;
            _equippedWeapon.SetEquipPoint(_weaponEquipPoint);
            _equippedWeapon.Init();
        }
        else
        {
            UnequipWeapon();
            EquipWeapon(weapon);
        }

   
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
            _equippedWeapon.TryToSecondaryyAttack();
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


    public bool IsWeaponEquipped { get { return _isWeaponEquipped; } }
}
