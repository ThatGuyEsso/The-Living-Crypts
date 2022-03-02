using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseBossAbility
{
    [SerializeField] private string ReadyUpAnim;
    [SerializeField] private string AttackAnim;
    [SerializeField] private float MaxChargeDistance;
    [SerializeField] private GameObject ChargeColliderPrefab;
    [SerializeField] private LayerMask StopChargeLayers;
    [SerializeField] private Vector3 ChargeColliderOffset;
    [SerializeField] private float ChargeSpeed;
    private Animator _animator;

    private LimbTargetManager _limbTargetManager;
    private WalkMovement _movement;
    private AttackAnimManager _attackAnimManager;
    private SmoothMatchParentRotLoc[] _smoothMatchParentRots;
    private FaceDirection _faceDirection;
    private AttackCollider _chargeCollider;
    private Vector3 ChargePoint;
    private float _defaultSpeed;
    private float _defaultStoppingDistance;
    private bool _canRotate;
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
        if (_smoothMatchParentRots == null)
        {
            _smoothMatchParentRots = _owner.GetComponentsInChildren<SmoothMatchParentRotLoc>();
        }
        if (!_movement)
        {
            _movement = _owner.GetComponent<WalkMovement>();
        }




    }

    

    public void GetChargePoint()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(_owner.transform.position, _owner.transform.forward, out hitInfo, MaxChargeDistance,
            StopChargeLayers))
        {
            ChargePoint = hitInfo.point;
        }
        else
        {
            ChargePoint = _owner.transform.forward * MaxChargeDistance;
        }

    }
    private void SetupAttackCollider()
    {
        if (!ChargeColliderPrefab)
        {
            Debug.LogError("Missing charge collider prefab");
            return;
        }
        if (!_chargeCollider)
        {
            if (ObjectPoolManager.instance)
            {
                _chargeCollider = ObjectPoolManager.Spawn(ChargeColliderPrefab, transform, transform.position + ChargeColliderOffset).GetComponent<AttackCollider>();

            }
            else
            {
                _chargeCollider = Instantiate(ChargeColliderPrefab, transform.position + ChargeColliderOffset, Quaternion.identity).GetComponent<AttackCollider>();
                _chargeCollider.transform.SetParent(transform);
            }
            _chargeCollider.SetOwner(_owner.gameObject);
            _chargeCollider.OnObjectHit += EvaluateObjectHit;
            _chargeCollider.OnAttackPerfomed += OnAttackEnd;
        }
    }

    private void EvaluateObjectHit(GameObject other)
    {
        if(StopChargeLayers == (StopChargeLayers | (1 << other.gameObject.layer)))
        {
            OnAttackEnd();
        }
    }
    private void Update()
    {
        if (!_canAttack && currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
            if (currentCoolDown <= 0)
            {
                _canAttack = true;
            }
        }

        if (_canRotate)
        {
            _owner.FaceCurrentTarget();
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
        _canRotate = false;
        GetChargePoint();
        StartCoroutine(WaitToExecuteAttack(MaxPoseTime));

    }
    public override void PerformAttack()
    {

        //SetupAttackCollider();
        _defaultSpeed = _movement.GetMaxSpeed();
        _defaultStoppingDistance = _movement.GetStoppingDistance();
        _movement.SetMaxSpeed(ChargeSpeed);
        _movement.SetStoppingDistance(0f);

        _movement.MoveToPoint(ChargePoint);
        _animator.Play(AttackAnim, 0, 0f);
        OnAbilityPerformed?.Invoke();
    }

    override protected void OnAttackEnd()
    {
        _chargeCollider.OnObjectHit -= EvaluateObjectHit;
        _chargeCollider.OnAttackPerfomed -= OnAttackEnd;

        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(_chargeCollider.gameObject);
        }
        else
        {
            Destroy(_chargeCollider.gameObject);
        }
        _chargeCollider = null;
        _movement.SetMaxSpeed(_defaultSpeed);
        _movement.SetStoppingDistance(_defaultStoppingDistance);

        _movement.BeginStop();
        StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
    }


    public override void Execute()
    {
        if (!_isInitialised)
        {
            Init();
        }

        if (_canAttack)
        {
          
            _canAttack = false;
            if (_limbTargetManager && _attackAnimManager && _animator)
            {
                _limbTargetManager.UseSelfAsTarget();
                _animator.enabled = true;
                _attackAnimManager.OnReadyUpBegin += OnReadyUpBegin;
                _attackAnimManager.OnReadyUpComplete += OnReadyUpComplete;
                _animator.Play(ReadyUpAnim, 0, 0f);
                _canRotate = true;
            }


        }
        else if (!_canAttack && !_abilityData.IsPriority)
        {
            _owner.SkipAttack();
        }
    }

    public override void Terminate()
    {
        _animator.enabled = false;
        _limbTargetManager.UseInitialTarget();
        foreach (SmoothMatchParentRotLoc rotloc in _smoothMatchParentRots)
        {
            rotloc.ResetChild(5f);
        }

        _currentCooldown = _abilityData.AbilityCooldown;
        OnAbilityFinished?.Invoke();
    }
}
