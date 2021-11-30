using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HealthData
{
    public float _maxDefaultHealth;
    public float _minDefaultHealth;
    public float _maxHurtTime;
}
[RequireComponent(typeof(Rigidbody))]

public class Dummy : MonoBehaviour, IDamage
{
    [SerializeField] private HealthData _healthData;
    private Rigidbody _rb;
    private float _maxHealth;
    private float _currentHealth;
    private float _currentHurtTime;
    private bool _canBeHurt;


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _maxHealth = Random.Range(_healthData._minDefaultHealth, _healthData._maxDefaultHealth);
        _currentHealth = _maxHealth;
        _rb = GetComponent<Rigidbody>();
    }
    private void OnKillDummy()
    {
        Destroy(gameObject);
    }
    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker)
    {
        if (_canBeHurt&& attacker != gameObject)
        {
            _canBeHurt = false;
            _currentHurtTime = _healthData._maxHurtTime;
            _currentHealth -= dmg;
            if(_currentHealth <= 0f)
            {
                OnKillDummy();
            }
            else
            {
                if (_rb)
                    _rb.AddForce(kBackDir * kBackMag, ForceMode.Impulse);
            }
         

        }
    }

    private void Update()
    {
        if (!_canBeHurt)
        {
            if(_currentHurtTime <= 0f)
            {
                _canBeHurt = true;
            }
            else
            {
                _currentHurtTime -= Time.deltaTime;
            }
        }
    }
}
