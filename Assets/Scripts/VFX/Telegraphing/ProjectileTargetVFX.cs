using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTargetVFX : MonoBehaviour
{
    BaseProjectile _trackedProjectile;
    public void Init(BaseProjectile projectile)
    {
        if (projectile)
        {
            _trackedProjectile = projectile;

            _trackedProjectile.OnProjectileDestroyed += OnProjectileDestroyed;
        }
        else
        {
            RemoveVFX();
        }
    }
  
    public void OnProjectileDestroyed()
    {
        if (_trackedProjectile)
        {
            _trackedProjectile.OnProjectileDestroyed -= OnProjectileDestroyed;
        }
        RemoveVFX();
    }

    public void RemoveVFX()
    {
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }
}
