using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpMovement))]
public class LivingEmber : BaseEnemy
{
    [Header("Enemy Character Settings")]
    [SerializeField] private JumpData AttackJumpSettings;
    [Tooltip("If scale <MinimumSplitsSize don't split")]
    [SerializeField] private float MinimumSplitSize;
    [SerializeField] private int MaxSplitCount;
    [SerializeField] private GameObject LivingEmberPrefab;
    private JumpMovement _jumpMovement;
    private CharacterHealthManager _hManager;
    private RandomSizeInRange _randomSize;
    public override void Init()
    {
        base.Init();
        _randomSize = GetComponent<RandomSizeInRange>();
        if (_randomSize) _randomSize.SetRandomSize();
        if (!_jumpMovement) _jumpMovement = GetComponent<JumpMovement>();
        _hManager = GetComponent<CharacterHealthManager>();
        BoostStatsWithSize();
        if (!_hManager) Destroy(this);
        else
        {

            _hManager.Init();
            _hManager.OnHurt += OnHurt;
            _hManager.OnNotHurt += OnNotHurt;
            _hManager.OnDie += KillEnemy;
        }

    }

    protected override void DoAttack(GameObject target)
    {
        IDamage damagable = target.GetComponent<IDamage>();
        if(damagable != null)
        {
            damagable.OnDamage(CharacterSettings.GetRandomDamage(),transform.forward, CharacterSettings.GetRandomKnockBack(), gameObject);
            _canAttack = false;
            _currentTimeBtwnAttacks = CharacterSettings.AttackRate;
        }
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

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Chase:
                //Abort cases
                if (!CurrentTarget||!PathFinder) return;
                if (_currentPath.corners.Length <= 0) return;
                //evaluate path
                if(!PathFollower.EvaluatePath(_currentPath,transform.position)) return;
                FaceCurrentPathPoint();
                if (EssoUtility.InSameDirection(transform.forward, (PathFollower.GetCurrentPathPoint() - transform.position).normalized, 0.1f))
                {
                    _jumpMovement.DoJump((transform.forward).normalized);
                }
                
                break;
            case EnemyState.Attack:
                FaceCurrentTarget();
          
          
                if(_canAttack)
                    _jumpMovement.DoJump((transform.forward).normalized,AttackJumpSettings);
           
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
        if (other.gameObject.CompareTag("Player")){
            if (_canAttack && !_hManager.IsHurt)
            {
                DoAttack(other.gameObject);
            }
        }
    }

    public void OnHurt()
    {
        OnEnemyStateChange(EnemyState.Idle);
    }
    public void OnNotHurt()
    {
        OnEnemyStateChange(EnemyState.Chase);
    }

    private void OnDestroy()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
    }

    private void OnDisable()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= OnHurt;
            _hManager.OnNotHurt -= OnNotHurt;
            _hManager.OnDie -= KillEnemy;
        }
    }

    protected override void KillEnemy()
    {
        if (transform.localScale.x > MinimumSplitSize)
            Split();
        else
            Destroy(gameObject);
    }


    public void Split()
    {
        int splitCount = Random.Range(2, MaxSplitCount + 1);

        float splitSize = transform.localScale.x/ splitCount;
        if(splitSize < MinimumSplitSize)
        {
            Destroy(gameObject);
        }
        else
        {
            HealthData healthData = GetComponent<CharacterHealthManager>().HealthData;
            healthData._maxDefaultHealth = healthData._maxDefaultHealth / splitCount;

            for (int i = 0; i < splitCount; i++)
            {
                LivingEmber newEmber = Instantiate(LivingEmberPrefab, transform.position, Quaternion.identity).GetComponent<LivingEmber>();

                if (newEmber)
                {
                    RandomSizeInRange size = newEmber.gameObject.GetComponent<RandomSizeInRange>();
                    if (size) size.SetRandomSize(splitSize, splitSize, false);

                    CharacterHealthManager healthManager = newEmber.gameObject.GetComponent<CharacterHealthManager>();
                    healthManager.HealthData = healthData;
                    newEmber.SetTarget(CurrentTarget);
                    newEmber.OnEnemyStateChange(EnemyState.Chase);
                    newEmber.Init();

                }
            }
        }
     
        Destroy(gameObject);
    }


    public void BoostStatsWithSize()
    {
        //Boost health
        if (_hManager)
        {
            HealthData hData = _hManager.HealthData;
            hData._maxDefaultHealth = _hManager.HealthData._maxDefaultHealth * transform.localScale.x;
            _hManager.HealthData = hData;
        }


        //Boost Attack range
        CharacterSettings.AttackRange+= transform.localScale.x / 2f;
        CharacterSettings.MaxDamage += CharacterSettings.MaxDamage * transform.localScale.x/2;
        CharacterSettings.MinDamage += CharacterSettings.MinDamage * transform.localScale.x / 2;

        CharacterSettings.MaxKnockBack += CharacterSettings.MaxKnockBack * transform.localScale.x / 4;
        CharacterSettings.MinKnockBack += CharacterSettings.MinKnockBack * transform.localScale.x / 4;
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
}