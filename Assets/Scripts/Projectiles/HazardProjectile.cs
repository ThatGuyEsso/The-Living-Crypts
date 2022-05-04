using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardProjectile : BaseProjectile
{
    [Header("Hazard Zone Settings")]
    [SerializeField] private GameObject HazardZonePrefab;
    [SerializeField] private float HazardZoneLifeTime;
    [SerializeField] private bool AlwaysSpawnHazardZone;

    [SerializeField] private LayerMask GroundLayers;
    private void SpawnHazardZone()
    {
        if (HazardZonePrefab)
        {
            HazardVolume zone;
            if (ObjectPoolManager.instance)
            {
                 zone = ObjectPoolManager.Spawn(HazardZonePrefab, transform.position + Vector3.up * 3f, Quaternion.identity).GetComponent<HazardVolume>();
            }
            else 
            {
                zone = Instantiate(HazardZonePrefab, transform.position+ Vector3.up*3f, Quaternion.identity).GetComponent<HazardVolume>();
            }


            if (zone)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(zone.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
                {
                    zone.transform.position = new Vector3(zone.transform.position.x, hitInfo.point.y, zone.transform.position.z);
                }


                zone.Init(_projectileData._owner);

            }
        }
    }


    override protected void OnTriggerEnter(Collider other)
    {
        if (_blockingLayers == (_blockingLayers | (1 << other.gameObject.layer)))
        {
            SpawnHazardZone();
            BreakProjectile();


        }
        else if (_damageLayers == (_damageLayers | (1 << other.gameObject.layer)))
        {
            Iteam otherTeam = other.GetComponent<Iteam>();

            if(otherTeam == null)
            {
                otherTeam = other.GetComponentInParent<Iteam>();
                if (otherTeam == null)
                {
                   
                    return;
                }
            }
       
           

            if (other.transform.parent != _projectileData._owner)
            {
                 Iteam ourTeam = _projectileData._owner.GetComponent<Iteam>();

                if (ourTeam ==null || !ourTeam.IsOnTeam(otherTeam.GetTeam()))
                {

                    IProjectile proj = other.gameObject.GetComponent<IProjectile>();
                    if (proj != null)
                    {
                        GameObject otherOwner = proj.GetOwner();
                        if (otherOwner != _projectileData._owner)
                        {
                            if (AlwaysSpawnHazardZone)
                            {
                                SpawnHazardZone();
                            }
                            BreakProjectile();
                        }
                    }
                    else
                    {
                        IDamage damage = other.GetComponent<IDamage>();

                        if (damage != null)
                        {
                             damage = other.GetComponentInParent<IDamage>();
                            if (damage != null)
                            {
                                damage.OnDamage(_projectileData._damage, _rb.velocity.normalized,
                              _projectileData._knockback, _projectileData._owner, other.ClosestPoint(transform.position));
                            }
                          
                        }

                        if (AlwaysSpawnHazardZone)
                        {
                            SpawnHazardZone();
                        }
                        BreakProjectile();
                    }
                }

            }
        }
        else
        {     BreakProjectile();

        }

    }
}
