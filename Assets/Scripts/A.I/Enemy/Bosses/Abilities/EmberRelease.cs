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
    [SerializeField] private string ReadyUpAnim, EndAnim;


    [Header("Boss Components")]
    private Animator _animator;

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

   
        if (!_attackAnimManager)
        {
            _attackAnimManager = _owner.GetComponent<AttackAnimManager>();


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

                StartCoroutine(WaitToReset(HoldFinalPoseTime));
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
        IsActive = true;
        if (_canAttack)
        {
            Debug.Log("Quake is calling attack");
            _canAttack = false;
            if (_attackAnimManager && _animator)
            {
         
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
        if (!IsActive)
        {
            return;
        }
        if (_attackAnimManager)
        {
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpBegin;
        }
        OnAbilityStarted?.Invoke();

    }
    override protected void OnReadyUpComplete()
    {
        if (!IsActive)
        {
            return;
        }
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
        Debug.Log("Performing ember release");
        _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
        _nLeftToSpawn = Random.Range(MinToSpawn, MaxToSpawn);
        _isAttacking = true;
        OnAbilityPerformed?.Invoke();
    }

    override protected void OnAttackEnd()
    {
        if (!IsActive)
        {
            return;
        }
        _attackAnimManager.OnAttackEnd -= OnAttackEnd;

        StartCoroutine(WaitToEndAttack(HoldFinalPoseTime));
    }
    protected override void OnReset()
    {
        if (!IsActive)
        {
            return;
        }
        _animator.Play(EndAnim, default, 0f);
        _attackAnimManager.OnAnimEnd += Terminate;
    }
    public override void Terminate()
    {

        _isAttacking = false;
        IsActive = false;

        _attackAnimManager.OnAnimEnd -= Terminate;
        _currentCooldown = _abilityData.AbilityCooldown;
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
