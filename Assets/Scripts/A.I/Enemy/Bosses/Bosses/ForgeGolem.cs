using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeGolem : BaseBoss
{
    private WalkMovement _walkMovement;
    [Header("Animation")]
    [SerializeField] protected BossAnimationManager AnimManager;
    public override void Init()
    {
        base.Init();
        if(_hManager) _hManager.IsAlive = false;
        if (!_walkMovement)
        {
            _walkMovement = GetComponent<WalkMovement>();
        }

        if (!AnimManager)
        {
            AnimManager = GetComponent<BossAnimationManager>();
        }
      
    }

  

    protected override void DoAttack(GameObject target, Vector3 point)
    {
    
    }

    protected override void KillEnemy()
    {
        
    }


    protected override void BeginNewStage(BossStage newStage)
    {
        if (!_bossFightRunning) return;
        base.BeginNewStage(newStage);
        switch (newStage)
        {
            case BossStage.First:
                _hManager.IsAlive = true;
                SetUpNewAbilities();
                Debug.Log("First Stage");
                break;
            case BossStage.Second:
                _hManager.IsAlive = true;
                SetUpNewAbilities();
                Debug.Log("Second Stage");
                break;
            case BossStage.Final:
                _hManager.IsAlive = true;
                SetUpNewAbilities();
                Debug.Log("Final Stage");
                break;
            case BossStage.Transition:

                _hManager.IsAlive = false;
                Debug.Log("Transition Stage");
                Invoke("EndTransitionStage", 4f);
                break;
        }

    }
    public override void StartBossFight()
    {
        base.StartBossFight();
        _walkMovement.Init();
        AnimManager.Init();
        AnimManager.PlayIdleAnimation();
    }
    protected override void ProcessAI()
    {
        if (!_bossFightRunning) return;
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:

                if (!CurrentTarget) OnEnemyStateChange(EnemyState.Idle);
                if (_isUsingAttack) return;
                if (!InAbilityRange())
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
                if (_isUsingAttack) return;
                if (!InAbilityRange())
                {
                    OnEnemyStateChange(EnemyState.Chase);
                }
                else 
                {
                    _walkMovement.BeginStop();
                    if (_canUseAttack)
                    {
                        Debug.Log("Golem is calling attack");
                        ExecuteAbility();
                    }
                }
                break;
            case EnemyState.Flee:
                break;
        }
    }
    override protected void Update()
    {
        base.Update();
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget || !PathFinder) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if (!PathFollower.EvaluatePath(_currentPath, transform.position)) return;
                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    _walkMovement.MoveToPoint(PathFollower.GetCurrentPathPoint());
                }

                break;
            case EnemyState.Attack:
                if (_isUsingAttack) return;
                FaceCurrentTarget();

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

    public override void OnEnemyStateChange(EnemyState newState)
    {
        base.OnEnemyStateChange(newState);

        switch (newState)
        {
            case EnemyState.Chase:
                AnimManager.PlayWalkAnimation();
                break;

            case EnemyState.Idle:
                AnimManager.PlayIdleAnimation();

                break;
        }
    }
    public override void ResetEnemy()
    {
        throw new System.NotImplementedException();
    }
}

