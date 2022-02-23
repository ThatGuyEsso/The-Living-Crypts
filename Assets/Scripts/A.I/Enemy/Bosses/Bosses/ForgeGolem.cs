using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeGolem : BaseBoss
{
    private WalkMovement walkMovement;
    public override void Init()
    {
        base.Init();
        if(_hManager) _hManager.IsAlive = false;
        walkMovement = GetComponent<WalkMovement>();
    }

  

    protected override void DoAttack(GameObject target)
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
                Debug.Log("First Stage");
                break;
            case BossStage.Second:
                _hManager.IsAlive = true;
                Debug.Log("Second Stage");
                break;
            case BossStage.Final:
                _hManager.IsAlive = true;
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
        walkMovement.Init();
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
                    OnEnemyStateChange(EnemyState.Chase);
                }
                break;
            case EnemyState.Flee:
                break;
        }
    }


    private void Update()
    {
        if (!_bossFightRunning) return;
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
                    walkMovement.MoveToPoint(PathFollower.GetCurrentPathPoint());
                }

                break;
            case EnemyState.Attack:
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

}

