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
    protected MaterialSwitch[] MaterialSwitches;
    protected ComplexHitFlashManager HitVFXs;
    [SerializeField] protected CreateFreeRigidBBody[] BreakJoints;
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
        MainCollider = GetComponent<CapsuleCollider>();


    }

  

    protected override void DoAttack(GameObject target, Vector3 point)
    {
    
    }

    public override void EndBossFight()
    {
        base.EndBossFight();
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
            jointsToBreak[i].Init(); ;
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
                AnimManager.PlayWalkAnimation();
                break;

            case EnemyState.Idle:
                AnimManager.PlayIdleAnimation();

                break;

            case EnemyState.Attack:
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
}

