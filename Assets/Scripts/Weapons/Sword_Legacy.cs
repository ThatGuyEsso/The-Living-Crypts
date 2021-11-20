using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Legacy : BaseWeapon
{
    //Collider
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;
    private bool _isSwingingRight = false;
    private float _primCurrTimeToIdle;
    private float _secCurrTimeToIdle;

    public override void Init()
    {
        base.Init();
        if (!_attackCollider) Debug.LogError("Sword has Not attack Collider");
        else _attackCollider.enabled = false;

        if (_animController)
        {
            _animController.OnAttackAnimBegin += ActivateAttackCollider;
            _animController.OnAttackAnimEnd += ResetPrimaryAttack;
        }
    }
    public override void StopTryToPrimaryAttack()
    {
        _isPrimaryAttacking = false;
    }

    public override void StopTryToSecondaryAttack()
    {
        _isSecondaryAttacking = false;
    }

    public override void TryToPrimaryAttack()
    {
        
        _isPrimaryAttacking = true;
  
        if (_canPrimaryAttack&&_canAttack)
        {
            DoPrimaryAttack();
        }
    }

    public override void TryToSecondaryyAttack()
    {
        _isSecondaryAttacking = true;
        if (_canSecondaryAttack&& _canAttack)
        {
            DoSecondaryAttack();
        }
    }

    protected override void DoPrimaryAttack()
    {
        _canPrimaryAttack = false;
        if (_isSwingingRight)
        {
            _animController.PlayPrimaryAttackAnimation(1);
            _isSwingingRight = false;
        }
        else
        {
            _animController.PlayPrimaryAttackAnimation(0);
            _isSwingingRight = true;
        }

 
        Debug.Log("Primary Attack");
    }

    public void ActivateAttackCollider()
    {
        if (_attackCollider)
            if (_attackCollider.enabled)
                _attackCollider.enabled = true;
    }

    protected override void DoSecondaryAttack()
    {
        Debug.Log("Secondary Attack");
    }

  
    protected override void ResetPrimaryAttack()
    {
        _canPrimaryAttack = true;
        _primCurrTimeToIdle = _primaryTimeToIdle;
        if (_attackCollider)
            if(_attackCollider.enabled)
                _attackCollider.enabled = false;
    }

    protected override void ResetSecondaryAttack()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {

        //Secondary attack has cool down
        if (!_canSecondaryAttack)
        {
            if(_secondaryCurrentCooldownTime <= 0f)
            {
                _secondaryCurrentCooldownTime = _secondaryFire;
                _canSecondaryAttack = true;
            }
            else
            {
                _secondaryCurrentCooldownTime -= Time.deltaTime;
            }
        }

        if (_primCurrTimeToIdle > 0)
        {
            _primCurrTimeToIdle -= Time.deltaTime;
            if (_primCurrTimeToIdle <=0f)
            {
                _isSwingingRight = false;
                _animController.StopAnimating();
            }
        }
    }
    void FixedUpdate()
    {
        FollowEquipPoint();
        MatchEquipPointRotation();
    }
}
