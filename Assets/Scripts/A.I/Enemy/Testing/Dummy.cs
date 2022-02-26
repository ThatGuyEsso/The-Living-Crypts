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
    [SerializeField] private GameObject _damageVFX;
    private Rigidbody _rb;
    private MaterialFlash _hurtFlashVFX;
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
        _hurtFlashVFX = GetComponent<MaterialFlash>();
        if (_hurtFlashVFX)
            _hurtFlashVFX.Init();
    }
    private void OnKillDummy()
    {
        Destroy(gameObject);
    }
    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker,Vector3 point)
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
                if (_hurtFlashVFX)
                    _hurtFlashVFX.BeginFlash();
                if (_rb)
                    _rb.AddForce(kBackDir * kBackMag, ForceMode.Impulse);

                IDamagePopUp popUp = Instantiate(_damageVFX, transform.position, Quaternion.identity).GetComponent<IDamagePopUp>();
                if (popUp != null)
                {
                    popUp.InitDamageNumber(_maxHealth, dmg, kBackDir,transform.position);
                }
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
                if(_hurtFlashVFX)
                    _hurtFlashVFX.EndFlash();
            }
            else
            {
                _currentHurtTime -= Time.deltaTime;
            }
        }
    }
}
