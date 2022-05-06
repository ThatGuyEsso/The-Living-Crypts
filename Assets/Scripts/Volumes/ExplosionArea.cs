using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ExplosionData
{
    public GameObject _owner;
    public float _size;
    public float _duration;
    public float _minDamage;
    public float _maxDamage;
    public float _knockBack;
}
[RequireComponent(typeof(SphereCollider))]


public class ExplosionArea : MonoBehaviour, IExplosion
{
    private float _timeLeft;
    private ExplosionData _explosionData;
    [SerializeField] private LayerMask _damageLayers;


    public GameObject GetOwner()
    {
        return _explosionData._owner;
    }

    public void InitExplosion(ExplosionData data)
    {
        _timeLeft = data._duration;
        _explosionData = data;
    }


    public void Update()
    {
        if(_timeLeft > 0f)
        {
            _timeLeft -= Time.deltaTime;
            if(_timeLeft <= 0f)
                ClearExplosion();
        }
    }

    public void ClearExplosion()
    {
        Destroy(gameObject);
    }
    virtual protected void OnTriggerEnter(Collider other)
    {
       
        if (_damageLayers == (_damageLayers | (1 << other.gameObject.layer)))
        {
            if (other.transform.parent != _explosionData._owner)
            {
                IProjectile proj = other.gameObject.GetComponent<IProjectile>();
                if (proj == null)
                {
                    proj = other.gameObject.GetComponentInParent<IProjectile>();
                }

                if (proj != null)
                {
                    proj = other.gameObject.GetComponentInParent<IProjectile>();
                    GameObject otherOwner = proj.GetOwner();
                    if (otherOwner != _explosionData._owner)
                    {
                        proj.BreakProjectile();
                    }
                }
            
                IDamage damage = other.GetComponent<IDamage>();

                if (damage == null)
                {
                     damage = other.GetComponentInParent<IDamage>();
              
                    
                }
                if (damage != null)
                {
                    float dmg = Random.Range(_explosionData._minDamage, _explosionData._maxDamage);
                    Vector3 kBackDir = other.transform.position - transform.position;
                    damage.OnDamage(dmg, kBackDir.normalized, _explosionData._knockBack, _explosionData._owner, other.transform.position);
                }


            }
        }

    }
}
