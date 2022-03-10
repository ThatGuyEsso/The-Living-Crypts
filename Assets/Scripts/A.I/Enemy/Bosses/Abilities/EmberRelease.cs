using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberRelease : BaseBossAbility
{
    [Header("Attack Settings")]
    [SerializeField] private BaseEnemy EnemyPrefab;
    [SerializeField] private int MinToSpawn, MaxToSpawn;
    [SerializeField] private float MinArchHeight, MaxArchHeight;
    [SerializeField] private float MinSpawnRate, MaxSpawnRate;
    [SerializeField] private float MaxInaccuracy;
    [SerializeField] private LayerMask GroundLayers;
    [Header("Attack Animation")]
    [SerializeField] private string ReadyUpAnim;


    [Header("Boss Components")]
    private Animator _animator;
    private LimbTargetManager _limbTargetManager;
    private AttackAnimManager _attackAnimManager;
    private SmoothMatchParentRotLoc[] _smoothMatchParentRots;

    private int _nLeftToSpawn;
    private float _timeToSpawnLeft;
    private bool _isAttacking;
    private Transform _firePoint;
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


        if (_isAttacking)
        {
            if (_nLeftToSpawn <= 0)
            {
          
                _isAttacking = false;

                StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
            }

            if(_timeToSpawnLeft > 0)
            {
                _timeToSpawnLeft -= Time.deltaTime;

            }
            else
            {
                _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
                SpawnNewEmber();
            }

            
        }
 
    }

    public void SpawnNewEmber()
    {
        _nLeftToSpawn--;
        if (!_firePoint)
        {
            _firePoint = _owner.GetFirePoint();

        }
        if (!_owner.GetTaget())
        {
            return;
        }
       
        Vector3 targetPoint =Random.insideUnitSphere*Random.Range(0.0f,MaxInaccuracy)+ _owner.GetTaget().position;

        RaycastHit hitInfo;
        if (Physics.Raycast(targetPoint, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
        {
            targetPoint = hitInfo.point;
        }

        BaseEnemy enemy;
        if (ObjectPoolManager.instance)
        {
            enemy = ObjectPoolManager.Spawn(EnemyPrefab, _firePoint.position, _firePoint.rotation).GetComponent<BaseEnemy>();
        }
        else
        {
            enemy = Instantiate(EnemyPrefab, _firePoint.position, _firePoint.rotation).GetComponent<BaseEnemy>();
        }

        if (!enemy) return;

        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        if (!rb) return;

        ProjectileMaths.LaunchData data = ProjectileMaths.CalculateLaunchData(_firePoint.position, targetPoint
            , Random.Range(MinArchHeight, MaxArchHeight), -Physics.gravity.magnitude);

        rb.velocity= data.initialVelocity;
        enemy.WaitToInit(data.timeToTarget);

        enemy.SetTarget(_owner.GetTaget());
        ChangeCollision changeCollisin= enemy.gameObject.AddComponent<ChangeCollision>();
        changeCollisin.SetLayer(LayerMask.NameToLayer("IgnoreCharacters"));
        changeCollisin.WaitToChangeLayer(LayerMask.NameToLayer("Character"), data.timeToTarget);
        _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
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
            if (_limbTargetManager && _attackAnimManager && _animator)
            {
                _limbTargetManager.UseSelfAsTarget();
                _animator.enabled = true;
                _attackAnimManager.OnReadyUpBegin += OnReadyUpBegin;
                _attackAnimManager.OnReadyUpComplete += OnReadyUpComplete;
                _animator.Play(ReadyUpAnim, 0, 0f);
            }


        }
        else if (!_canAttack && !_abilityData.IsPriority)
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
        Debug.Log("Performing ember release");
        _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
        _nLeftToSpawn = Random.Range(MinToSpawn, MaxToSpawn);
        _isAttacking = true;
        OnAbilityPerformed?.Invoke();
    }

    override protected void OnAttackEnd()
    {
        _attackAnimManager.OnAttackEnd -= OnAttackEnd;

        StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
    }

    public override void Terminate()
    {
        _animator.enabled = false;
        _isAttacking = false;
        _limbTargetManager.UseInitialTarget();
        foreach (SmoothMatchParentRotLoc rotloc in _smoothMatchParentRots)
        {
            rotloc.ResetChild(5f);
        }

        _currentCooldown = _abilityData.AbilityCooldown;
        OnAbilityFinished?.Invoke();

    }

}
