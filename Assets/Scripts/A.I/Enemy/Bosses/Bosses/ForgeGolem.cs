using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeGolem : BaseBoss
{
    public override void Init()
    {
        base.Init();
        if(_hManager) _hManager.IsAlive = false;

    }

  

    protected override void DoAttack(GameObject target)
    {
    
    }

    protected override void KillEnemy()
    {
        
    }


    protected override void BeginNewStage(BossStage newStage)
    {
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
                    OnEnemyStateChange(EnemyState.Chase);
                }
                break;
            case EnemyState.Flee:
                break;
        }
    }
}
