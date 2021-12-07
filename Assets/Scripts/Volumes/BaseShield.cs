using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ShieldSettings
{
    public GameObject _owner;
    public float _lifeTime;
    public float _minDamage;
    public float _maxDamage;
    public float _minKnockBack;
    public float _maxKnockBack;
    public LayerMask _targetLayers;
    public Vector3 _followOffset;
    public ShieldSettings(GameObject owner, float lifeTime, float minDmg,
        float maxDmg, float minKnockBack, float maxKnockBack,LayerMask targetLayers,Vector3 offset )
    {
        _owner = owner;
        _lifeTime = lifeTime;
        _minDamage = minDmg;
        _maxDamage = maxDmg;
        _minKnockBack = minKnockBack;
        _maxKnockBack = maxKnockBack;
        _targetLayers = targetLayers;
        _followOffset = offset;
    }

}
[RequireComponent(typeof(SphereCollider))]
public abstract class BaseShield : MonoBehaviour
{
    protected bool _isInitialised;
    protected Transform _followTarget;
    protected float _timeLeft;
    protected bool _canExpire;
    [SerializeField] protected ShieldSettings _settings;
    public virtual void Init(ShieldSettings settings,Transform followTarget)
    {
        GetComponent<SphereCollider>().isTrigger = true;
        _settings = settings;
        _followTarget = followTarget;
        if (settings._lifeTime > 0)
        {
            _timeLeft = settings._lifeTime;
            _canExpire = true;
        }
        _isInitialised = true;
    }


    protected abstract void DoShieldInteraction(GameObject other);

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!_isInitialised) return;
        if (_settings._targetLayers == (_settings._targetLayers | (1 << other.gameObject.layer)))
        {
            if (other.transform.parent != _settings._owner)
            {
                DoShieldInteraction(other.gameObject);
            }
        }
    }

    protected virtual void Update()
    {
        if (_canExpire)
        {
            if (_timeLeft <= 0)
            {
                DestroyShield();
            }
            else
            {
                _timeLeft -= Time.deltaTime;
            }
        }

      
    }

    private void LateUpdate()
    {
        if (!_isInitialised) return;
        if (_followTarget)
        {
            transform.position = _followTarget.position + _settings._followOffset;
        }
    }
    protected virtual void DestroyShield()
    {
        Destroy(gameObject);
    }
}

