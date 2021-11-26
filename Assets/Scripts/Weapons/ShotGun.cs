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
    public GameObject _bulletPrefab;
} 
public class ShotGun : BaseWeapon
{



    [Header("Gun Settings")]
    [SerializeField] private float _maxRecoilOffset;
    [SerializeField] private float _primaryRecoilAmount;

    [SerializeField] private Transform _fp;
    [SerializeField] private GunShotData _gunShotData;
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
        if (_isPrimaryAttacking) ValidatePrimaryAttack();
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

        ValidatePrimaryAttack();
    }

    public void ValidatePrimaryAttack()
    {
        if (_canPrimaryAttack && _canAttack)
        {
            DoPrimaryAttack();
        }
    }
    public override void TryToSecondaryAttack()
    {
        _isSecondaryAttacking = true;
        if (_canSecondaryAttack && _canAttack )
        {
            DoSecondaryAttack();
        }
    }


    protected override void DoPrimaryAttack()
    {
        _canPrimaryAttack = false;


        PrimaryShot();



    }

    protected override void DoSecondaryAttack()
    {
       
    }

    protected override void ResetPrimaryAttack()
    {
        _canPrimaryAttack = true;
    }

    protected override void ResetSecondaryAttack()
    {
        
    }
    private void PrimaryShot()
    {
        FirePrimaryRound();
        AddRecoil(_primaryRecoilAmount);
        _primaryCurrentCooldownTime = _primaryfireRate;
    }

    public void FirePrimaryRound()
    {
       

        for (int i=0; i< _gunShotData.nShots; i++)
        {
            Vector3 targetDir = _fp.forward.normalized +new Vector3( Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread),
                Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread), Random.Range(-_gunShotData._bulletSpread, _gunShotData._bulletSpread)) ;


            IProjectile bullet = Instantiate(_gunShotData._bulletPrefab, _fp.position, Quaternion.identity).GetComponent<IProjectile>();


            if (bullet != null)
            {
                ProjectileData bulletData = new ProjectileData(Random.Range(_primaryMinDamage, _primaryMaxDamage), targetDir.normalized
                    , _gunShotData._bulletSpeed, _gunShotData._bulletLifeTime, WeaponManager._instance.Getowner());
                bullet.SetUpProjectile(bulletData);


                bullet.ShootProjectile();
            }

            Debug.DrawRay(_fp.position, targetDir, Color.green, 10f);
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
        Vector3 aimPoint = EssoUtility.GetCameraLookAtPoint(_fovCam,_aimingDistance,_aimableLayers);
        Quaternion q=  Quaternion.LookRotation(aimPoint+ _currentRecoilOffset);
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
