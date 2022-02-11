using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour, IDamage
{
    [SerializeField] private bool _inDebug;
    [SerializeField] private HealthData _healthData;

  
    private float _maxHealth;
    private float _currentHealth;
    private float _currentHurtTime;
    private bool _canBeHurt;
    private bool _isAlive;

    public System.Action<float ,float , float , Vector3 > OnDamageReceived;
    public System.Action OnHurt;
    public System.Action OnNotHurt;
    public System.Action OnDie;


   
    public void Init()
    {
        _maxHealth = Random.Range(_healthData._minDefaultHealth, _healthData._maxDefaultHealth);
        _currentHealth = _maxHealth;
        _canBeHurt = true;
        _isAlive = true;
    }
    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker)
    {
        if (_isAlive&&_canBeHurt && attacker != gameObject)
        {
            _canBeHurt = false;
            _currentHurtTime = _healthData._maxHurtTime;
            _currentHealth -= dmg;
            if (_currentHealth <= 0f)
            {
                _isAlive = false;
                OnDie?.Invoke();
            }
            else
            {

                OnDamageReceived?.Invoke(_maxHealth,dmg,kBackMag,kBackDir);
                OnHurt?.Invoke();
            }


        }
    }

    private void Update()
    {
        if (!_canBeHurt)
        {
            if (_currentHurtTime <= 0f)
            {
                _canBeHurt = true;
                OnNotHurt?.Invoke();
            }
            else
            {
                _currentHurtTime -= Time.deltaTime;
            }
        }
    }

    public bool IsHurt { get { return !_canBeHurt; } }

    public HealthData HealthData { set { _healthData = value; } get { return _healthData; } }
    public float CurrentHealth { set { _currentHealth = value; } get { return _currentHealth; } }
    public float GetMaxHealth() { return _maxHealth; }
    public bool IsAlive { set { _isAlive = value; } get { return _isAlive; } }


}
