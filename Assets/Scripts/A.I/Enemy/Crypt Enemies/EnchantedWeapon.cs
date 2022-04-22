using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(CryptCharacterManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Rotator))]
[RequireComponent(typeof(FloatMovement))]
public class EnchantedWeapon : BaseEnemy
{
    [Header("Weapon Data")]
    [SerializeField] private List<EnchantedWeaponData> WeaponDatas;

    [Header("Settings")]
    [SerializeField] private float MinIdleTime, MaxIdleTime;
    [SerializeField] private float LaunchForce;
    [SerializeField] private LayerMask GroundLayers;

    [Header("Manager References")]
    [SerializeField] private EnemyTicketManager TicketManager;

    private CryptCharacterManager _cryptCharacter;
    //Weapon Data
    private GameObject _weaponGO;
    private EnchantedWeaponData currentWeaponData;

    //Component Refrences
    private Rotator _rotator;
    private FloatMovement _floatMovement;
    private Rigidbody _rb;
    //States
    private bool _isIdle;

    //Counter
    private float _currentIdleTime;
    private float _currentMoveTime;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init()
    {
        base.Init();
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
        _hManager.OnDie += KillEnemy;

        if (!_floatMovement)
        {
            _floatMovement = GetComponent<FloatMovement>();
        }
        if (!_rotator)
        {
            _rotator = GetComponent<Rotator>();
        }
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
        SetUpWeapon();
        SetNewMoveTIme();

    }

    public void SetUpWeapon()
    {
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
    }

    protected override void DoAttack(GameObject target, Vector3 point)
    {
        SetNewIdleTime();
        if (_floatMovement)
        {
            _floatMovement.StopAndDrop();
        }
        if (_rotator&& _rotator.enabled)
        {
            _rotator.enabled = false;
        }
        Vector3 dir = point - transform.position;
        _rb.AddForce(dir.normalized * LaunchForce, ForceMode.Impulse);

      
    }

    protected override void KillEnemy()
    {
        _cryptCharacter.RemoveSelf();
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void ProcessAI()
    {

        if (!IsActive)
        {
            if (_rotator && !_rotator.enabled)
            {
                _rotator.enabled = false;
            }
            return;
        }

        if (_isIdle)
        {
            if (_rotator && !_rotator.enabled)
            {
                _rotator.enabled = false;
            }
            return;
        }
        if (TicketManager && !TicketManager.CanAttack())
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
                if (_currentMoveTime>0)
                {
                    DrawPathToTarget();
                    PathFollower.EvaluatePath(_currentPath, transform.position);
                    if (_rotator &&!_rotator.enabled)
                    {
                        
                        _rotator.enabled = true;
                    }
                    
                }
               

                break;
       
        }
    }

    private void Update()
    {

        if (!IsActive)
        {
            return;
        }

        if (_isIdle)
        {
            if (_currentIdleTime <= 0)
            {
                SetNewMoveTIme();
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
                if (!CurrentTarget || !PathFinder) return;
                if (_currentPath == null) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if (!PathFollower.EvaluatePath(_currentPath, transform.position)) return;

                if (!_floatMovement)
                {
                    return;
                }
                Vector3 direction = _currentPath.corners[1] - transform.position;
                _floatMovement.OnMove(direction.normalized);

                if(_currentMoveTime <= 0f)
                {
                 
                    DoAttack(CurrentTarget.gameObject, CurrentTarget.position);

                }
                else
                {
                    _currentMoveTime -= Time.deltaTime;
                }
                break;
            case EnemyState.Attack:
          


                break;
            case EnemyState.Flee:
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
            _hManager.OnDie -= KillEnemy;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_hManager)
        {
       
            _hManager.OnDie -= KillEnemy;
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
}
