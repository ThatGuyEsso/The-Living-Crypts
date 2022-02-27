using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : BaseBossAbility
{
    [SerializeField] private string ReadyUpAnim;
    [SerializeField] private string AttackAnim;
    [Tooltip("How long the ready up pose is held before attack is executed")]
    [SerializeField] private float MaxPoseTime;
    private Animator _animator;
   
    private LimbTargetManager _limbTargetManager;
    private AttackAnimManager _attackAnimManager;
    private SmoothMatchParentRotLoc _smoothMatchParentRot;
    private float currentCoolDown = 0;


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
        if (!_smoothMatchParentRot)
        {
            _smoothMatchParentRot = _owner.GetComponentInChildren<SmoothMatchParentRotLoc>();
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

    public void OnReadyUpBegin()
    {
        if (_attackAnimManager)
        {
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpBegin;
        }
        OnAbilityStarted?.Invoke();

    }
    public void OnReadyUpComplete()
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
        OnAbilityPerformed?.Invoke();
    }

    public void OnAttackEnd()
    {
        _attackAnimManager.OnAttackEnd -= OnAttackEnd;
        StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
    }
  
    public override void Terminate()
    {
        _animator.enabled = false;
        _limbTargetManager.UseInitialTarget();
        _smoothMatchParentRot.ResetChild(5f);
        _currentCooldown = _abilityData.AbilityCooldown;
        OnAbilityFinished?.Invoke();

    }


}
