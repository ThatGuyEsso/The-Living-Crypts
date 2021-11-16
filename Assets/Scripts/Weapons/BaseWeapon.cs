using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected float _primaryMinDamage,  _primaryMaxDamage, _secondaryMinDamage, _secondaryMaxDamage;
    [SerializeField] protected Transform _equipTransform;
    [SerializeField] protected float _primaryfireRate,_secondaryFire;


    protected bool _canAttack;
    protected abstract void DoPrimaryAttack();


    protected abstract void DoSecondaryAttack();


    protected abstract void ResetPrimaryAttack();
    protected abstract void ResetSecondaryAttack();

    protected virtual void FollowEquipPoint()
    {
        if (_equipTransform)
        {
            transform.position = _equipTransform.position;
        }
    }
    protected virtual void MatchEquipPointRotation()
    {
        if (_equipTransform)
        {
            transform.rotation = _equipTransform.rotation;
        }
    }

    public virtual void SetEquipPoint(Transform equipTransform)
    {
        _equipTransform = equipTransform;
    }
}
