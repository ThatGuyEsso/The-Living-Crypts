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
public abstract class BaseEnemy : MonoBehaviour ,Iteam ,IInitialisable, IEnemy
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

    [Header("SFX")]
    [SerializeField] protected string HurtSFX;
    [SerializeField] protected string KilledSFX;
    [SerializeField] protected AudioManager AM;

    [Header("VFX")]
    [SerializeField] protected GameObject DeathVFX;

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

    protected bool IsActive;
    
    protected bool _canAttack;
    protected GameManager _gameManager;
    public System.Action OnInit;
    protected bool _isInitialised;


    protected virtual void Awake()
    {
        if (InDebug) Init();
    }
    public virtual void Init()
    {
        float randValue = Random.Range(0f, MaxTickOffset);

        if (!PathFinder)
        {
            PathFinder = GetComponent<PathFinder>();
         
        }
        if (!PathFollower)
        {
            PathFollower = GetComponent<PathFollower>();
          
        }
        if (!FaceDirection)
        {
            FaceDirection = GetComponent<FaceDirection>();
        }

        _currentPath = new NavMeshPath();

        _hManager = GetComponent<CharacterHealthManager>();


        Invoke("StartAITick", randValue);
  
        IsActive = true;
        _isInitialised = true;
        OnInit?.Invoke();

    }

    public virtual void OnEnable()
    {
        IsActive = true;

        if (!_gameManager)
        {
            if (GameStateManager.instance && GameStateManager.instance.GameManager)
            {
                _gameManager = GameStateManager.instance.GameManager;
            }
        }


        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent += EvaluateNewGameplayEvent;
        }
      


    }
    protected abstract void ProcessAI();

    virtual protected void EvaluateNewGameplayEvent( GameplayEvents newEvent)
    {
        //
    }

    protected virtual void DrawPathToTarget()
    {
        if (!CurrentTarget || !PathFinder) return;
        _currentPath = PathFinder.GetPathToTarget(transform.position, CurrentTarget.position, NavMesh.AllAreas);
    }

    public virtual bool InRange()
    {

        if (!CurrentTarget) return false;
        float dist = Vector3.Distance(CurrentTarget.position, transform.position);
        return dist <= CharacterSettings.AttackRange;

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

    public virtual void SetTarget(Transform target)
    {
        CurrentTarget = target;
    }
    protected abstract void DoAttack(GameObject target, Vector3 point);

    protected abstract void KillEnemy();
    protected virtual void DestroyEnemy()
    {

        SpawnDeathVFX();
    }
    protected virtual void SpawnDeathVFX()
    {

    }
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
    virtual protected void OnDisable()
    {
        if(_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvaluateNewGameplayEvent;
        }
    }
    virtual protected void OnDestroy()
    {
        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvaluateNewGameplayEvent;
        }
    }

    public virtual void SetTicketManager(EnemyTicketManager ticketManager)
    {
       //
    }

    public virtual AudioPlayer PlaySFX(string sfxName,bool randPitch)
    {
        if (AM)
        {
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
        else
        {
            if(!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }

            AM = GameStateManager.instance.AudioManager;
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
    }

    public abstract void ResetEnemy();

    public virtual void StartAITick()
    {
        StartCoroutine(DoAITick());
    }
    public virtual IEnumerator DoAITick()
    {
        ProcessAI();

        yield return new WaitForSeconds(TickRate);
        StartCoroutine(DoAITick());
    }

    public AudioManager AudioManager { set { AM = value; } }
}
