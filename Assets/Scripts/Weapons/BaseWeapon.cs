using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour, Iteam
{
    [Header("Weapon Settings")]
    [SerializeField] protected string _weaponName;
    [SerializeField] protected float _primaryMinDamage,  _primaryMaxDamage, _secondaryMinDamage, _secondaryMaxDamage;
    [SerializeField] protected float _primaryMinKnockback, _primaryMaxKnockback, _secondaryMinKnockback, _secondaryMaxKnockback;
    [SerializeField] protected Transform _equipTransform;
    [SerializeField] protected float _primaryFireRate,_secondaryFireRate;
    [SerializeField] protected float _followEquipSpeed = 20f;
    protected bool _canPrimaryAttack,_canSecondaryAttack,_canAttack;
    protected bool _isPrimaryAttacking, _isSecondaryAttacking;
    protected float _primaryCurrentCooldownTime, _secondaryCurrentCooldownTime;
    protected Vector3 _equipOffset;
    protected AudioManager AM;

    [Header("Weapon UI")]
    [SerializeField] protected Sprite _primaryAttackUISprite, _secondaryAttackUISprite;

    public System.Action OnAttackStarted;
    public System.Action OnAttackEnded;
    public System.Action<float> OnNewPrimaryCooldown;
    public System.Action<float> OnNewSecondaryCooldown;
    [Header("Weapon Animation")]
    [SerializeField] protected WeaponAnimController _animController;
    public virtual void StopTryToPrimaryAttack()
    {
        _isPrimaryAttacking = false;
    }

    public virtual void StopTryToSecondaryAttack()
    {
        _isSecondaryAttacking = false;
    }


    public virtual void TryToPrimaryAttack()
    {

        _isPrimaryAttacking = true;

        ValidatePrimaryAttack();
    }


    public virtual void TryToSecondaryAttack()
    {
        _isSecondaryAttacking = true;
        ValidateSecondaryAttack();
    }

    protected abstract void DoPrimaryAttack();


    protected bool _isOwnerMoving;

    protected abstract void DoSecondaryAttack();


    protected abstract void ResetPrimaryAttack();
    protected abstract void ResetSecondaryAttack();
    public virtual void Init()
    {
        _canPrimaryAttack = true;
        _canSecondaryAttack = true;
        _canAttack = true;
        if (!_animController)
            _animController = GetComponent<WeaponAnimController>();
        _animController.Init();
    }
    protected virtual void FollowEquipPoint()
    {
        if (_equipTransform)
        {
            transform.position =  _equipTransform.position;
        }
    }
    protected virtual void FollowEquipPoint(Vector3 offset)
    {
        if (_equipTransform && offset == Vector3.zero) 
        {
            transform.position = _equipTransform.position ;
        }
        else if(_equipTransform && offset != Vector3.zero)
        {
            transform.position = _equipTransform.position;
            transform.position =Vector3.Lerp(transform.position, transform.position +offset,Time.deltaTime*_followEquipSpeed*2f);
        }
    }
    protected virtual void LerpToEquipoint()
    {
        if (_equipTransform)
        {
            transform.position =Vector3.Lerp(transform.position, _equipTransform.position,Time.deltaTime* _followEquipSpeed);
            if (Vector3.Distance(transform.position, _equipTransform.position) <= 0.1f) transform.position = _equipTransform.position;
        }
    }
    protected virtual void MatchEquipPointRotation()
    {
        if (_equipTransform)
        {
            transform.rotation = _equipTransform.rotation;
        }
    }
    virtual public void ValidatePrimaryAttack()
    {
        if (_canPrimaryAttack && _canAttack && !_isSecondaryAttacking)
        {
            DoPrimaryAttack();
        }
    }

    virtual public void ValidateSecondaryAttack()
    {
        if (_canSecondaryAttack && _canAttack && !_isPrimaryAttacking)
        {
            DoSecondaryAttack();
        }
    }
    public virtual void SetEquipPoint(Transform equipTransform)
    {
        _equipTransform = equipTransform;
    }


    public void SetIsMoving(bool isMoving)
    {
        _isOwnerMoving = isMoving;
    }

    protected void PlaySFX(string SFX, bool randPitch)
    {
        if (!AM)
        {
            if(!GameStateManager.instance|| !GameStateManager.instance.AudioManager)
            {
                return;
            }
            AM = GameStateManager.instance.AudioManager;
        }

        AM.PlayThroughAudioPlayer(SFX, transform.position, randPitch);
    }

 
    public Team GetTeam()
    {
       return Team.Player; 
    }

    public bool IsOnTeam(Team team)
    {
        return team == Team.Player;
     
    }

    public string WeaponName { get { return _weaponName; } }


    public Sprite PrimaryUISprite { get { return _primaryAttackUISprite; } }
    public Sprite SecondaryUISprite { get { return _secondaryAttackUISprite; } }

    public virtual float MaxPrimaryCooldown { get { return _primaryFireRate; } }
    public virtual float MaxSecondaryCooldown { get { return _secondaryFireRate; } }

    public AudioManager AudioManager
    {
        get
        {
            return AM;
        }

        set
        {
            AM = value;
        }
    }
}
