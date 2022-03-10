using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] protected ProjectileData _projectileData;
    [SerializeField] protected LayerMask _blockingLayers;
    [SerializeField] protected LayerMask _damageLayers;
    private float _currentLifeTime;
    protected Rigidbody _rb;

    public System.Action OnProjectileDestroyed;
    virtual protected void Update()
    {
        if (_currentLifeTime > 0)
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
    virtual public void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }
    virtual public void BreakProjectile()
    {
        OnProjectileDestroyed?.Invoke();
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {

             Destroy(gameObject);
        }
    }

    public GameObject GetOwner()
    {
        return _projectileData._owner;
    }

    public ProjectileData GetProjectileData()
    {
        return _projectileData;
    }

    public GameObject GetSelf()
    {
        return gameObject;
    }


    virtual public void RepelProjectile(Vector3 dir, float force)
    {
        //
    }

    virtual public void ResetProjectile()
    {
        _currentLifeTime = _projectileData._lifeTime;
    }

    virtual public void SetOwner(GameObject owner)
    {
        _projectileData._owner = owner;
    }

    virtual public void SetRotationSpeed(float rotSpeed)
    {
        throw new System.NotImplementedException();
    }

    virtual public void SetSpeed(float speed)
    {
        _projectileData._speed = speed;
    }

    virtual public void SetUpProjectile(ProjectileData data)
    {
        _projectileData = data;
        Init();
    }

    virtual public void ShootProjectile(ProjectileData data)
    {
        if (!_rb)
        {
            Init();
        }
        _projectileData = data;

        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = data._direction * data._speed;
        transform.forward= _rb.velocity;
    }


    virtual public void ShootProjectile()
    {
        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = _projectileData._direction * _projectileData._speed;
        transform.forward = _rb.velocity;
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (_blockingLayers == (_blockingLayers | (1 << other.gameObject.layer))){
            BreakProjectile();


        }else if (_damageLayers == (_damageLayers | (1 << other.gameObject.layer))){
            Iteam otherTeam = other.GetComponent<Iteam>();

            if (otherTeam == null)
            {
                return;
            }
            IProjectile proj = other.gameObject.GetComponent<IProjectile>();
            if(other.transform.parent != _projectileData._owner)
            {
                Iteam ourTeam = _projectileData._owner.GetComponent<Iteam>();

                if (ourTeam == null || !ourTeam.IsOnTeam(otherTeam.GetTeam()))
                {

                    if (proj != null)
                    {
                        GameObject otherOwner = proj.GetOwner();
                        if (otherOwner != _projectileData._owner)
                        {
                            BreakProjectile();
                        }
                    }
                    else
                    {
                        IDamage damage = other.GetComponent<IDamage>();

                        if (damage != null)
                        {
                            damage.OnDamage(_projectileData._damage, _rb.velocity.normalized,
                                _projectileData._knockback, _projectileData._owner, other.ClosestPoint(transform.position));
                        }

                        BreakProjectile();
                    }
                }
            }
        }

    }

}
