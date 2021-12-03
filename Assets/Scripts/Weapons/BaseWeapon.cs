using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected string _weaponName;
    [SerializeField] protected float _primaryMinDamage,  _primaryMaxDamage, _secondaryMinDamage, _secondaryMaxDamage;
    [SerializeField] protected float _primaryMinKnockback, _primaryMaxKnockback, _secondaryMinKnockback, _secondaryMaxKnockback;
    [SerializeField] protected Transform _equipTransform;
    [SerializeField] protected float _primaryFireRate,_secondaryFireRate;

    [SerializeField] protected WeaponAnimController _animController;
    protected bool _canPrimaryAttack,_canSecondaryAttack,_canAttack;
    protected bool _isPrimaryAttacking, _isSecondaryAttacking;
    protected float _primaryCurrentCooldownTime, _secondaryCurrentCooldownTime;
    protected Vector3 _equipOffset;
    public virtual void StopTryToPrimaryAttack()
    {
        _isPrimaryAttacking = false;
    }

    public virtual void StopTryToSecondaryAttack()
    {
        _isSecondaryAttacking = false;
    }


    public virtual void TryToPrimaryAttack()
    {

        _isPrimaryAttacking = true;

        ValidatePrimaryAttack();
    }


    public virtual void TryToSecondaryAttack()
    {
        _isSecondaryAttacking = true;

    }

    protected abstract void DoPrimaryAttack();


    protected bool _isOwnerMoving;

    protected abstract void DoSecondaryAttack();


    protected abstract void ResetPrimaryAttack();
    protected abstract void ResetSecondaryAttack();
    public virtual void Init()
    {
        _canPrimaryAttack = true;
        _canSecondaryAttack = true;
        _canAttack = true;
        if (!_animController)
            _animController = GetComponent<WeaponAnimController>();
        _animController.Init();
    }
    protected virtual void FollowEquipPoint()
    {
        if (_equipTransform)
        {
            transform.position =  _equipTransform.position;
        }
    }
    protected virtual void LerpToEquipoint()
    {
        if (_equipTransform)
        {
            transform.position =Vector3.MoveTowards(transform.position, _equipTransform.position,Time.deltaTime*5f);
        }
    }
    protected virtual void MatchEquipPointRotation()
    {
        if (_equipTransform)
        {
            transform.rotation = _equipTransform.rotation;
        }
    }
    virtual public void ValidatePrimaryAttack()
    {
        if (_canPrimaryAttack && _canAttack && !_isSecondaryAttacking)
        {
            DoPrimaryAttack();
        }
    }

    virtual public void ValidateSecondaryAttack()
    {
        if (_canSecondaryAttack && _canAttack && !_isPrimaryAttacking)
        {
            DoSecondaryAttack();
        }
    }
    public virtual void SetEquipPoint(Transform equipTransform)
    {
        _equipTransform = equipTransform;
    }


    public void SetIsMoving(bool isMoving)
    {
        _isOwnerMoving = isMoving;
    }
}
