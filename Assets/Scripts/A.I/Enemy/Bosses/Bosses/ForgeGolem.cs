using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeGolem : BaseBoss
{
    public override void Init()
    {
        base.Init();
        BeginNewStage(BossStage.First);
    }
    protected override void DoAttack(GameObject target)
    {
    
    }

    protected override void KillEnemy()
    {
        
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
