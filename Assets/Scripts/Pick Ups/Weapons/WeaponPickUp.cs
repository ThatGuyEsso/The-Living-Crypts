using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : BasePickUp
{

    [SerializeField] private GameObject _weaponPrefab;
    protected override void DoPickUp()
    {
        BaseWeapon weapon = Instantiate(_weaponPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseWeapon>();
        if (weapon)
        {
            WeaponManager._instance.EquipWeapon(weapon);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!WeaponManager._instance.IsWeaponEquipped)
            {
                DoPickUp();
            }
        }
    }
}
