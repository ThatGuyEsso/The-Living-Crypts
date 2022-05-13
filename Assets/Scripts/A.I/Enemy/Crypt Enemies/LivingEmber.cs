using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpMovement))]
[RequireComponent(typeof(CryptCharacterManager))]
[RequireComponent(typeof(SquashAndStretch))]
public class LivingEmber : BaseEnemy
{
    [Header("Enemy Character Settings")]
    [SerializeField] private JumpData AttackJumpSettings;
    [Tooltip("If scale <MinimumSplitsSize don't split")]
    [SerializeField] private float MinimumSplitSize;
    [SerializeField] private int MaxSplitCount;
    [SerializeField] private GameObject LivingEmberPrefab;
    [SerializeField] private EnemySettings _defaultSettings;
    [SerializeField] private JumpData DefaultJumpData;
    private SquashAndStretch _squashAndStretch;
    private JumpMovement _jumpMovement;
    private LootDropper _lootDropper;
    private RandomSizeInRange _randomSize;

    private CryptCharacterManager _cryptCharacter;

    private bool IsChargingJump = false;

    protected override void Awake()
    {
        base.Awake();
       
   
      
    }
    public override void Init()
    {
        base.Init();
        if (!_randomSize)
        {
            _randomSize = GetComponent<RandomSizeInRange>();
        }
   
        if (!_jumpMovement)
        {
            _jumpMovement = GetComponent<JumpMovement>();
            DefaultJumpData = _jumpMovement.JumpData;
        }
        if(!_squashAndStretch)
        {
            _squashAndStretch = GetComponent<SquashAndStretch>();
          
        }     if (!_cryptCharacter)
        {
            _cryptCharacter = GetComponent<CryptCharacterManager>();
        }

        if (!_lootDropper)
        {
            _lootDropper = GetComponent<LootDropper>();
        }
        ResetEnemy();
    }

    protected override void DoAttack(GameObject target, Vector3 point)
    {
        IDamage damagable = target.GetComponent<IDamage>();
        if(damagable != null)
        {
            damagable.OnDamage(CharacterSettings.GetRandomDamage(),transform.forward, CharacterSettings.GetRandomKnockBack(), gameObject,point);
            _canAttack = false;
            _currentTimeBtwnAttacks = CharacterSettings.AttackRate;
        }
    }

    protected override void EvaluateNewGameplayEvent(GameplayEvents newEvent)
    {
        base.EvaluateNewGameplayEvent(newEvent);

        switch (newEvent)
        {
            case GameplayEvents.PlayerDied:

                IsActive = false;
                break;
            case GameplayEvents.OnBossKilled:

                DestroyEnemy();
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

    protected override void ProcessAI()
    {
        if (!IsActive)
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
                    break;
                }
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

    private void Update()
    {

        if (!IsActive)
        {
            return;
        }
        if(transform.position.y <- 16f)
        {
            KillEnemy();
        }
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget||!PathFinder) return;
                if (_currentPath == null) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if(!PathFollower.EvaluatePath(_currentPath,transform.position)) return;
                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    if (!IsChargingJump&&_squashAndStretch&&!_squashAndStretch.Animating)
                    {
                        IsChargingJump = true;
                        _squashAndStretch.DoSquash();
                        _squashAndStretch.OnAnimComplete += DoJump;
                    }
                 
                }
                
                break;
            case EnemyState.Attack:
                FaceCurrentTarget();


                if (_canAttack)
                {
                    if (!IsChargingJump && _squashAndStretch)
                    {
                        IsChargingJump = true;
                        _squashAndStretch.DoSquash();
                        _squashAndStretch.OnAnimComplete += DoJumpAttackJump;
                    }
                }
                   
           
                break;
            case EnemyState.Flee:
                break;
        }

        if (!_canAttack)
        {
            if (_currentTimeBtwnAttacks <= 0f)
            {
                _canAttack = true;
            }
            else
            {
                _currentTimeBtwnAttacks -= Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsActive)
        {
            return;
        }


        if (AttackJumpSettings.GroundedLayers == (AttackJumpSettings.GroundedLayers | (1 << other.gameObject.layer)))
        {
            if (_squashAndStretch)
            {
                _squashAndStretch.ReturnToNormal();
            }

        }

        Iteam otherTeam = other.gameObject.GetComponent<Iteam>();
        if (otherTeam != null){

            if (IsOnTeam(otherTeam.GetTeam())) return;
            if (_canAttack && !_hManager.IsHurt)
            {
                DoAttack(other.gameObject,other.collider.ClosestPoint(transform.position));
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (AttackJumpSettings.GroundedLayers == (AttackJumpSettings.GroundedLayers | (1 << other.gameObject.layer)))
        {
            if (_squashAndStretch)
            {
                _squashAndStretch.DoStretch();
            }

        }
    }

    public void OnHurt()
    {
        OnEnemyStateChange(EnemyState.Idle);
        PlaySFX(HurtSFX,true);
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
    public void OnNotHurt()
    {
        OnEnemyStateChange(EnemyState.Chase);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
        if (_squashAndStretch)
        {
            _squashAndStretch.OnAnimComplete -= DoJump;
            _squashAndStretch.OnAnimComplete -= DoJumpAttackJump;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
        if (_squashAndStretch)
        {
            _squashAndStretch.OnAnimComplete -= DoJump;
            _squashAndStretch.OnAnimComplete -= DoJumpAttackJump;
        }
    }
    protected override void KillEnemy()
    {
        base.KillEnemy();
        CharacterSettings = _defaultSettings;
        PlaySFX(KilledSFX, true);
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
        if (transform.localScale.x > MinimumSplitSize)
        {

            Split();
        }
        else
        {
            _lootDropper.SpawnLoot();
            if (_cryptCharacter)
            {
                _cryptCharacter.RemoveSelf();
            }
        
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
    protected override void DestroyEnemy()
    {
        base.DestroyEnemy();
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
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
     
    }

    public void Split()
    {
        int splitCount = Random.Range(2, MaxSplitCount + 1);

        float splitSize = transform.localScale.x/ splitCount;
        if(splitSize < MinimumSplitSize)
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
        else
        {
            HealthData healthData = GetComponent<CharacterHealthManager>().HealthData;
            healthData._maxDefaultHealth = healthData._maxDefaultHealth / splitCount;

            for (int i = 0; i < splitCount; i++)
            {
                LivingEmber newEmber;
                float randX = Random.Range(-1f, 1f);
                float randZ = Random.Range(-1f, 1f);
                Vector3 randXZ = new Vector3(randX, 0f, randZ);
                if (ObjectPoolManager.instance)
                {
                     newEmber = ObjectPoolManager.Spawn(LivingEmberPrefab, transform.position+ randXZ, Quaternion.identity).GetComponent<LivingEmber>();
                }
                else
                {
                     newEmber = Instantiate(LivingEmberPrefab, transform.position, Quaternion.identity).GetComponent<LivingEmber>();
                }

                Rigidbody emberRB = newEmber.GetComponent<Rigidbody>();
                if (newEmber)
                {
                    RandomSizeInRange size = newEmber.gameObject.GetComponent<RandomSizeInRange>();
                    if (size) size.SetRandomSize(splitSize, splitSize, false);

                    CharacterHealthManager healthManager = newEmber.gameObject.GetComponent<CharacterHealthManager>();
                    healthManager.HealthData = healthData;
                    newEmber.SetTarget(CurrentTarget);
                    newEmber.OnEnemyStateChange(EnemyState.Chase);
                    newEmber.Init();

                    CryptCharacterManager cryptEmber = newEmber.GetComponent<CryptCharacterManager>();

                    if (cryptEmber)
                    {
                        _cryptCharacter.AddNewCharacter(cryptEmber);
                        if (emberRB)
                        {
                            emberRB.AddForce(Vector3.up + randXZ * _jumpMovement.JumpData.JumpForce, ForceMode.Impulse);
                        }
                    }

              
                }

            }
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
     
    }

    private void DoJump()
    {
        if (_squashAndStretch)
        {
            _squashAndStretch.OnAnimComplete -= DoJump;
       
   
        }
        IsChargingJump = false;
        _jumpMovement.DoJump((transform.forward).normalized);

    }

    private void DoJumpAttackJump()
    {
        if (_squashAndStretch)
        {
            _squashAndStretch.OnAnimComplete -= DoJumpAttackJump;
           

        }
        IsChargingJump = false;
        _jumpMovement.DoJump((transform.forward).normalized, AttackJumpSettings);

    }

    public void BoostStatsWithSize()
    {
        //Boost health
        if (_hManager)
        {
            
            _hManager.CurrentHealth = _hManager.HealthData._maxDefaultHealth * transform.localScale.x; ;
        }


        //Boost Attack range
        CharacterSettings.AttackRange= transform.localScale.x / 2f;
        CharacterSettings.MaxDamage = _defaultSettings.MaxDamage * transform.localScale.x/2;
        CharacterSettings.MinDamage = _defaultSettings.MinDamage * transform.localScale.x / 2;

        CharacterSettings.MaxKnockBack = _defaultSettings.MaxKnockBack * transform.localScale.x / 4;
        CharacterSettings.MinKnockBack = _defaultSettings.MinKnockBack * transform.localScale.x / 4;
        if (_jumpMovement)
        {
            //Movement
            if (transform.localScale.x >= 1)
            {
                JumpData data = _jumpMovement.JumpData;
                data.JumpHeightMultiplier = data.JumpHeightMultiplier + transform.localScale.x / 2f;
                data.JumpForce = data.JumpForce + transform.localScale.x / 2f;
                _jumpMovement.JumpData = data;
            }
      
        }
        
   

    }

    public override void ResetEnemy()
    {
        if (_jumpMovement)
        {
            _jumpMovement.JumpData = DefaultJumpData;
            _jumpMovement.Init();
        }
        BoostStatsWithSize();
        if (_randomSize)
        {
            _randomSize.SetRandomSize();

        }
        if (!_hManager) Destroy(this);
        else
        {

            _hManager.Init();
            _hManager.OnHurt += OnHurt;
            _hManager.OnNotHurt += OnNotHurt;
            _hManager.OnDie += KillEnemy;
        }
        IsChargingJump = false;
     

        if (_squashAndStretch)
        {
            _squashAndStretch.Init();
        }
    }
}