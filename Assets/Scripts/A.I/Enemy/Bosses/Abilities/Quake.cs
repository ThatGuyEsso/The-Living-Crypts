using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : BaseBossAbility
{
    [SerializeField] private string ReadyUpAnim,EndAnim;
    [SerializeField] private string AttackAnim;


    private Animator _animator;
   
   
    private AttackAnimManager _attackAnimManager;

    

    public override void Init()
    {
        base.Init();
        if (!_animator)
        {
            _animator = _owner.GetComponent<Animator>();
        }


        if (!_attackAnimManager)
        {
            _attackAnimManager = _owner.GetComponent<AttackAnimManager>();


        }
 
    }

    private void Update()
    {
        if(!_canAttack )
        {
            if( currentCoolDown > 0)
            {
                currentCoolDown -= Time.deltaTime;
            }
           
            else 
            {
                _canAttack = true;
            }


        }

    
    }

    public override void Execute()
    {
        if (!_isInitialised)
        {
            Init();
        }
        if (!_abilityData.IsPriority)
        {
            currentTimeToSkip = _abilityData.MaxTimeToAttempt;
        }
        IsActive = true;
        if (_canAttack)
        {
            Debug.Log("Quake is calling attack");
            _canAttack = false;
            if ( _attackAnimManager&& _animator)
            {
       
                _animator.enabled = true;
                _attackAnimManager.OnReadyUpBegin += OnReadyUpBegin;
                _attackAnimManager.OnReadyUpComplete += OnReadyUpComplete;
                _animator.Play(ReadyUpAnim, default, 0f);
            }
      

        }
        else if(!_canAttack && !_abilityData.IsPriority)
        {
            _owner.SkipAttack();
        }

    }

    override protected void OnReadyUpBegin()
    {

   
        if (_attackAnimManager)
        {
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpBegin;
        }
        OnAbilityStarted?.Invoke();

    }
    override protected void OnReadyUpComplete()
    {
        if (_attackAnimManager)
        {
            _attackAnimManager.OnReadyUpComplete -= OnReadyUpComplete;
        }
        StartCoroutine(WaitToExecuteAttack(MaxPoseTime));

    }

    public override void PerformAttack()
    {
        if (!IsActive)
        {
            return;
        }

        if (_attackAnimManager)
        {
            _attackAnimManager.OnAttackEnd += OnAttackEnd;
        }
        _animator.Play(AttackAnim, default, 0f);
        _owner.ToggleLimbAttackColliders(true);
        OnAbilityPerformed?.Invoke();
    }

    override protected void OnAttackEnd()
    {
        if (!IsActive)
        {
            return;
        }
        _attackAnimManager.OnAttackEnd -= OnAttackEnd;
        _owner.ToggleLimbAttackColliders(false);
        StartCoroutine(WaitToReset(HoldFinalPoseTime));
    }

    override protected void OnReset()
    {

        _animator.Play(EndAnim, default, 0f);
        _attackAnimManager.OnAnimEnd += Terminate;
    }



    public override void Terminate()
    {

  
        IsActive = false;
        if (_attackAnimManager)
        {
            _attackAnimManager.OnAnimEnd -= Terminate;
            _currentCooldown = _abilityData.AbilityCooldown;

        }

        OnAbilityFinished?.Invoke();

    }

    public override void CancelAttack()
    {
        IsActive = false;
        StopAllCoroutines();
        if (_attackAnimManager)
        {
            _attackAnimManager.OnReadyUpComplete -= OnReadyUpComplete;
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpBegin;
            _attackAnimManager.OnAttackEnd -= OnAttackEnd;
            _attackAnimManager.OnAnimEnd -= Terminate;
        }
   
        _currentCooldown = _abilityData.AbilityCooldown;
    }


  
}
