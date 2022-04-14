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


public class TheBalance : BaseWeapon
{
    [Header("Beam Settings")]
    [SerializeField] private BeamSettings _beamSettings;
    [SerializeField] private float _primaryAttackCooldown;
    [SerializeField] private LineManager _beamLineManager;
    //[SerializeField] private GameObject _beamOriginVFXPrefab;
    [SerializeField] private GameObject _beamHitVFXPrefab;
    private GameObject _beamOriginVFX;

    [Header("SFX")]
    [SerializeField] private string InitBeamSFX, BeamSustainSFX, ShieldSpawnSFX;
    private AudioPlayer BeamSustainSFXPlayer;
    [Header("Shield Settings")]
    [SerializeField] private ShieldSettings _shieldSettings;
    [SerializeField] private GameObject _shieldPrefab;
    [SerializeField] private Vector3 _shieldSpwnOffset;
    [Header("Animation Settings")]
    [SerializeField] private float _primaryTimeToIdle;
    [SerializeField] private float _secondaryTimeToIdle;
    [SerializeField] private float _aimSpeed;
    [SerializeField] private Vector3 _aimOffset;
    [SerializeField] private Transform _meshTranform;
    [SerializeField] private SmoothMatchParentRotLoc _idleReset;
    [SerializeField] private float _idleResetRate = 5f;
    [SerializeField] private GameObjectShake _shaker;


    private float _currentBeamDuration;
    private float _currentBeamCooldown;
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

        //populating shield data
        _shieldSettings._minDamage = _secondaryMinDamage;
        _shieldSettings._maxDamage = _secondaryMaxDamage;
        _shieldSettings._minKnockBack = _secondaryMinKnockback;
        _shieldSettings._maxKnockBack = _secondaryMaxKnockback;
        _shieldSettings._owner =WeaponManager._instance.Getowner();
    }
    public override void StopTryToPrimaryAttack()
    {
        base.StopTryToPrimaryAttack();
        if (_isCastingBeam) StopBeam();
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
        OnAttackStarted?.Invoke();
    }

    public void BeginBeam()
    {
        _animController.OnAttackAnimEnd -= BeginBeam;
        _isCastingBeam = true;
        Transform currentMeshTransform = _meshTranform;
        _animController.StopAnimating();
        _meshTranform.rotation = _meshTranform.rotation;

        PlaySFX(InitBeamSFX,true);
        _currentBeamDuration = _beamSettings._maxDuration;
        _primCurrentTickRate = 0f;
        PlayBeamSFX();
    }

    public void PlayBeamSFX()
    {
        if (!BeamSustainSFXPlayer)
        {
            if (!AM)
            {
                if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
                {
                    return;
                }
                AM = GameStateManager.instance.AudioManager;
            }
            if (AM)
            {
                BeamSustainSFXPlayer = AM.PlayThroughAudioPlayer(BeamSustainSFX, transform.position, false);
                BeamSustainSFXPlayer.transform.SetParent(WeaponManager._instance.Getowner().transform);
            }


        }
        else
        {
            BeamSustainSFXPlayer.Play();
        }
    }

    public void StopBeamSFX()
    {
        if (BeamSustainSFXPlayer)
        {
            BeamSustainSFXPlayer.KillAudio();
            BeamSustainSFXPlayer = null;
        }
    }
    public void RotateInAimDirectionOverTime()
    {
        if (!_fovCam) return;
        Vector3 aimDir = EssoUtility.GetCameraLookAtPoint(_fovCam, _beamSettings._distance, _beamSettings._targetLayers) - _beamSettings._firePoint.position;

        Debug.DrawRay(_beamSettings._firePoint.position, aimDir, Color.yellow);
        Quaternion q = Quaternion.LookRotation(aimDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _aimSpeed);
    }
  
    public void DrawBeam()
    {
        //if (!_beamOriginVFX&&_beamOriginVFXPrefab) _beamOriginVFX = Instantiate(_beamOriginVFXPrefab, Vector3.zero, _beamSettings._firePoint.rotation);
        //if (_beamOriginVFX) _beamOriginVFX.transform.position = _beamSettings._firePoint.position;
        Vector3 dir = EssoUtility.GetCameraLookAtPoint(_fovCam, _beamSettings._distance, _beamSettings._targetLayers) - _beamSettings._firePoint.position;
        List<Vector3> points = new List<Vector3>();
        Vector3 aimPoint = _beamSettings._firePoint.position +dir.normalized  * _beamSettings._distance;
        RaycastHit hitInfo;
  


        if (Physics.Raycast(_beamSettings._firePoint.position, dir.normalized, out hitInfo, _beamSettings._distance,
            _beamSettings._targetLayers))
        {
            aimPoint = hitInfo.point;
            
        }

          
         SpawnHitVFX(aimPoint);
            
      
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
                    damage.OnDamage(dmg, dir.normalized, kBack, WeaponManager._instance.Getowner(),hitInfo.point);

                    SpawnHitVFX(hitInfo.point);
                }

            }
        }
    
        _primCurrentTickRate = _primaryFireRate;


    }
    public void CancelBeam()
    {
        StopBeamSFX();
        _beamLineManager.ClearLine();
        _isCastingBeam = false;
        if (_beamOriginVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(_beamOriginVFX);
            }
            else
            {
                Destroy(_beamOriginVFX);
            }

        }
        _currentBeamCooldown = _primaryAttackCooldown;
        _primCurrTimeToIdle = 0;
        _isAttacking = false;

        _animController.StopAnimating();
        if (_idleReset)
        {
            _idleReset.ResetChild(_idleResetRate);
        }
    }
    public void StopBeam()
    {
        StopBeamSFX();
        _beamLineManager.ClearLine();
        _isCastingBeam = false;
        if (_beamOriginVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(_beamOriginVFX);
            }
            else
            {
                Destroy(_beamOriginVFX);
            }
          
        }
        _currentBeamCooldown = _primaryAttackCooldown;
        _primCurrTimeToIdle = _primaryTimeToIdle;
        _isAttacking = false;

        OnAttackEnded?.Invoke();
    }
    public void SpawnHitVFX(Vector3 point)
    {
        if (_beamHitVFXPrefab)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(_beamHitVFXPrefab, point, Quaternion.identity);
            }
            Instantiate(_beamHitVFXPrefab, point, Quaternion.identity);
        }
           
    }

    protected override void DoSecondaryAttack()
    {
        if (_idleReset) _idleReset.Stop();
        OnAttackStarted?.Invoke();
        _canSecondaryAttack = false;
        _isAttacking = true;
        _animController.OnAttackAnimEnd += SummonShield;
        _animController.PlaySecondaryAttackAnimation(0);
        ResetIdleTimers();
    }


    public void SummonShield()
    {
        _animController.OnAttackAnimEnd -= SummonShield;

        AttackBubbleShield shield = Instantiate(_shieldPrefab,
            WeaponManager._instance.Getowner().transform.position + WeaponManager._instance.Getowner().transform.forward,
            WeaponManager._instance.Getowner().transform.rotation).GetComponent<AttackBubbleShield>();
        shield.transform.SetParent(WeaponManager._instance.Getowner().transform);

        PlaySFX(ShieldSpawnSFX,true);
        if (shield)
        {
            shield.Init(_shieldSettings, null);
        }

        _secondaryCurrentCooldownTime = _secondaryFireRate;
        _secCurrTimeToIdle = _secondaryTimeToIdle;
        _isAttacking = false;
    
    }

    public override void ValidatePrimaryAttack()
    {
        if (_canPrimaryAttack && _canAttack && !_isAttacking)
        {
            DoPrimaryAttack();
        }
    }

    public override void ValidateSecondaryAttack()
    {
        if (_canSecondaryAttack && _canAttack && !_isAttacking)
        {
            DoSecondaryAttack();
        }
    }
    protected override void ResetPrimaryAttack()
    {

        _canPrimaryAttack = true;


    }
    protected override void ResetSecondaryAttack()
    {
        _canSecondaryAttack = true;
    }
    private void Update()
    {
    
        if (_isCastingBeam)
        {
            if (!_isPrimaryAttacking)
            {
                CancelBeam();
                return;
            }
            Vector3 offset = Vector3.zero;
            if (_shaker) offset = _shaker.GetShakeOffset();

            FollowEquipPoint(offset);

            DrawBeam();

            if (_primCurrentTickRate < 0f)
            {
                DoBeamAttack();
            }
            else
            {
                _primCurrentTickRate -= Time.deltaTime;
            }
            
            if (_currentBeamDuration <= 0)
            {
                StopBeam();
            }
            else
            {
                _currentBeamDuration -= Time.deltaTime;
            }
            


        }
        else
        {
          
            FollowEquipPoint();
         
        
        }


        if (!_canPrimaryAttack && _currentBeamCooldown > 0)
        {
            _currentBeamCooldown -= Time.deltaTime;
            if (_currentBeamCooldown <= 0f)
            {

                ResetPrimaryAttack();
            }

        }
        if (!_canSecondaryAttack && _secondaryCurrentCooldownTime > 0)
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
            if (_primCurrTimeToIdle <= 0f)
            {
            
                _animController.StopAnimating();
                if (_idleReset) _idleReset.ResetChild(_idleResetRate);
            }
        }
        if (_secCurrTimeToIdle > 0)
        {
            _secCurrTimeToIdle -= Time.deltaTime;
            if (_secCurrTimeToIdle <= 0f)
            {
                OnAttackEnded?.Invoke();
                _animController.StopAnimating();
                if (_idleReset) _idleReset.ResetChild(_idleResetRate);
            }
        }

        if (_isPrimaryAttacking) ValidatePrimaryAttack();
        if (_isSecondaryAttacking) ValidateSecondaryAttack();
    }
    private void LateUpdate()
    {
        if(!_isCastingBeam)
            MatchEquipPointRotation();
       
        else if(_isCastingBeam&&!_animController.IsPlayingPrimaryAttack())
        {
          
            
            RotateInAimDirectionOverTime();
        }
        
      
    }
    private void ResetIdleTimers()
    {
        _primCurrTimeToIdle = 0f;

        _secCurrTimeToIdle = 0f;
    }

}
