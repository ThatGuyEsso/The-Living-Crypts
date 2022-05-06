using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PathFollower))]
[RequireComponent(typeof(WalkMovement))]
[RequireComponent(typeof(CryptCharacterManager))]
[RequireComponent(typeof(GolemAnimationController))]
public class SupportGolem : BaseEnemy , IAttacker
{

    private WalkMovement _walkMovement;
    private CryptCharacterManager _cryptCharacter;
    [SerializeField] private LayerMask GroundLayers;
    private AttackCollider[] _attackColliders;
    private SmoothMatchParentRotLoc[] _matchToLoc;
    private bool _isAttacking;

    private GolemAnimationController _animController;
    public override void Init()
    {
        base.Init();
        if (!_walkMovement)
        {
            _walkMovement = GetComponent<WalkMovement>();
            _walkMovement.Init();
        }

        if (!_cryptCharacter)
        {
            _cryptCharacter = GetComponent<CryptCharacterManager>();
        }
        _attackColliders = GetComponentsInChildren<AttackCollider>();
    
        if (_attackColliders.Length > 0)
        {
            foreach (AttackCollider collider in _attackColliders)
            {
                collider.SetOwner(gameObject);
            }
        }

        _matchToLoc = GetComponentsInChildren<SmoothMatchParentRotLoc>();


   
        if (! _animController)
        {
            _animController = GetComponent<GolemAnimationController>();
            _animController.Init();
        }
        ResetEnemy();
    }


    protected override void DoAttack(GameObject target, Vector3 point)
    {
        if(!_isAttacking && _animController && !_animController.IsPlayingAttackAnimation())
        {
            _canAttack = false;
            _animController.PlayAttackAnim();
            
        }
    }

    protected override void KillEnemy()
    {
        base.KillEnemy();
        _cryptCharacter.RemoveSelf();
        PlaySFX(KilledSFX, true);
        if (DeathVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(DeathVFX, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(DeathVFX, transform.position, Quaternion.identity);
            }
        }
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    protected override void ProcessAI()
    {
        if (!IsActive)
        {
            return;
        }
        if (_isAttacking)
        {
            return;
        }
        switch (CurrentState)
        {
            case EnemyState.Idle:
                if (CurrentTarget)
                {
                    OnEnemyStateChange(EnemyState.Chase);

                }
                break;
            case EnemyState.Chase:

                if (!CurrentTarget)
                {
                    OnEnemyStateChange(EnemyState.Chase);
               
                }

                if (InRange()) 
                {
                    OnEnemyStateChange(EnemyState.Attack);
                }


                 DrawPathToTarget();
                PathFollower.EvaluatePath(_currentPath, transform.position);

                break;
            case EnemyState.Attack:
                if (!CurrentTarget)
                {
                    OnEnemyStateChange(EnemyState.Idle);
                 
                }
                if (!InRange())
                {
                    OnEnemyStateChange(EnemyState.Chase);
                }
                break;
        }
    }
    public void StartAttack()
    {
        _isAttacking = true;
    }
    public void StopAttack()
    {
        _isAttacking = false;
     
    }
    public void EnableAttackColliders()
    {
        foreach (AttackCollider collider in _attackColliders)
        {
            collider.ToggleColiider(true);
        }
    }
    public void DisableAttackColliders()
    {
        foreach (AttackCollider collider in _attackColliders)
        {
            collider.ToggleColiider(false);
        }
    }
    private void Update()
    {
        if (!IsActive)
        {
            return;
        }
        if (transform.position.y < -16f)
        {
            KillEnemy();
        }
        if (_isAttacking)
        {
            return;
        }
        switch (CurrentState)
        {
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget || !PathFinder)
                {
                    OnEnemyStateChange(EnemyState.Idle);
                    return;
                }
                if (_currentPath.corners.Length <= 0)
                {
                  
                    return;
                }
                //evaluate path
                if (!PathFollower.EvaluatePath(_currentPath, transform.position))
                {
                    return;
                }


                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    _walkMovement.MoveToPoint(PathFollower.GetCurrentPathPoint());
                }
              

                break;
            case EnemyState.Attack:
                if (!CurrentTarget)
                {
                    return;
                }
                FaceCurrentTarget();
              
                _walkMovement.MoveToPoint(CurrentTarget.position);
                if (InRange() && _canAttack)
                {
                    DoAttack(CurrentTarget.gameObject, CurrentTarget.position);
                }
                else if(InRange())
                {
                    if (!_isAttacking && _animController)
                    {
                        _animController.Stop();
                    }
                }
           

                break;
        
        }

        if (!_canAttack)
        {
            if (_currentTimeBtwnAttacks <= 0f)
            {
                _canAttack = true;
            }
            else
            {
                _currentTimeBtwnAttacks -= Time.deltaTime;
            }
        }
    }

    public override void OnEnemyStateChange(EnemyState newState)
    {
        base.OnEnemyStateChange(newState);

        switch (newState)
        {
            case EnemyState.Idle:
                if (!_isAttacking && _animController)
                {
                    _animController.Stop();
                }
                if (_walkMovement)
                {
                    _walkMovement.BeginStop();
                }
                if (_matchToLoc.Length > 0)
                {
                    foreach (SmoothMatchParentRotLoc match in _matchToLoc)
                    {
                        match.ResetChild(5f);
                    }
                }
                break;
            case EnemyState.Chase:
                if (_matchToLoc.Length > 0)
                {
                    foreach (SmoothMatchParentRotLoc match in _matchToLoc)
                    {
                        match.Stop();
                    }
                }
                if (!_isAttacking && _animController )
                {
                    _animController.PlayWalkCycle();
                }
                break;
            case EnemyState.Attack:
                if (_walkMovement)
                {
                    _walkMovement.BeginStop();
                }
                if (_matchToLoc.Length > 0)
                {
                    foreach (SmoothMatchParentRotLoc match in _matchToLoc)
                    {
                        match.Stop();
                    }
                }
                break;
      
            case EnemyState.Flee:
                break;
        }
    }
    protected override void EvaluateNewGameplayEvent(GameplayEvents newEvent)
    {
        base.EvaluateNewGameplayEvent(newEvent);

        switch (newEvent)
        {
            case GameplayEvents.PlayerDied:

                IsActive = false;
                break;

            case GameplayEvents.PlayerRespawnBegun:

                if (ObjectPoolManager.instance)
                {
                    if (gameObject && gameObject.activeInHierarchy)
                    {
                        ObjectPoolManager.Recycle(gameObject);
                    }


                }
                else
                {
                    if (gameObject)
                    {
                        Destroy(gameObject);
                    }

                }
                break;
        }
    }

    public void OnHurt()
    {
     
        PlaySFX(HurtSFX, true);
        if (HurtVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(HurtVFX, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(HurtVFX, transform.position, Quaternion.identity);
            }
        }
    }
    public void OnNotHurt()
    {
        OnEnemyStateChange(EnemyState.Chase);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
  
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
    }
    protected override void DrawPathToTarget()
    {
        if (!CurrentTarget || !PathFinder) return;


        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
        {

            _currentPath = PathFinder.GetPathToTarget(hitInfo.point, CurrentTarget.position, UnityEngine.AI.NavMesh.AllAreas);
        }

    }

    public void OnCollisionEnter(Collision other)
    {
        if (!CurrentTarget)
        {
            return;
        }
        if(other.transform.root.gameObject == CurrentTarget.gameObject)
        {
            OnEnemyStateChange(EnemyState.Attack);
        }
    }

    public AttackData GetAttackData()
    {
        AttackData data = new AttackData(CharacterSettings.MinDamage,
            CharacterSettings.MaxDamage, CharacterSettings.MinDamage, CharacterSettings.MaxDamage);
        return data;
    }

    public override void ResetEnemy()
    {
        if (_hManager)
        {
            _hManager.Init();
            _hManager.OnHurt += OnHurt;
            _hManager.OnNotHurt += OnNotHurt;
            _hManager.OnDie += KillEnemy;
        }

    }
}
