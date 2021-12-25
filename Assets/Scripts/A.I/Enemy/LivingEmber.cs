using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpMovement))]
public class LivingEmber : BaseEnemy
{
    private JumpMovement jumpMovement;
    public override void Init()
    {
        base.Init();
        if (!jumpMovement) jumpMovement = GetComponent<JumpMovement>();
    }
    protected override void ProcessAI()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                DrawPathToTarget();
                break;
            case EnemyState.Attack:
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
                if (!CurrentTarget||!PathFinder||PathFinder.Path.corners.Length <= 0) return;
         
                    FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFinder.Path.corners[1] - transform.position).normalized,0.1f))
                    jumpMovement.DoJump((PathFinder.Path.corners[1] - transform.position).normalized);
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Flee:
                break;
        }
    }


}