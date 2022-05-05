using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhysicsRotator))]
[RequireComponent(typeof(CryptCharacterManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(FloatMovement))]
public class EnchantedWeapon : BaseEnemy, IAttacker
{
    [Header("Weapon Data")]
    [SerializeField] private List<EnchantedWeaponData> WeaponDatas;

    [Header("VFX")]
    [SerializeField] private GameObject ActiveSphere;
    [SerializeField] private Material AttackMaterial;
    [SerializeField] private Material DefaultMaterial;
    private TrailRenderer _trailRenderer;
    [Header("Settings")]
    [SerializeField] private float MinIdleTime, MaxIdleTime;
    [SerializeField] private float LaunchForce;
    [SerializeField] private LayerMask GroundLayers;
    [Header("SFX")]
    [SerializeField] private string OnSwingSFX;
    [SerializeField] private float SwingSFXPlayRate =0.5f;
    [Header("Manager References")]
    [SerializeField] private EnemyTicketManager TicketManager;

    private CryptCharacterManager _cryptCharacter;
    //Weapon Data
    private GameObject _weaponGO;
    private EnchantedWeaponData currentWeaponData;
    private AttackCollider _weaponCollider;
    //Component Refrences
    private PhysicsRotator _rotator;
    private FloatMovement _floatMovement;
    private Rigidbody _rb;
    private FaceTarget _faceTarget;
    private ComplexHitFlashManager _complexHitFlash;
    //States
    private bool _isIdle;
    private bool HasTicket;
    //Counter
    private float _currentIdleTime;
    private float _currentMoveTime;
    private AudioPlayer _spinAudioPlayer;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init()
    {
        base.Init();



        if (!_floatMovement)
        {
            _floatMovement = GetComponent<FloatMovement>();
        }
        if (!_rotator)
        {
            _rotator = GetComponent<PhysicsRotator>();
        }
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }

        if (!_faceTarget)
        {
            _faceTarget = GetComponent<FaceTarget>();
        }
        if (!_cryptCharacter)
        {
            _cryptCharacter = GetComponent<CryptCharacterManager>();
        }
        if (!_complexHitFlash)
        {
            _complexHitFlash = GetComponent<ComplexHitFlashManager>();
        }

        ResetEnemy();

        if (ActiveSphere)
        {
            ActiveSphere.SetActive(false);
        }




    }
    public override void SetTarget(Transform target)
    {
        base.SetTarget(target);
        if (_floatMovement)
        {
            _floatMovement.SetRelativeTargetHeight(CurrentTarget.position.y);
        }
    }
    protected override void EvaluateNewGameplayEvent(GameplayEvents newEvent)
    {
        base.EvaluateNewGameplayEvent(newEvent);

        switch (newEvent)
        {
            case GameplayEvents.PlayerDied:

                IsActive = false;
                if (_floatMovement)
                {
                    _floatMovement.StopAndDrop();
                }

                if (_rotator)
                {
                    _rotator.Stop();
                }
                break;

            case GameplayEvents.PlayerRespawnBegun:

                if (ObjectPoolManager.instance)
                {
                    if (gameObject)
                    {
                        ObjectPoolManager.Recycle(gameObject);
                    }


                }
                else
                {
                    if (gameObject)
                    {
                        Destroy(gameObject);
                    }

                }

                break;
        }
    }
    public void SetUpWeapon()
    {
        if (_weaponGO)
        {
            return;
        }
        if (!currentWeaponData)
        {
            Debug.LogError("No Weapon Data");
            return;
        }
        if (ObjectPoolManager.instance)
        {
            _weaponGO = ObjectPoolManager.Spawn(currentWeaponData.EnchantedWeaponPrefab, transform);
        }
        else
        {
            _weaponGO = Instantiate(currentWeaponData.EnchantedWeaponPrefab, transform);
        }
        float randYAxis = Random.Range(0f, 360f);
        _weaponGO.transform.localRotation = Quaternion.Euler(90f, randYAxis, 0f);
        CharacterSettings.MaxDamage = currentWeaponData.attackData.MaxDamage;
        CharacterSettings.MinDamage = currentWeaponData.attackData.MinDamage;
        CharacterSettings.MaxKnockBack = currentWeaponData.attackData.MaxKnockBack;
        CharacterSettings.MinKnockBack = currentWeaponData.attackData.MinKnockBack;


        if (_floatMovement)
        {
            _floatMovement.Init(currentWeaponData.GetFloatHeight(), currentWeaponData.MovementSpeed, true);
        }

        if (CurrentTarget)
        {
            if (_floatMovement)
            {
                _floatMovement.SetRelativeTargetHeight(CurrentTarget.position.y);
            }
        }
        _weaponCollider = _weaponGO.GetComponent<AttackCollider>();
        if (_weaponCollider)
        {
            _weaponCollider.SetOwner(gameObject);
        }
        if (_complexHitFlash)
        {
            _complexHitFlash.Init();
        }
    }

    protected override void DoAttack(GameObject target, Vector3 point)
    {
      
        if (_floatMovement)
        {
            _floatMovement.StopAndDrop();
        }

        if (_rotator)
        {
            _rotator.Stop();
        }
        PlaySFX(OnSwingSFX, true);
        if (_spinAudioPlayer)
        {
            _spinAudioPlayer.BeginFadeOut();
        }
            Vector3 dir = point - _weaponGO.transform.position;
        _rb.AddForce(dir.normalized * LaunchForce, ForceMode.Impulse);
        OnEnemyStateChange(EnemyState.Attack);
        if (TicketManager)
        {
            TicketManager.TicketUsed();
            HasTicket = false;
        }
        if (_trailRenderer)
        {
            _trailRenderer.material = AttackMaterial;
        }
        if (ActiveSphere)
        {
            ActiveSphere.SetActive(false);
        }

    }

    protected override void KillEnemy()
    {
        PlaySFX(KilledSFX, true);
        if (_cryptCharacter)
        {
            _cryptCharacter.RemoveSelf();
        }
        if (DeathVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(DeathVFX, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(DeathVFX, transform.position, Quaternion.identity);
            }
        }

        if (ObjectPoolManager.instance)
        {
            if (_weaponGO)
            {
                ObjectPoolManager.Recycle(_weaponGO);
            }
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            if (_weaponGO)
            {
                Destroy(_weaponGO);
            }
            Destroy(gameObject);
        }
    }

    protected override void ProcessAI()
    {

        if (!IsActive)
        {
         
            return;
        }
        if (TicketManager)
        {
            if (!HasTicket)
            {
                HasTicket = TicketManager.CanAttack();
                if (!HasTicket)
                {
                    //Debug.Log("No Tickets");
                    OnEnemyStateChange(EnemyState.Idle);
                    return;
                }
                //Debug.Log(" Ticket gained");
            }

        }
        if (_isIdle)
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

                if (!CurrentTarget)
                {
                    OnEnemyStateChange(EnemyState.Idle);
                }
                
                DrawPathToTarget();
                PathFollower.EvaluatePath(_currentPath, transform.position);
               
                

                break;
       
        }
    }

    private void Update()
    {

        if (!IsActive)
        {
            return;
        }
        //if weapon doesn't have ticket don't run
        if(TicketManager && !HasTicket)
        {
            return;
        }
        if (_isIdle)
        {
            if (_currentIdleTime <= 0)
            {
                OnEnemyStateChange(EnemyState.Chase);
                _isIdle = false;
            }
            else
            {
                _currentIdleTime -= Time.deltaTime;
            }

            return;
        }

        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget || !PathFinder)
                {
                    OnEnemyStateChange(EnemyState.Idle);
                    return;
                }
                if (_currentPath == null) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if (!PathFollower.EvaluatePath(_currentPath, transform.position)) return;

                if (!_floatMovement)
                {
                    OnEnemyStateChange(EnemyState.Idle);
                    return;
                }
                Vector3 direction = _currentPath.corners[1] - transform.position;
                _floatMovement.OnMove(direction.normalized);

                if(_currentMoveTime <= 0f)
                {
                    if (CurrentTarget)
                    {
                        DoAttack(CurrentTarget.gameObject, CurrentTarget.position);
                    }
                    else
                    {
                        _floatMovement.StopAndDrop();
                        OnEnemyStateChange(EnemyState.Idle);
                    }
         

                }
                else
                {
                    _currentMoveTime -= Time.deltaTime;
                }
                break;
            case EnemyState.Attack:

                if (_faceTarget)
                {
                    _faceTarget.SetTarget(CurrentTarget);
                    _faceTarget.FaceCurrentTarget();
                }
                break;


          
        }
    }



    public void SetNewIdleTime()
    {
        _currentIdleTime = Random.Range(MinIdleTime, MaxIdleTime);
        _isIdle = true;
    }
    public void SetNewMoveTIme()
    {
        if (!currentWeaponData)
        {
            return;
        }
        _currentMoveTime = Random.Range(currentWeaponData.MinMoveTime, currentWeaponData.MaxMoveTime);
    
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnDie -= KillEnemy;
        }

     
        if (_weaponGO)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(_weaponGO);
            }
            else
            {
                Destroy(_weaponGO);
            }
            _weaponGO = null;
        }
        if (gameObject)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
           
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnDie -= KillEnemy;
        }

        if (_weaponGO)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(_weaponGO);
            }
            else
            {
                Destroy(_weaponGO);
            }
            _weaponGO = null;
        }
        if (gameObject)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
           
        }
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

    public AttackData GetAttackData()
    {
        return currentWeaponData.attackData;
    }


    public override void OnEnemyStateChange(EnemyState newState)
    {
        base.OnEnemyStateChange(newState);
        switch (newState)
        {
            case EnemyState.Idle:
                
                SetNewIdleTime();
                if (_weaponCollider)
                {
                    _weaponCollider.IsEnabled = false;
                }
                if (_rotator )
                {

                    _rotator.Stop();
                }

                if (_floatMovement)
                {
                    _floatMovement.StopAndDrop();
                }
                break;
            case EnemyState.Chase:
                if (!CurrentTarget)
                {
                    OnEnemyStateChange(EnemyState.Idle);

                    return;
                }
                if (_trailRenderer)
                {
                    _trailRenderer.material = DefaultMaterial;
                }
                if (ActiveSphere)
                {
                    ActiveSphere.SetActive(true);
                }
                SetNewMoveTIme();
                if (_currentMoveTime > 0)
                {
                    if (_weaponCollider)
                    {
                        _weaponCollider.IsEnabled = true;
                    }
                    if (_rotator )
                    {

                        _rotator.Begin(0.5f);
                        Invoke("PlaySwingSFX", 0.8f);
                    }

                }
            
                break;

            case EnemyState.Attack:

                if (_rotator)
                {
                 
                    _rotator.Stop();
                }
                break;
        }
    }

    public void GoToIdle()
    {
        if (_floatMovement)
        {
            _floatMovement.StopAndDrop();
        }

        if (_rotator)
        {
            _rotator.Stop();
        }
        PlaySFX(OnSwingSFX, true);
        if (_spinAudioPlayer)
        {
            _spinAudioPlayer.BeginFadeOut();
        }
    
        OnEnemyStateChange(EnemyState.Idle);
        if (TicketManager)
        {
            TicketManager.TicketUsed();
            HasTicket = false;
        }
        if (_trailRenderer)
        {
            _trailRenderer.material = DefaultMaterial;
        }
        if (ActiveSphere)
        {
            ActiveSphere.SetActive(false);
        }
    }
    public void OnHurt()
    {
        PlaySFX(HurtSFX, true);
        if(CurrentState!= EnemyState.Idle)
        {
            GoToIdle();
        }
        if (HurtVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(HurtVFX, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(HurtVFX, transform.position, Quaternion.identity);
            }
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if(CurrentState == EnemyState.Attack)
        {
            OnEnemyStateChange(EnemyState.Idle);

        }
    }
    public override void SetTicketManager(EnemyTicketManager ticketManager)
    {
        TicketManager = ticketManager;
    }


    public void PlaySwingSFX()
    {
      
        _spinAudioPlayer= PlaySFX(OnSwingSFX, true);
        if (_rotator.IsRotating)
        {
            Invoke("PlaySwingSFX", SwingSFXPlayRate);
        }
   
    
    }

    public override void ResetEnemy()
    {
        if (WeaponDatas.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        SetNewIdleTime();



        int randIndex = Random.Range(0, WeaponDatas.Count);
        currentWeaponData = WeaponDatas[randIndex];

        if (!_hManager)
        {
            Destroy(this);
            return;
        }
        _hManager.Init();
        _hManager.OnHurt += OnHurt;
        _hManager.OnDie += KillEnemy;

        SetUpWeapon();
        SetNewMoveTIme();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
}
