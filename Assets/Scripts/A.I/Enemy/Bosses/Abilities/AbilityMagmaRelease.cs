using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMagmaRelease : BaseBossAbility
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private GameObject HitZonePrefab;
    [SerializeField] private int MinToSpawn, MaxToSpawn;
    [SerializeField] private float MinArchHeight, MaxArchHeight;
    [SerializeField] private float MinSpawnRate, MaxSpawnRate;
    [SerializeField] private float MaxInaccuracy;
    [SerializeField] private LayerMask GroundLayers;
    [Header("Attack Animation")]
    [SerializeField] private string ReadyUpAnim,EndAnim;


    [Header("Boss Components")]
    private Animator _animator;

    private AttackAnimManager _attackAnimManager;


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
        if (!_canAttack )
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


        if (_isAttacking)
        {
            if (_nLeftToSpawn <= 0)
            {

                _isAttacking = false;

                StartCoroutine(WaitToReset(HoldFinalPoseTime));
            }

            if (_timeToSpawnLeft > 0)
            {
                _timeToSpawnLeft -= Time.deltaTime;

            }
            else
            {
                _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
                FireProjectile();
            }


        }

    }

    public void FireProjectile()
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

        Vector3 targetPoint = Random.insideUnitSphere * Random.Range(0.0f, MaxInaccuracy) + _owner.GetTaget().position;

        RaycastHit hitInfo;
        if (Physics.Raycast(targetPoint, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
        {
            targetPoint = hitInfo.point;
        }

        GameObject projectileObject;
        if (ObjectPoolManager.instance)
        {
            projectileObject = ObjectPoolManager.Spawn(ProjectilePrefab, _firePoint.position, _firePoint.rotation);
        }
        else
        {
            projectileObject = Instantiate(ProjectilePrefab, _firePoint.position, _firePoint.rotation);
        }

        if (!projectileObject) return;

        IProjectile projectile = projectileObject.GetComponent<IProjectile>();

        if (projectile == null)
        {
            return;
        }
  

        ProjectileMaths.LaunchData data = ProjectileMaths.CalculateLaunchData(_firePoint.position, targetPoint
            , Random.Range(MinArchHeight, MaxArchHeight), -Physics.gravity.magnitude);

        
        float dmg = Random.Range(_abilityData.MinAttackDamage, _abilityData.MaxAttackDamage);
        float kBack = Random.Range(_abilityData.MinKnockBack, _abilityData.MaxKnockBack);
        projectile.ShootProjectile(new ProjectileData(dmg, data.initialVelocity.normalized,
            data.initialVelocity.magnitude, data.timeToTarget + 2.0f, kBack, _owner.gameObject));


        SpawnTargetAreaVFX(targetPoint, projectileObject);

        _timeToSpawnLeft = Random.Range(MinSpawnRate, MaxSpawnRate);
    }

    private void SpawnTargetAreaVFX(Vector3 hitPoint, GameObject projectile)
    {
        if (HitZonePrefab)
        {
            ProjectileTargetVFX vfx;
            if (ObjectPoolManager.instance)
            {
                vfx = ObjectPoolManager.Spawn(HitZonePrefab, hitPoint, Quaternion.identity).GetComponent<ProjectileTargetVFX>();
            }
            else
            {
                vfx = Instantiate(HitZonePrefab, hitPoint, Quaternion.identity).GetComponent<ProjectileTargetVFX>();
            }

            BaseProjectile proj = projectile.GetComponent<BaseProjectile>();
            if (proj && vfx)
            {
                vfx.Init(proj); 
            }
        }

    }

    protected override void OnReset()
    {
        _attackAnimManager.OnAnimEnd += Terminate;
        _animator.Play(EndAnim, default, 0f);
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
            if (  _attackAnimManager && _animator)
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

    public override void Terminate()
    {
        if (!IsActive)
        {
            return;
        }
        IsActive = false;
        _isAttacking = false;
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
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpComplete;
            _attackAnimManager.OnReadyUpBegin -= OnReadyUpBegin;
            _attackAnimManager.OnAttackEnd -= OnAttackEnd;
            _attackAnimManager.OnAnimEnd -= Terminate;
        }

        _currentCooldown = _abilityData.AbilityCooldown;
    }
}
