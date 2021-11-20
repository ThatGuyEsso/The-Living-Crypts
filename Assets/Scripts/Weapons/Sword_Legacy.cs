using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Legacy : BaseWeapon
{
    //Collider
    [SerializeField] private Collider _attackCollider;

    [Header("Animation Settings")]
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;
    [SerializeField] private SmoothMatchParentRotLoc _idleReset;
    private bool _isSwingingRight = false;

    private float _primCurrTimeToIdle;
    private float _secCurrTimeToIdle;
    private bool _isAttacking;
    public override void Init()
    {
        base.Init();
        if (!_attackCollider) Debug.LogError("Sword has Not attack Collider");
        else _attackCollider.enabled = false;

        if (_animController)
        {
            _animController.OnAttackAnimBegin += ActivateAttackCollider;
         
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
  
        if (_canPrimaryAttack&&_canAttack&&!_isAttacking)
        {
            DoPrimaryAttack();
        }
    }

    public override void TryToSecondaryAttack()
    {
        _isSecondaryAttacking = true;
        if (_canSecondaryAttack&& _canAttack&&!_isAttacking)
        {
            DoSecondaryAttack();
        }
    }

    protected override void DoPrimaryAttack()
    {
        if (_idleReset) _idleReset.Stop();
        _canPrimaryAttack = false;
        _isAttacking = true;

        if (_animController)
        {
            _animController.OnAttackAnimEnd += ResetPrimaryAttack;

        }
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

        ResetIdleTimers();
        Debug.Log("Primary Attack");
    }

    public void ActivateAttackCollider()
    {
        if (_attackCollider)
            if (!_attackCollider.enabled)
                _attackCollider.enabled = true;
    }

    protected override void DoSecondaryAttack()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd += OnSecondaryAttackEnd;

        }
        if (_idleReset) _idleReset.Stop();
        _canSecondaryAttack = false;
        _isAttacking = true;
        _animController.PlaySecondaryAttackAnimation(0);
        ResetIdleTimers();

        Debug.Log("Secondary Attack");
    }

    public void ResetIdleTimers()
    {
        _primCurrTimeToIdle = 0f;

        _secCurrTimeToIdle = 0f;
    }
    protected override void ResetPrimaryAttack()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd -= ResetPrimaryAttack;

        }
        _canPrimaryAttack = true;
        _isAttacking = false;
        _primCurrTimeToIdle = _primaryTimeToIdle;
        if (_attackCollider)
            if(_attackCollider.enabled)
                _attackCollider.enabled = false;
    }


    public void OnSecondaryAttackEnd()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd -= OnSecondaryAttackEnd;

        }
        _secondaryCurrentCooldownTime = _secondaryFire;
        _secCurrTimeToIdle = _secondaryTimeToIdle;
        _isAttacking = false;
        if (_attackCollider)
            if (_attackCollider.enabled)
                _attackCollider.enabled = false;
    }

    protected override void ResetSecondaryAttack()
    {
        _canSecondaryAttack = true;
   
    }

    private void Update()
    {
        if (!_isOwnerMoving)
            LerpToEquipoint();
        //Secondary attack has cool down
        if (!_canSecondaryAttack&& _secondaryCurrentCooldownTime>0)
        {
            _secondaryCurrentCooldownTime -= Time.deltaTime;
            if (_secondaryCurrentCooldownTime <= 0f)
            {
                
                ResetSecondaryAttack();
            }
         
        }

        if (_primCurrTimeToIdle > 0)
        {
            _primCurrTimeToIdle -= Time.deltaTime;
            if (_primCurrTimeToIdle <=0f)
            {
                _isSwingingRight = false;
                _animController.StopAnimating();
                if (_idleReset) _idleReset.ResetChild(5f);
            }
        }
        if (_secCurrTimeToIdle > 0)
        {
            _secCurrTimeToIdle -= Time.deltaTime;
            if (_secCurrTimeToIdle <= 0f)
            {
           
                _animController.StopAnimating();
                if (_idleReset) _idleReset.ResetChild(5f);
            }
        }
    }
    void FixedUpdate()
    {
        if(_isOwnerMoving)
            FollowEquipPoint();
        MatchEquipPointRotation();
    }
}
