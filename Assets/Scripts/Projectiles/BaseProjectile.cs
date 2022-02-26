using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private ProjectileData _projectileData;
    [SerializeField] private LayerMask _blockingLayers;
    [SerializeField] private LayerMask _damageLayers;
    private float _currentLifeTime;
    private Rigidbody _rb;


    private void Update()
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
        Destroy(gameObject);
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
        _projectileData = data;

        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = data._direction * data._speed*Time.deltaTime;
        transform.forward= _rb.velocity;
    }


    virtual public void ShootProjectile()
    {
        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = _projectileData._direction * _projectileData._speed * Time.deltaTime;
        transform.forward = _rb.velocity;
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (_blockingLayers == (_blockingLayers | (1 << other.gameObject.layer))){
            BreakProjectile();


        }else if (_damageLayers == (_damageLayers | (1 << other.gameObject.layer))){
            if(other.transform.parent != _projectileData._owner)
            {
                IProjectile proj = other.gameObject.GetComponent<IProjectile>();
                if(proj !=null)
                {
                    GameObject otherOwner = proj.GetOwner();
                    if(otherOwner != _projectileData._owner)
                    {
                        BreakProjectile();
                    }
                }
                else
                {
                    IDamage damage = other.GetComponent<IDamage>();

                    if(damage != null){
                        damage.OnDamage(_projectileData._damage, _rb.velocity.normalized, 
                            _projectileData._knockback, _projectileData._owner,other.ClosestPoint(transform.position));
                    }

                    BreakProjectile();
                }
            }
        }

    }

}
