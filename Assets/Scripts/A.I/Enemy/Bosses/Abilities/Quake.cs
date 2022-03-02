using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : BaseBossAbility
{
    [SerializeField] private string ReadyUpAnim;
    [SerializeField] private string AttackAnim;


    private Animator _animator;
   
    private LimbTargetManager _limbTargetManager;
    private AttackAnimManager _attackAnimManager;
    private SmoothMatchParentRotLoc[] _smoothMatchParentRots;



    public override void Init()
    {
        base.Init();
        if (!_animator)
        {
            _animator = _owner.GetComponent<Animator>();
        }

        if (!_limbTargetManager)
        {
            _limbTargetManager = _owner.GetComponent<LimbTargetManager>();
        }
        if (!_attackAnimManager)
        {
            _attackAnimManager = _owner.GetComponent<AttackAnimManager>();


        }
        if (_smoothMatchParentRots ==null)
        {
            _smoothMatchParentRots = _owner.GetComponentsInChildren<SmoothMatchParentRotLoc>();
        }
    }

    private void Update()
    {
        if(!_canAttack && currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
            if (currentCoolDown <= 0)
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

        if (_canAttack)
        {
            Debug.Log("Quake is calling attack");
            _canAttack = false;
            if (_limbTargetManager&& _attackAnimManager&& _animator)
            {
                _limbTargetManager.UseSelfAsTarget();
                _animator.enabled = true;
                _attackAnimManager.OnReadyUpBegin += OnReadyUpBegin;
                _attackAnimManager.OnReadyUpComplete += OnReadyUpComplete;
                _animator.Play(ReadyUpAnim, 0, 0f);
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
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpComplete;
        }
        StartCoroutine(WaitToExecuteAttack(MaxPoseTime));

    }
    public override void PerformAttack()
    {
        if (_attackAnimManager)
        {
            _attackAnimManager.OnAttackEnd += OnAttackEnd;
        }
        _animator.Play(AttackAnim, 0, 0f);
        _owner.ToggleLimbAttackColliders(true);
        OnAbilityPerformed?.Invoke();
    }

    override protected void OnAttackEnd()
    {
        _attackAnimManager.OnAttackEnd -= OnAttackEnd;
        _owner.ToggleLimbAttackColliders(false);
        StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
    }
  
    public override void Terminate()
    {
        _animator.enabled = false;
        _limbTargetManager.UseInitialTarget();
        foreach(SmoothMatchParentRotLoc rotloc in _smoothMatchParentRots)
        {
            rotloc.ResetChild(5f);
        }
     
        _currentCooldown = _abilityData.AbilityCooldown;
        OnAbilityFinished?.Invoke();

    }


}
