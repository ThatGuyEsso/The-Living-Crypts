using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Legacy : BaseWeapon, IWeapon, IAttacker
{
    //Collider
    [SerializeField] private Collider SwordAttackCollider;
    [SerializeField] private GameObject AttackVolumePrefab;
    [Header("Animation Settings")]
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;
    [SerializeField] private SmoothMatchParentRotLoc _idleReset;

    [SerializeField] private float _idleResetRate = 5f;

    [Header("Sword Swing SFX")]
    [SerializeField] private string LeftSwingSFX;
    [SerializeField] private string RightSwingSFX;
    [SerializeField] private string ThrustSFX;

    private bool _isSwingingRight = false;

    private float _primCurrTimeToIdle;
    private float _secCurrTimeToIdle;
    private bool _isAttacking;

    private AttackCollider _attackVolume;
    public override void Init()
    {
        base.Init();
        if (!SwordAttackCollider) Debug.LogError("Sword has Not attack Collider");
        else SwordAttackCollider.enabled = false;

        if (_animController)
        {
            _animController.OnAttackAnimBegin += ActivateAttackCollider;
         
        }
    }
    public override void StopTryToPrimaryAttack()
    {
        _isPrimaryAttacking = false;
    }

    public override void StopTryToSecondaryAttack()
    {
        _isSecondaryAttacking = false;
    }

    public override void TryToPrimaryAttack()
    {
        
        _isPrimaryAttacking = true;
  
        if (_canPrimaryAttack&&_canAttack&&!_isAttacking)
        {
            DoPrimaryAttack();
        }
    }

    public override void TryToSecondaryAttack()
    {
        _isSecondaryAttacking = true;
        if (_canSecondaryAttack&& _canAttack&&!_isAttacking)
        {
            DoSecondaryAttack();
        }
    }

    protected override void DoPrimaryAttack()
    {
        if (_idleReset) _idleReset.Stop();
        _canPrimaryAttack = false;
        _isAttacking = true;

        if (_animController)
        {
          
            _animController.OnAttackAnimEnd += OnPrimaryAttackEnd;
            if (_isSwingingRight)
            {
                PlaySFX(RightSwingSFX, true);
                _animController.PlayPrimaryAttackAnimation(1);
                _isSwingingRight = false;
            }
            else
            {
                PlaySFX(LeftSwingSFX, true);
                _animController.PlayPrimaryAttackAnimation(0);
                _isSwingingRight = true;
            }
        }


        ResetIdleTimers();
 
    }

    public void ActivateAttackCollider()
    {
        if (SwordAttackCollider)
        {
            if (!SwordAttackCollider.enabled)
            {
                SwordAttackCollider.enabled = true;
            }
              
        }

     
   

            if(!_attackVolume && AttackVolumePrefab)
            {
                if (ObjectPoolManager.instance)
                {
                    _attackVolume =ObjectPoolManager.Spawn(AttackVolumePrefab, 
                        transform.root.position + transform.root.forward, transform.root.rotation).GetComponent<AttackCollider>();
                }
                else
                {
                    _attackVolume = Instantiate(AttackVolumePrefab,
                        transform.root.position + transform.root.forward, transform.root.rotation).GetComponent<AttackCollider>();
                }

         
            }
            if (_attackVolume)
            {
                _attackVolume.SetOwner(gameObject);
                _attackVolume.IsEnabled=true;
            }
        

      

    }

    public void DisableAttackColliders()
    {
        if (_attackVolume)
        { 
            _attackVolume.IsEnabled = false;
            if (ObjectPoolManager.instance && _attackVolume.gameObject)
            {
                ObjectPoolManager.Recycle(_attackVolume.gameObject);
            }
            else if(_attackVolume.gameObject)
            {
                Destroy(_attackVolume.gameObject);
              
            }
            _attackVolume = null;
        }
    }
    protected override void DoSecondaryAttack()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd += OnSecondaryAttackEnd;

        }
        if (_idleReset) _idleReset.Stop();
        _canSecondaryAttack = false;
        _isAttacking = true;
        PlaySFX(ThrustSFX, true);
        _animController.PlaySecondaryAttackAnimation(0);
        ResetIdleTimers();

        
    }

    private void ResetIdleTimers()
    {
        _primCurrTimeToIdle = 0f;

        _secCurrTimeToIdle = 0f;
    }
    protected override void ResetPrimaryAttack()
    {
  
        _canPrimaryAttack = true;

    }


    public void OnPrimaryAttackEnd()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd -= OnPrimaryAttackEnd;

        }
        _primaryCurrentCooldownTime = _primaryFireRate;
        _primCurrTimeToIdle = _primaryTimeToIdle;
        _isAttacking = false;
        DisableAttackColliders();
              
    }
    public void OnSecondaryAttackEnd()
    {
        if (_animController)
        {
            _animController.OnAttackAnimEnd -= OnSecondaryAttackEnd;

        }
        _secondaryCurrentCooldownTime = _secondaryFireRate;
        _secCurrTimeToIdle = _secondaryTimeToIdle;
        _isAttacking = false;
        DisableAttackColliders();
    }

    protected override void ResetSecondaryAttack()
    {
        _canSecondaryAttack = true;
   
    }

    private void Update()
    {
        //if (_isOwnerMoving)
        //    LerpToEquipoint();
        //else
        FollowEquipPoint();

        MatchEquipPointRotation();

        if (!_canPrimaryAttack && _primaryCurrentCooldownTime > 0)
        {
            _primaryCurrentCooldownTime -= Time.deltaTime;
            if (_primaryCurrentCooldownTime <= 0f)
            {

                ResetPrimaryAttack();
            }

        }


        //Secondary attack has cool down
        if (!_canSecondaryAttack&& _secondaryCurrentCooldownTime>0)
        {
            _secondaryCurrentCooldownTime -= Time.deltaTime;
            if (_secondaryCurrentCooldownTime <= 0f)
            {
                
                ResetSecondaryAttack();
            }
         
        }

        if (_primCurrTimeToIdle > 0)
        {
            _primCurrTimeToIdle -= Time.deltaTime;
            if (_primCurrTimeToIdle <=0f)
            {
                _isSwingingRight = false;
                _animController.StopAnimating();
                if (_idleReset)
                {
                    _idleReset.ResetChild(_idleResetRate);
                }
            }
        }
        if (_secCurrTimeToIdle > 0)
        {
            _secCurrTimeToIdle -= Time.deltaTime;
            if (_secCurrTimeToIdle <= 0f)
            {
           
                _animController.StopAnimating();
                if (_idleReset)
                {
                    _idleReset.ResetChild(_idleResetRate);
                }
            }
        }
        
        if (_isPrimaryAttacking) ValidatePrimaryAttack();
        if (_isSecondaryAttacking) ValidateSecondaryAttack();
    }
  


    public void ApplyDamageToTarget(GameObject target,Vector3 point)
    {
        if (target.transform.parent != WeaponManager._instance.Getowner())
        {
            IDamage damage = target.GetComponent<IDamage>();

            if (damage != null)
            {
                if (_animController.IsPlayingPrimaryAttack())
                {
                    float dmg = Random.Range(_primaryMinDamage, _primaryMaxDamage);
                    float kBack = Random.Range(_primaryMinKnockback, _primaryMaxKnockback);
                    damage.OnDamage(dmg, WeaponManager._instance.Getowner().transform.forward,
                        kBack, WeaponManager._instance.Getowner(), point);
                }
                else if (_animController.IsPlayingSecondaryAttack())
                {
                    float dmg = Random.Range(_secondaryMinDamage, _secondaryMaxDamage);
                    float kBack = Random.Range(_secondaryMinKnockback, _secondaryMaxKnockback);
                    damage.OnDamage(dmg, WeaponManager._instance.Getowner().transform.forward, 
                        kBack, WeaponManager._instance.Getowner(), point);
                }
               
            }

        }
    }

    public AttackData GetAttackData()
    {
        if (_isAttacking &&!_canPrimaryAttack)
        {
            return new AttackData(_primaryMinDamage, _primaryMaxDamage, _primaryMinKnockback, _primaryMaxKnockback);
        }
        else if (_isAttacking && !_canSecondaryAttack)
        {
            return new AttackData(_secondaryMinDamage, _secondaryMaxDamage, _secondaryMinKnockback, _secondaryMaxKnockback);
        }
        else return new AttackData(0f, 0f, 0f, 0f);


    }
}
