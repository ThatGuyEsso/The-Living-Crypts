using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
    [SerializeField] private LineManager _beamLineManager;
    [SerializeField] private GameObject _beamOriginVFXPrefab;
    [SerializeField] private GameObject _beamHitVFXPrefab;
    private GameObject _beamOriginVFX;

    [Header("Shield Settings")]
    [SerializeField] private ShieldSettings _shieldSettings;
    [SerializeField] private GameObject _shieldPrefab;

    [Header("Animation Settings")]
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;

    [SerializeField] private Transform _meshTranform;
    [SerializeField] private SmoothMatchParentRotLoc _idleReset;
    [SerializeField] private GameObjectShake _shaker;


    private float _primCurrentTickRate;
    private float _primCurrTimeToIdle;
    private float _secCurrTimeToIdle;
    private bool _isCastingBeam;
    //states
    private bool _isAttacking;
    private Camera _fovCam;
    public override void Init()
    {
        base.Init();
        if (_beamLineManager) _beamLineManager.Init();
        _beamLineManager.SetOrigin(_beamSettings._firePoint);
        _fovCam = FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
    }
    public override void SetEquipPoint(Transform equipTransform)
    {
        base.SetEquipPoint(equipTransform);
        if (_shaker) _shaker.SetAnchor(equipTransform);
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
        _isCastingBeam = true;

        _primCurrentTickRate = 0f;
    }

    public void DrawBeam()
    {
        if (!_beamOriginVFX&&_beamOriginVFXPrefab) _beamOriginVFX = Instantiate(_beamOriginVFXPrefab, Vector3.zero, _beamSettings._firePoint.rotation);
        if (_beamOriginVFX) _beamOriginVFX.transform.position = _beamSettings._firePoint.position;

        Vector3 aimPoint = EssoUtility.GetCameraLookAtPoint(_fovCam, _beamSettings._distance, _beamSettings._targetLayers);
        
        List<Vector3> points = new List<Vector3>();
        points.Add(aimPoint);

        _beamLineManager.DrawLinePositions(points);
   
    }
    public void DoBeamAttack()
    {
        RaycastHit hitInfo;
        Vector3 dir = EssoUtility.GetCameraLookAtPoint(_fovCam, _beamSettings._distance, _beamSettings._targetLayers)- _beamSettings._firePoint.position;


        if (Physics.Raycast(_beamSettings._firePoint.position, dir.normalized, out hitInfo, _beamSettings._distance, _beamSettings._targetLayers))
        {
            if (WeaponManager._instance.Getowner() != hitInfo.collider.gameObject)
            {
                IDamage damage = hitInfo.collider.gameObject.GetComponent<IDamage>();
                if (damage != null)
                {
                    float dmg = Random.Range(_primaryMinDamage, _primaryMaxDamage);
                    float kBack = Random.Range(_primaryMinKnockback, _primaryMaxKnockback);
                    damage.OnDamage(dmg, dir.normalized, kBack, WeaponManager._instance.Getowner());

                    SpawnHitVFX(hitInfo.point);
                }

            }
        }
        _primCurrentTickRate = _primaryFireRate;


    }

    public void SpawnHitVFX(Vector3 point)
    {
        if (_beamHitVFXPrefab)
            Instantiate(_beamHitVFXPrefab, point, Quaternion.identity);
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
    
        if (_isCastingBeam)
        {
            Vector3 offset =Vector3.zero;
            if (_shaker) offset= _shaker.GetShakeOffset();

            FollowEquipPoint(offset);

            DrawBeam();
            if(_primCurrentTickRate < 0f)
            {
                DoBeamAttack();
            }
            else
            {
                _primCurrentTickRate -= Time.deltaTime;
            }

            


        }
        else
        {
            //if (_isOwnerMoving)
                FollowEquipPoint();
           //else
           //     LerpToEquipoint();
        
        }


    
       
    }
    private void LateUpdate()
    {
        MatchEquipPointRotation();
    }
    private void ResetIdleTimers()
    {
        _primCurrTimeToIdle = 0f;

        _secCurrTimeToIdle = 0f;
    }

}
