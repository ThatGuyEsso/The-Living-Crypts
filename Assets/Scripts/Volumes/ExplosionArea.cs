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

}
