using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public struct PopUpSettings{
    public float _maxBaseSpeed;
    public float _minSpeed;
    public float _maxSpeed;

    public float _maxSize;
  
    public float _minScaleTime;
    public float _maxScaleTime;

    public float _minLifeTime;
    public float _maxBaseLifeTime;
    public float _maxLifeTime;

    public float _acceleration;
    public float _deceleration;

    public float _minScaleBase;
    public float _maxScaleBase;
}
public class DamageNumber : MonoBehaviour, IDamagePopUp
{
    [SerializeField] private PopUpSettings _settings;
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Gradient _damageGradient;
    private GameObject _playerCamera;
    private float _currentLifeTime;
    private float _targetSpeed;
    private float _currentSpeed;
    private Vector3 _targetSize;
    private float _scaleSpeed;
    private Vector3 _moveDirection;
    bool _isActive;
    public void InitDamageNumber(float thisMaxHealth, float damage, Vector3 damageDirection,Vector3 point)
    {
        _text.text = Mathf.FloorToInt(damage).ToString();
        _text.color = _damageGradient.Evaluate(damage / thisMaxHealth);
        float baseScale = Random.Range(_settings._minScaleBase, _settings._maxScaleBase);
        float scaleFactor = baseScale + damage / thisMaxHealth;
        
        _targetSize = Vector3.one * Mathf.Clamp( scaleFactor, _settings._minScaleBase, _settings._maxSize);

        _scaleSpeed = Random.Range(_settings._minScaleTime, _settings._maxScaleTime);

        float lifeTime = Random.Range(_settings._minLifeTime, _settings._maxBaseLifeTime);
        _currentLifeTime = Mathf.Clamp(lifeTime * baseScale, _settings._minLifeTime, _settings._maxLifeTime);

        float speed = Random.Range(_settings._minSpeed, _settings._maxBaseSpeed);

        _targetSpeed =Mathf.Clamp( speed * scaleFactor, _settings._minSpeed, _settings._maxSpeed);

        transform.localScale = Vector3.one *0.1f;
        _currentSpeed = 0f;
        _moveDirection = (new Vector3(damageDirection.x * 0.75f, 0f, damageDirection.z * 0.75f) + Vector3.up +Random.insideUnitSphere).normalized;

        _playerCamera = CamShake.instance.gameObject;
        _isActive = true;
    }


    private void Update()
    {

        if (_isActive)
        {
            if (_currentLifeTime > 0)
            {
                transform.position +=  _moveDirection * _currentSpeed * Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, _targetSize, Time.deltaTime * _scaleSpeed);
                if (Mathf.Abs(transform.localScale.magnitude - _targetSize.magnitude) <= 0.1)
                {
                    transform.localScale = _targetSize;
                }

                _currentSpeed = Mathf.Lerp(_currentSpeed, _targetSpeed, Time.deltaTime * _settings._acceleration);
                if (Mathf.Abs(_currentSpeed - _targetSpeed) <= 0.1)
                {
                    _currentSpeed = _targetSpeed;
                }
                _currentLifeTime -= Time.deltaTime;
            }
            else
            {
                transform.position +=  _moveDirection * _currentSpeed * Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * _scaleSpeed);
                if (transform.localScale.magnitude <= 0.1)
                {
                    transform.localScale = Vector3.zero;
                    Destroy(gameObject);
                }

                _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.deltaTime * _settings._deceleration);
                if (_currentSpeed <= 0.1f)
                {
                    _currentSpeed = 0f;
                }
            }

            if (_playerCamera)
            {
                transform.forward = (_playerCamera.transform.forward);
            }
        }
 


    }


}
