using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpMovement))]
public class LivingEmber : BaseEnemy
{
    [Header("Enemy Character Settings")]
    [SerializeField] private JumpData AttackJumpSettings;
    private JumpMovement _jumpMovement;
    private CharacterHealthManager _hManager;
    
    public override void Init()
    {
        base.Init();
        if (!_jumpMovement) _jumpMovement = GetComponent<JumpMovement>();
        _hManager = GetComponent<CharacterHealthManager>();
        if (!_hManager) Destroy(this);
        else
        {
            _hManager.OnHurt += OnHurt;
            _hManager.OnNotHurt += OnNotHurt;
            _hManager.OnDie += KillEnemy;
        }
 
    }

    protected override void DoAttack(GameObject target)
    {
        IDamage damagable = target.GetComponent<IDamage>();
        if(damagable != null)
        {
            damagable.OnDamage(CharacterSettings.GetRandomDamage(),transform.forward, CharacterSettings.GetRandomKnockBack(), gameObject);
            _canAttack = false;
            _currentTimeBtwnAttacks = CharacterSettings.AttackRate;
        }
    }


    protected override void ProcessAI()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:

                if (!CurrentTarget) OnEnemyStateChange(EnemyState.Idle);
                if (!InRange())
                {
                    DrawPathToTarget();
                    PathFollower.EvaluatePath(_currentPath, transform.position);
                }
                else
                {
                    OnEnemyStateChange(EnemyState.Attack);
                }
            
                break;
            case EnemyState.Attack:
                if (!CurrentTarget) OnEnemyStateChange(EnemyState.Idle);
                if (!InRange())
                {
                    if (!CurrentTarget) OnEnemyStateChange(EnemyState.Chase);
                }
                break;
            case EnemyState.Flee:
                break;
        }


    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget||!PathFinder) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if(!PathFollower.EvaluatePath(_currentPath,transform.position)) return;
                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    _jumpMovement.DoJump((transform.forward).normalized);
                }
                
                break;
            case EnemyState.Attack:
                FaceCurrentTarget();
          
          
                if(_canAttack)
                    _jumpMovement.DoJump((transform.forward).normalized,AttackJumpSettings);
           
                break;
            case EnemyState.Flee:
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")){
            if (_canAttack && !_hManager.IsHurt)
            {
                DoAttack(other.gameObject);
            }
        }
    }

    public void OnHurt()
    {
        OnEnemyStateChange(EnemyState.Idle);
    }
    public void OnNotHurt()
    {
        OnEnemyStateChange(EnemyState.Chase);
    }

    private void OnDestroy()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
    }

    private void OnDisable()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
    }

    protected override void KillEnemy()
    {
        Destroy(gameObject);
    }
}