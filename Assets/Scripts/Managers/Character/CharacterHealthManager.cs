using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthManager : MonoBehaviour, IDamage,IInitialisable
{
    [SerializeField] private bool _inDebug;
    [SerializeField] private HealthData _healthData;

  
    private float _maxHealth;
    private float _currentHealth;
    private float _currentHurtTime;
    private bool _canBeHurt;
    private bool _isAlive;

    public System.Action<float ,float , float , Vector3,Vector3 > OnDamageReceived;
    public System.Action<float> OnHealthUpdated;
    public System.Action OnHurt;
    public System.Action OnNotHurt;
    public System.Action OnDie;


    private void Awake()
    {
        if (_inDebug)
        {
            Init();
        }
    }
    public void Init()
    {
       

        _maxHealth = Random.Range(_healthData._minDefaultHealth, _healthData._maxDefaultHealth);
        _currentHealth = _maxHealth;
        _canBeHurt = true;
        _isAlive = true;
        
       
       
     
    }
    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker, Vector3 point)
    {
        if (_isAlive&&_canBeHurt && attacker != gameObject)
        {
            _canBeHurt = false;
            _currentHurtTime = _healthData._maxHurtTime;
            _currentHealth -= dmg;
            if (_currentHealth <= 0f)
            {
                _isAlive = false;
                OnHealthUpdated?.Invoke(CurrentHealth);
                OnDie?.Invoke();
                Debug.Log("Player Died");
            }
            else
            {

                OnDamageReceived?.Invoke(_maxHealth, dmg,kBackMag,kBackDir,point);
                OnHealthUpdated?.Invoke(CurrentHealth);
                Debug.Log("Player Hurt: " + _currentHealth);
                OnHurt?.Invoke();
             
            }


        }
    }
    public void KillCharacter()
    {
        _currentHealth = 0;
        OnHealthUpdated?.Invoke(CurrentHealth);
        OnDie?.Invoke();
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
    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _canBeHurt = true;
        _isAlive = true;
        OnHealthUpdated?.Invoke(CurrentHealth);
    }

    public bool IsHurt { get { return !_canBeHurt; } }

    public HealthData HealthData { set { _healthData = value; } get { return _healthData; } }
    public float CurrentHealth { set { _currentHealth = value; } get { return _currentHealth; } }
    public float GetMaxHealth() { return _maxHealth; }
    public bool IsAlive { set { _isAlive = value; } get { return _isAlive; } }


}
