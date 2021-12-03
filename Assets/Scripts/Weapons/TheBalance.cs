using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BeamSettings
{
    public float _distance;
    public float _maxDuration;
    public LayerMask _targetLayers;
    public Transform _firePoint;

}

[System.Serializable]
public struct ShieldSettings
{
    public GameObject _owner;
    public float _lifeTime;
    public float _minDamage;
    public float _maxDamage;
    public float _minKnockBack;
    public float _maxKnockBack;
    public ShieldSettings(GameObject owner, float lifeTime,float minDmg,float maxDmg, float minKnockBack, float maxKnockBack)
    {
        _owner = owner;
        _lifeTime = lifeTime;
        _minDamage = minDmg;
        _maxDamage = maxDmg;
        _minKnockBack = minKnockBack;
        _maxKnockBack = maxKnockBack;
    }

}
public class TheBalance : BaseWeapon
{
    [Header("Beam Settings")]
    [SerializeField] private BeamSettings _beamSettings;
    [SerializeField] private float _primaryAttackCooldown;

    [Header("Shield Settings")]
    [SerializeField] private ShieldSettings _shieldSettings;
    [SerializeField] private GameObject _shieldPrefab;

    [Header("Animation Settings")]
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;

    [SerializeField] private Transform _meshTranform;
    [SerializeField] private SmoothMatchParentRotLoc _idleReset;
    [SerializeField] private GameObjectShake _shaker;

    private float _primCurrTimeToIdle;
    private float _secCurrTimeToIdle;

    //states
    private bool _isAttacking;
    public override void SetEquipPoint(Transform equipTransform)
    {
        base.SetEquipPoint(equipTransform);
        if (_shaker) SetEquipPoint(equipTransform);
    }
    protected override void DoPrimaryAttack()
    {
        if (_idleReset) _idleReset.Stop();
        _canPrimaryAttack = false;
        _isAttacking =true;
        _animController.OnAttackAnimEnd += BeginBeam;
        _animController.PlayPrimaryAttackAnimation(0);
        ResetIdleTimers();
    }

    public void BeginBeam()
    {
        _animController.OnAttackAnimEnd -= BeginBeam;
        if (_shaker) _shaker.BeginShake();


    }


    protected override void DoSecondaryAttack()
    {
        if (_idleReset) _idleReset.Stop();
        _canSecondaryAttack = false;
        _isAttacking = true;
        _animController.OnAttackAnimEnd += SummonShield;
        _animController.PlaySecondaryAttackAnimation(0);
        ResetIdleTimers();
    }


    public void SummonShield()
    {
        _animController.OnAttackAnimEnd -= SummonShield;
    }

    protected override void ResetPrimaryAttack()
    {
        _isAttacking = false;
        _primaryCurrentCooldownTime = _primaryAttackCooldown;
        _primCurrTimeToIdle = _primaryTimeToIdle;


    }
    protected override void ResetSecondaryAttack()
    {
        _isAttacking = false;
        _secondaryCurrentCooldownTime = _secondaryFireRate;
        _secCurrTimeToIdle = _secondaryTimeToIdle;
    }
    private void Update()
    {
        if (!_isOwnerMoving)
            LerpToEquipoint();
    }
    private void FixedUpdate()
    {
        if (_isOwnerMoving)
            FollowEquipPoint();
        MatchEquipPointRotation();
    }

    private void ResetIdleTimers()
    {
        _primCurrTimeToIdle = 0f;

        _secCurrTimeToIdle = 0f;
    }

}
