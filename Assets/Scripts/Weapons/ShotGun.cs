using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public struct GunShotData
{
    public int nShots;
    [Range(0f,1f)]
    public float _bulletSpread;
    public float _bulletSpeed;
    public float _bulletLifeTime;
    public float _knockBack;
    public GameObject _bulletPrefab;
} 
public class ShotGun : BaseWeapon
{


    [Header("Attack Settings")]
    [SerializeField] private float _primaryRecoilAmount;
    [SerializeField] private float _secondaryRecoilAmount;
    [SerializeField] private GameObject _secondaryAttackExplosion;
    [SerializeField] private GunShotData _gunShotData;
    [SerializeField] private ExplosionData _explosionAttackData;
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Gun Settings")]
    [SerializeField] private float _maxRecoilOffset;
    [SerializeField] private Transform _fp;

    [Header("VFX Settings")]
    [SerializeField] private CamShakeSetting _primaryShotScreenShake;
    [SerializeField] private CamShakeSetting _secondaryShotScreenShake;
    [Tooltip("How far the gun raycast to set current aim target")]
    [SerializeField] private float _aimingDistance;
    [SerializeField] private float _aimSpeed;
    [SerializeField] private LayerMask _aimableLayers;
    [SerializeField] private float _timeBeforeRecoilRecovery;
    [SerializeField] private float _recoilRecoveryRate;


    private Camera _fovCam;
    private bool _canRecover;
    private float _currentTimeBeforeRecoilRecovery;
    private Vector3 _currentRecoilOffset = Vector3.zero;
    private void Update()
    {
        if (!_isOwnerMoving)
            LerpToEquipoint();
        UpdateWeaponAim();
        if(_currentRecoilOffset != Vector3.zero&& _canRecover)
        {
            RecoverFromOverTimeRecoil();
        }

        if(_currentTimeBeforeRecoilRecovery > 0)
        {
            _currentTimeBeforeRecoilRecovery -= Time.deltaTime;
            if (_currentTimeBeforeRecoilRecovery <= 0.0f)
            {
                _canRecover = true;
            } 

        }


        if (!_canPrimaryAttack && _primaryCurrentCooldownTime > 0)
        {
            _primaryCurrentCooldownTime -= Time.deltaTime;
            if (_primaryCurrentCooldownTime <= 0f)
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

        if (_isPrimaryAttacking) ValidatePrimaryAttack();
        if (_isSecondaryAttacking) ValidateSecondaryAttack();
    }
    void FixedUpdate()
    {
        if (_isOwnerMoving)
            FollowEquipPoint();

    }
    public override void Init()
    {
        _canPrimaryAttack = true;
        _canSecondaryAttack = true;
        _canAttack = true;
        _fovCam= FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
        _explosionAttackData._owner = WeaponManager._instance.Getowner();
    
    }
 

    protected override void DoPrimaryAttack()
    {
        _canPrimaryAttack = false;


        PrimaryShot();



    }

    protected override void DoSecondaryAttack()
    {
        _canSecondaryAttack = false;
        SecondaryShot();
    }

    protected override void ResetPrimaryAttack()
    {
        _canPrimaryAttack = true;
    }

    protected override void ResetSecondaryAttack()
    {
        _canSecondaryAttack = true;
    }
    private void PrimaryShot()
    {
        FirePrimaryRound();
        AddRecoil(_primaryRecoilAmount);
        _primaryCurrentCooldownTime = _primaryFireRate;
        if (CamShake.instance)
            CamShake.instance.DoScreenShake(_primaryShotScreenShake);
    }
    private void SecondaryShot()
    {
        FireSecondaryExplosion();
        AddRecoil(_secondaryRecoilAmount);
        _secondaryCurrentCooldownTime = _secondaryFireRate;
        if (CamShake.instance)
            CamShake.instance.DoScreenShake(_secondaryShotScreenShake);
    }


    public void FirePrimaryRound()
    {

        for (int i=0; i< _gunShotData.nShots; i++)
        {
            Vector3 targetDir =_fp.forward + new Vector3( Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread),
                Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread), Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread)) ;


            IProjectile bullet = Instantiate(_gunShotData._bulletPrefab, _fp.position, Quaternion.identity).GetComponent<IProjectile>();


            if (bullet != null)
            {
                ProjectileData bulletData = new ProjectileData(Random.Range(_primaryMinDamage, _primaryMaxDamage), targetDir.normalized
                    , _gunShotData._bulletSpeed, _gunShotData._bulletLifeTime, _gunShotData._knockBack, WeaponManager._instance.Getowner());
                bullet.SetUpProjectile(bulletData);


                bullet.ShootProjectile();
            }

            Debug.DrawRay(_fp.position, targetDir.normalized, Color.green, 10f);
        }
    }
    public void FireSecondaryExplosion()
    {
        RaycastHit hitInfo;
        Vector3 targetpoint = _fp.position + _fp.forward *( (_explosionAttackData._size / 2.0f) - 1f);


        if (Physics.Raycast(_fp.position, _fp.forward, out hitInfo, (_explosionAttackData._size / 2.0f)-1f, _aimableLayers))
        {
            targetpoint = hitInfo.point;
        }

        IExplosion explosion = Instantiate(_explosionPrefab, targetpoint, Quaternion.identity).GetComponent<IExplosion>();
        if(explosion != null)
        {
            explosion.InitExplosion(_explosionAttackData);
        }
    }
    public void AddRecoil(float recoilAmount)
    {
        _currentRecoilOffset += new Vector3(0f, recoilAmount, 0f);
        float currRecoil = _currentRecoilOffset.y;
        currRecoil = Mathf.Clamp(currRecoil, 0f, _maxRecoilOffset);

        _currentRecoilOffset += new Vector3(0f, currRecoil, 0f);
        _canRecover = false;
        _currentTimeBeforeRecoilRecovery = _timeBeforeRecoilRecovery;
    }

    public void UpdateWeaponAim()
    {
        if (!_fovCam) return;
        Vector3 aimDir = EssoUtility.GetCameraLookAtPoint(_fovCam,_aimingDistance,_aimableLayers)- _fp.position;

        Debug.DrawRay(_fp.position, aimDir, Color.yellow);
        Quaternion q=  Quaternion.LookRotation(aimDir + _currentRecoilOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _aimSpeed);
    }
    private void RecoverFromOverTimeRecoil()
    {
       _currentRecoilOffset = Vector3.MoveTowards(_currentRecoilOffset,Vector3.zero, Time.deltaTime * _recoilRecoveryRate *_currentRecoilOffset.y);

        if(_currentRecoilOffset == Vector3.zero)
        {
            _canRecover = false;
        }
    }



}
