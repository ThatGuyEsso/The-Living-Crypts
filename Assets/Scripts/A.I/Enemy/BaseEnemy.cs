using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Flee,
    
};

[System.Serializable]
public struct EnemySettings
{

    [Header("Attack Settings")]
    public float AttackRange;
    public float AttackRate;
    public float MaxDamage;
    public float MinDamage;
    public float MaxKnockBack;
    public float MinKnockBack;

    public float GetRandomDamage()
    {
        return Random.Range(MinDamage, MaxDamage);
    }
    public float GetRandomKnockBack()
    {
        return Random.Range(MinKnockBack, MaxKnockBack);
    }
}
    [RequireComponent(typeof(PathFinder))]
public abstract class BaseEnemy : MonoBehaviour ,Iteam ,IInitialisable
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
    [SerializeField] protected PathFollower PathFollower;
    protected NavMeshPath _currentPath;
    [Header("UX")]
    [SerializeField] protected FaceDirection FaceDirection;
    protected CharacterHealthManager _hManager;
    //Variables that are set and updated at run time

    protected float _currentTimeBtwnAttacks;

    
    protected bool _canAttack;

    public System.Action OnInit;


    protected virtual void Awake()
    {
        if (InDebug) Init();
    }
    public virtual void Init()
    {
        float randValue = Random.Range(0f, MaxTickOffset);
   
        if (!PathFinder) PathFinder = GetComponent<PathFinder>();
        if (!PathFollower) PathFollower = GetComponent<PathFollower>();
        if (!FaceDirection) FaceDirection = GetComponent<FaceDirection>();
        _currentPath = new NavMeshPath();
        _hManager = GetComponent<CharacterHealthManager>();
        InvokeRepeating("ProcessAI", randValue, TickRate);

        OnInit?.Invoke();
    }




    protected abstract void ProcessAI();

    protected virtual void DrawPathToTarget()
    {
        if (!CurrentTarget || !PathFinder) return;
        _currentPath = PathFinder.GetPathToTarget(transform.position, CurrentTarget.position, NavMesh.AllAreas);
    }

    public virtual bool InRange()
    {

        if (!CurrentTarget) return false;
        return Vector2.Distance(CurrentTarget.position, transform.position) <= CharacterSettings.AttackRange;

    }

    public virtual void FaceCurrentTarget()
    {
        if (!FaceDirection || !CurrentTarget) return;
        FaceDirection.SmoothRotDirection(CurrentTarget.position- transform.position);
    }

    protected virtual void FaceCurrentPathPoint()
    {
        if ( !FaceDirection) return;
        if (_currentPath.corners.Length <= 0) return;

        //transform.LookAt(PathFinder.Path.corners[1]);
        FaceDirection.SmoothRotDirection((PathFollower.GetCurrentPathPoint() - transform.position).normalized);
    }

    public virtual void OnEnemyStateChange(EnemyState newState)
    {
        CurrentState = newState;
    }

    public void SetTarget(Transform target)
    {
        CurrentTarget = target;
    }
    protected abstract void DoAttack(GameObject target, Vector3 point);

    protected abstract void KillEnemy();

    public void WaitToInit(float time)
    {
        Invoke("Init", time);
    }


    public bool IsOnTeam(Team team)
    {
        return team == Team.Enviroment;
    }

    public Transform GetTaget()
    {
        return CurrentTarget;
    }

    public Team GetTeam()
    {
        return Team.Enviroment;
    }
}
