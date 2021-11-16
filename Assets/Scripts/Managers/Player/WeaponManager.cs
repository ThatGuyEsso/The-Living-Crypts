using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager _instance;

    public static BaseWeapon _equippedWeapon;
    private bool _isWeaponEquipped;

    [SerializeField] private Transform _weaponEquipPoint;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if (!_instance)
        {
            _instance = this;
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
        }
        else
        {
            UnequipWeapon();
            EquipWeapon(weapon);
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


    public bool IsWeaponEquipped { get { return _isWeaponEquipped; } }
}
