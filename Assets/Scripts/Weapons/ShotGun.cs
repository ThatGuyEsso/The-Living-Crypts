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
[System.Serializable]
public struct GunHitScanData
{
    public int nShots;
    [Range(0f, 1f)]
    public float Spread;
    public float ScanRange;
    public LayerMask BlockingLayers;
}

public struct HitScanTarget
{
    public GameObject Target;
    public float CurrentDamage;
    public float CurrentKnockBack;
    public Vector3 Direction;
    public Vector3 HitPoint;

    public HitScanTarget(GameObject target, float damage,float knockback,Vector3 direction,Vector3 hitPoint)
    {
        Target = target;
        CurrentDamage = damage;
        CurrentKnockBack = knockback;
        Direction = direction;
        HitPoint = hitPoint;
    }

 

}
public class ShotGun : BaseWeapon
{


    [Header("Attack Settings")]
    [SerializeField] private float _primaryRecoilAmount;
    [SerializeField] private float _secondaryRecoilAmount;
    [SerializeField] private GameObject _secondaryAttackExplosion;
    [SerializeField] private GunHitScanData GunScanData;
    [SerializeField] private ExplosionData _explosionAttackData;
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Gun Settings")]
    [SerializeField] private float _maxRecoilOffset;
    [SerializeField] private Transform _fp;
    [SerializeField] private float BulletRange;

    [Header("Shotgun SFX")]
    [SerializeField] private string PrimaryBlastSFX;
    [SerializeField] private string SecondaryBlastSFX;
  

    [Header("VFX Settings")]
    [SerializeField] private GameObject MuzzleFlashPrefab;
    [SerializeField] private CamShakeSetting _primaryShotScreenShake;
    [SerializeField] private CamShakeSetting _secondaryShotScreenShake;
    [SerializeField] private float MuzzleDuration;
    [Tooltip("How far the gun raycast to set current aim target")]
    [SerializeField] private float _aimingDistance;
    [SerializeField] private float _aimSpeed;
    [SerializeField] private LayerMask _aimableLayers;
    [SerializeField] private float _timeBeforeRecoilRecovery;
    [SerializeField] private float _recoilRecoveryRate;

    private GameObject _muzzleFlash;
    private Camera _fovCam;
    private bool _canRecover;
    private float _currentTimeBeforeRecoilRecovery;
    private Vector3 _currentRecoilOffset = Vector3.zero;
    
    private void Update()
    {
        //if (!_isOwnerMoving)
        //    LerpToEquipoint();
        //else
         FollowEquipPoint();
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
        PlaySFX(PrimaryBlastSFX, true);
        if (CamShake.instance)
            CamShake.instance.DoScreenShake(_primaryShotScreenShake);
    }
    private void SecondaryShot()
    {
        FireSecondaryExplosion();
        AddRecoil(_secondaryRecoilAmount);
              PlaySFX(SecondaryBlastSFX, true);
        _secondaryCurrentCooldownTime = _secondaryFireRate;
        if (CamShake.instance)
            CamShake.instance.DoScreenShake(_secondaryShotScreenShake);
    }


    public void FirePrimaryRound()
    {
      
        RaycastHit hitInfo;

        List<HitScanTarget> targetsHit = new List<HitScanTarget>();
        for (int i=0; i< GunScanData.nShots; i++)
        {
            Vector3 targetDir = _fp.forward +_fp.right *Random.Range(-GunScanData.Spread, GunScanData.Spread)+ _fp.up * Random.Range(-GunScanData.Spread, GunScanData.Spread);

            if (Physics.Raycast(_fp.position, targetDir, out hitInfo, GunScanData.ScanRange, GunScanData.BlockingLayers))
            {
                Debug.DrawLine(_fp.position, hitInfo.point, Color.red, 10f);
                if (targetsHit.Count == 0)
                {
                    float dmg = Random.Range(_primaryMinDamage, _primaryMaxDamage);
                    float kBack = Random.Range(_primaryMinKnockback, _primaryMaxKnockback);

                    targetsHit.Add(new HitScanTarget(hitInfo.collider.transform.root.gameObject, dmg, kBack, targetDir, hitInfo.point));
                }
                else
                {
                    int indexToUpdate = 0;
                    bool valueFound = false;
                    for (int j = 0; j < targetsHit.Count; j++)
                    {
                        if (targetsHit[j].Target == hitInfo.collider.transform.root.gameObject)
                        {
                            valueFound = true;
                            indexToUpdate = j;
                            break;
                        }
                    }


                    if (valueFound)
                    {
                        float dmg = Random.Range(_primaryMinDamage, _primaryMaxDamage);
                        float kBack = Random.Range(_primaryMinKnockback, _primaryMaxKnockback);

                        HitScanTarget newTargetValue = new HitScanTarget(hitInfo.collider.transform.root.gameObject, targetsHit[indexToUpdate].CurrentDamage + dmg,
                            targetsHit[indexToUpdate].CurrentKnockBack + kBack, (targetDir + targetsHit[indexToUpdate].Direction).normalized, hitInfo.point);

                        targetsHit[indexToUpdate] = newTargetValue;
                    }
                    else
                    {
                        float dmg = Random.Range(_primaryMinDamage, _primaryMaxDamage);
                        float kBack = Random.Range(_primaryMinKnockback, _primaryMaxKnockback);

                        targetsHit.Add(new HitScanTarget(hitInfo.collider.transform.root.gameObject, dmg, kBack, targetDir, hitInfo.point));
                    }
                }
            }
            else
            {
                Debug.DrawLine(_fp.position, _fp.position + targetDir.normalized * GunScanData.ScanRange, Color.red, 10f);
            }
          

        }

        if (!_muzzleFlash&& MuzzleFlashPrefab)
        {
            if (ObjectPoolManager.instance)
            {
                _muzzleFlash = ObjectPoolManager.Spawn(MuzzleFlashPrefab, _fp.position, Quaternion.identity);
                Invoke("RemoveMuzzleFlash", MuzzleDuration);
            }
            else
            {
                _muzzleFlash = Instantiate(MuzzleFlashPrefab, _fp.position, Quaternion.identity);
                Invoke("RemoveMuzzleFlash", MuzzleDuration);
            }
            if (targetsHit.Count > 0)
            {
                foreach(HitScanTarget target in targetsHit)
                {
                    Iteam otherTeam = target.Target.GetComponent<Iteam>();

                    if (otherTeam == null)
                    {
                        return;
                    }


                    if (!otherTeam.IsOnTeam(Team.Player))
                    {

      
                     
                        IDamage damage = target.Target.GetComponent<IDamage>();

                        if (damage != null)
                        {
                            damage.OnDamage(target.CurrentDamage, target.Direction,target.CurrentKnockBack,WeaponManager._instance.Getowner(),
                                target.HitPoint);
                        }

                         
                      
                    }
                }
            }
        }
       
    }

    public void RemoveMuzzleFlash()
    {
        if (_muzzleFlash)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(_muzzleFlash);
            }
            else
            {
               Destroy(_muzzleFlash);

            }
            _muzzleFlash = null;
        }
    }
    public void FireSecondaryExplosion()
    {
        RaycastHit hitInfo;
        Vector3 targetpoint = _fp.position + _fp.forward *( (_explosionAttackData._size / 2.0f) - 1f);


        if (Physics.Raycast(_fp.position, _fp.forward, out hitInfo, (_explosionAttackData._size / 2.0f)-1f, GunScanData.BlockingLayers))
        {
            targetpoint = hitInfo.point;
        }
        IExplosion explosion =null;
        if (ObjectPoolManager.instance)
        {
            explosion = ObjectPoolManager.Spawn(_explosionPrefab, targetpoint, Quaternion.identity).GetComponent<IExplosion>();
        }
        else
        {
            explosion = Instantiate(_explosionPrefab, targetpoint, Quaternion.identity).GetComponent<IExplosion>();

        }
    
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
        Vector3 aimDir = EssoUtility.GetCameraLookAtPoint(_fovCam,_aimingDistance,_aimableLayers)- transform.position;

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
