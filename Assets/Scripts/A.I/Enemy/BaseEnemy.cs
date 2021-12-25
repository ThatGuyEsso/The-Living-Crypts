using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Flee
};

[System.Serializable]
public struct EnemySettings
{
    [Header("Power Scale Settings")]
    public float MaxHealth;
    public float MaxMovementSpeed;

    [Header("Attack Settings")]
    public float AttackRange;
    public float AttackRate;
    public float MaxDamage;
    public float MinDamage;

}
[RequireComponent(typeof(PathFinder))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] protected bool InDebug;

    [Header("AI Settings")]
    [SerializeField] protected float TickRate;
    [SerializeField] protected float MaxTickOffset;
    [SerializeField] protected Transform CurrentTarget;
    [Tooltip("Order Matters")]


    [Header("Enemy Character Settings")]
    [SerializeField] protected EnemySettings CharacterSettings;
  
    [Header("Enemy States")]
    [SerializeField] protected EnemyState CurrentState;

    [Header("Pathfinding")]
    [SerializeField] protected PathFinder PathFinder;

    [Header("UX")]
    [SerializeField] protected FaceDirection FaceDirection;

    //Variables that are set and updated at run time
    protected float _currentHealth;
    protected float _currentSpeed;
    protected virtual void Awake()
    {
        if (InDebug) Init();
    }
    public virtual void Init()
    {
        float randValue = Random.Range(0f, MaxTickOffset);
   
        if (!PathFinder) PathFinder = GetComponent<PathFinder>();

        InvokeRepeating("ProcessAI", randValue, TickRate);
    }




    protected abstract void ProcessAI();

    protected virtual void DrawPathToTarget()
    {
        if (!CurrentTarget || !PathFinder) return;
        PathFinder.GetPathToTarget(transform.position, CurrentTarget.position, NavMesh.AllAreas);
    }

    public virtual bool InRange()
    {

        if (!CurrentTarget) return false;
        return Vector2.Distance(CurrentTarget.position, transform.position) <= CharacterSettings.AttackRange;

    }

    protected virtual void FaceCurrentTarget()
    {
        if (!FaceDirection || !CurrentTarget) return;
        FaceDirection.SmoothRotDirection(CurrentTarget.position);
    }

    protected virtual void FaceCurrentPathPoint()
    {
        if (!PathFinder || !FaceDirection) return;
        if (PathFinder.Path.corners.Length <= 0) return;
        //transform.LookAt(PathFinder.Path.corners[1]);
        FaceDirection.SmoothRotDirection((PathFinder.Path.corners[1] - transform.position).normalized);
    }
}
