using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private ProjectileData _projectileData;
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
    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void BreakProjectile()
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


    public void RepelProjectile(Vector3 dir, float force)
    {
        //
    }

    public void ResetProjectile()
    {
        _currentLifeTime = _projectileData._lifeTime;
    }

    public void SetOwner(GameObject owner)
    {
        _projectileData._owner = owner;
    }

    public void SetRotationSpeed(float rotSpeed)
    {
        throw new System.NotImplementedException();
    }

    public void SetSpeed(float speed)
    {
        _projectileData._speed = speed;
    }

    public void SetUpProjectile(ProjectileData data)
    {
        _projectileData = data;
        Init();
    }

    public void ShootProjectile(ProjectileData data)
    {
        _projectileData = data;

        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = data._direction * data._speed*Time.deltaTime;
        transform.forward= _rb.velocity;
    }


    public void ShootProjectile()
    {
        _currentLifeTime = _projectileData._lifeTime;
        _rb.velocity = _projectileData._direction * _projectileData._speed * Time.deltaTime;
        transform.forward = _rb.velocity;
    }
}
