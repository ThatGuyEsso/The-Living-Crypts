using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ForgeGolem : BaseBoss
{

    [Header("Layers")]
    [SerializeField] private LayerMask GroundLayers;
    [Header("Collision")]
    [SerializeField] private CapsuleCollider MainCollider;
    private WalkMovement _walkMovement;
    [Header("Animation")]
    [SerializeField] protected BossAnimationManager AnimManager;

    [Header("SFX")]
    [SerializeField] protected string AwakenSFX;
    [Header("VFX")]
    protected float MinTimeBetweenBreaks=0.25f,MaxTimeBetweenBreaks=1f;
    [SerializeField] protected GameObject ChestFlameVFX;
    protected MaterialSwitch[] MaterialSwitches;
    protected ComplexHitFlashManager HitVFXs;
    [SerializeField] protected CreateFreeRigidBBody[] BreakJoints;

    protected override void Awake()
    {
        base.Awake();
        if (ChestFlameVFX)
        {
            ChestFlameVFX.SetActive(false);
        }
    }
    public override void Init()
    {
        base.Init();
        if (_hManager)
        {
            _hManager.IsAlive = false;

            _hManager.OnDamageReceived += DamageReceived;
        }
        if (!_walkMovement)
        {
            _walkMovement = GetComponent<WalkMovement>();
        }

        if (!AnimManager)
        {
            AnimManager = GetComponent<BossAnimationManager>();
        }
        MainCollider = GetComponent<CapsuleCollider>();

      
    }

    protected override void EvaluateNewGameplayEvent(GameplayEvents newEvent)
    {
        base.EvaluateNewGameplayEvent(newEvent);

        switch (newEvent)
        {
            case GameplayEvents.PlayerDied:

                OnEnemyStateChange(EnemyState.Idle);
                _bossFightRunning = false;
                
                break;

        }
    }


    protected override void DoAttack(GameObject target, Vector3 point)
    {
    
    }

    public override void EndBossFight()
    {
        base.EndBossFight();
        if (_gameManager)
        {
            _gameManager.BeginNewGameplayEvent(GameplayEvents.OnBossKilled);
        }
        KillEnemy();
    }
    protected override void KillEnemy()
    {
        _bossFightRunning = false;
        _walkMovement.BeginStop();
        
        ClearEquippedAbilities();
        if (AnimManager)
        {
            AnimManager.PlayIdleAnimation();
            AnimManager.StopAnimating();
        }
        if (MaterialSwitches.Length > 0)
        {


            foreach (MaterialSwitch MatSwitch in MaterialSwitches)
            {
                MatSwitch.SwitchOff();
            }
        }


        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            collider.isTrigger = false;
        }

        if (MainCollider)
        {
            MainCollider.enabled = false;
        }
        if (ChestFlameVFX)
        {
            ChestFlameVFX.SetActive(false);
        }
        if (BreakJoints.Length > 0)
        {
            StartCoroutine(BreakJointsOverTime(BreakJoints));

        }

       


     
       

    }
    private IEnumerator BreakJointsOverTime( CreateFreeRigidBBody[] jointsToBreak)
    {

        for (int i= 0;i < jointsToBreak.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(MinTimeBetweenBreaks,MaxTimeBetweenBreaks));
            jointsToBreak[i].Init();
            if (DeathVFX)
            {
                if (ObjectPoolManager.instance)
                {
                    ObjectPoolManager.Spawn(DeathVFX, jointsToBreak[i].transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(DeathVFX, jointsToBreak[i].transform.position, Quaternion.identity);
                }
            }
        }
        if (_gameManager)
        {
            _gameManager.BeginNewGameplayEvent(GameplayEvents.OnBossFightEnd);
        }

   
    }

    protected override void BeginNewStage(BossStage newStage)
    {
        if (!_bossFightRunning) return;
        base.BeginNewStage(newStage);
        switch (newStage)
        {
            case BossStage.First:
             
                _hManager.IsAlive = true;
                OnEnemyStateChange(EnemyState.Chase);
                SetUpNewAbilities();
                Debug.Log("First Stage");
                break;
            case BossStage.Second:
            
                _hManager.IsAlive = true;
                OnEnemyStateChange(EnemyState.Chase);
                SetUpNewAbilities();
                Debug.Log("Second Stage");
                break;
            case BossStage.Final:
             
                _hManager.IsAlive = true;
                OnEnemyStateChange(EnemyState.Chase);
                SetUpNewAbilities();
                Debug.Log("Final Stage");
                break;
            case BossStage.Transition:
                
                AnimManager.PlayIdleAnimation();
                _walkMovement.BeginStop();
                _hManager.IsAlive = false;
                InitTransitionAbility();
                break;
        }

    }
    public override void AwakenBoss()
    {
        MaterialSwitches = GetComponentsInChildren<MaterialSwitch>();

        if (MaterialSwitches.Length > 0)
        {
            foreach (MaterialSwitch MatSwitch in MaterialSwitches)
            {
                MatSwitch.SwitchOn();
            }
        }

        HitVFXs = GetComponent<ComplexHitFlashManager>();
        if (HitVFXs)
        {
            HitVFXs.Init();
        }
        PlaySFX(AwakenSFX, false);
        if (ChestFlameVFX)
        {
            ChestFlameVFX.SetActive(true);
        }
    }
    public override void StartBossFight()
    {
        base.StartBossFight();
        _walkMovement.Init();
        AnimManager.Init();
        OnEnemyStateChange(EnemyState.Idle);
    }
    protected override void ProcessAI()
    {
        if (!_bossFightRunning) return;
        if(_currentStage == BossStage.Transition)
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

                if (!CurrentTarget) OnEnemyStateChange(EnemyState.Idle);
                if (_isUsingAttack)
                {
                    return;
                }
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
                if (_isUsingAttack)
                {
                    return;
                }
                if (!InAbilityRange())
                {
                    OnEnemyStateChange(EnemyState.Chase);
                }
                else 
                {
                    _walkMovement.BeginStop();
                    if (_canUseAttack)
                    {
                      
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
                if (!CurrentTarget || !PathFinder)
                {
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
                if (InAbilityRange())
                {
                    OnEnemyStateChange(EnemyState.Attack);
                    if (_canUseAttack)
                    {
                      
                        ExecuteAbility();
                    }
                    return;
                }
                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    _walkMovement.MoveToPoint(PathFollower.GetCurrentPathPoint());
                }

                break;
            case EnemyState.Attack:

             
                if (_isUsingAttack)
                {
                    return;
                }
                FaceCurrentTarget();
                if (InAbilityRange())
                {
                    if (_canUseAttack)
                    {

                        ExecuteAbility();
                    }
                }
                break;
            case EnemyState.Flee:
                break;
        }



        
    }

    public override void OnEnemyStateChange(EnemyState newState)
    {
        base.OnEnemyStateChange(newState);

        switch (newState)
        {
            case EnemyState.Chase:
                if (!AnimManager)
                {
                    return;
                }
                AnimManager.PlayWalkAnimation();
                break;
              
            case EnemyState.Idle:
                if (!AnimManager)
                {
                    return;
                }
                AnimManager.PlayIdleAnimation();

                break;

            case EnemyState.Attack:
                if (!AnimManager)
                {
                    return;
                }
                AnimManager.PlayIdleAnimation();
                _walkMovement.BeginStop();
                break;
        }
    }
    public override void ResetEnemy()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnHurt()
    {
        base.OnHurt();
        PlaySFX(HurtSFX,true);
    }
    protected override void DrawPathToTarget()
    {
        if (!CurrentTarget || !PathFinder) return;


        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
        {

            _currentPath = PathFinder.GetPathToTarget(hitInfo.point, CurrentTarget.position, NavMesh.AllAreas);
        }

    }

    protected void DamageReceived(float maxHealh , float dmg , float kback , Vector3 kDir, Vector3 point)
    {
        if (HurtVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(HurtVFX, point, Quaternion.identity);
            }
            else
            {
                Instantiate(HurtVFX, point, Quaternion.identity);
            }
        }
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
        if (_hManager)
        {
            _hManager.OnDamageReceived -= DamageReceived;
         
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_hManager)
        {
            _hManager.OnDamageReceived -= DamageReceived;
        }
    }
}

