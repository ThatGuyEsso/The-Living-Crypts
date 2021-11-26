using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ProjectileData
{
    public float _damage;
    public Vector3 _direction;
    public float _speed;
    public float _lifeTime;
    public GameObject _owner;

    public ProjectileData(float dmg, Vector3 direction, float speed, float lifeTime, GameObject owner)
    {
        _damage = dmg;
        _direction = direction;
        _speed = speed;
        _lifeTime = lifeTime;
       
        _owner = owner;
    }
}
public interface IProjectile 
{
    void SetUpProjectile(ProjectileData data);
    void SetOwner(GameObject owner);
    void ShootProjectile(ProjectileData data);
    void ShootProjectile();
    void SetRotationSpeed(float rotSpeed);

    void ResetProjectile();


 
    ProjectileData GetProjectileData();
    GameObject GetOwner();
    GameObject GetSelf();
    void RepelProjectile(Vector3 dir, float force);

    void BreakProjectile();
    void SetSpeed(float speed);


}
