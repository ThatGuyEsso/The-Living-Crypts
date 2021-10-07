using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class StrafeTilt : MonoBehaviour,IInitialisable
{
    [SerializeField] private bool _inDebug;
    [Header("Componenets")]
    [SerializeField] private CinemachineVirtualCamera _targetCamera;
    [SerializeField] private FPSMovement _movementController;
    [Header("Settings")]
    [SerializeField] private float _maxTilt;
    [SerializeField] private float _tiltRate;
    [SerializeField] private float _returnToNormalRate;


    private float _defaultTilt;
    private bool _isInitialised;
    private bool _isTitling;
    private bool _isTitlingRight;
    private void Awake()
    {
        if (_inDebug) Init();
    }

    private void Update()
    {
        if (_isTitling)
        {

            if (_isTitlingRight) TiltRight();
            else TiltLeft();
        }
        else if(!_isTitling&&_targetCamera.m_Lens.Dutch != _defaultTilt)
        {
            ReturnToNormal();
        }

    }

    public void TiltRight()
    {
        if (_targetCamera.m_Lens.Dutch !=- _maxTilt)
        {
        
            _targetCamera.m_Lens.Dutch = Mathf.Lerp(_targetCamera.m_Lens.Dutch, -_maxTilt, _tiltRate * Time.fixedDeltaTime);
            if (Mathf.Abs(Mathf.Abs(_targetCamera.m_Lens.Dutch) - Mathf.Abs(_maxTilt)) <= 0.01f)
            {
                _targetCamera.m_Lens.Dutch = -_maxTilt;
            }

         
        }
    }

    public void TiltLeft()
    {
        if (_targetCamera.m_Lens.Dutch != _maxTilt)
        {
      
            _targetCamera.m_Lens.Dutch = Mathf.Lerp(_targetCamera.m_Lens.Dutch, _maxTilt, _tiltRate * Time.fixedDeltaTime);
            if (Mathf.Abs(Mathf.Abs(_targetCamera.m_Lens.Dutch) - Mathf.Abs(_maxTilt)) <= 0.01f)
            {
                _targetCamera.m_Lens.Dutch = _maxTilt;
            }
        }
    }
    private void ReturnToNormal()
    {
        if (_targetCamera.m_Lens.Dutch != _defaultTilt)
        {
        
            _targetCamera.m_Lens.Dutch = Mathf.Lerp(_targetCamera.m_Lens.Dutch, _defaultTilt, _returnToNormalRate * Time.fixedDeltaTime);
            if (Mathf.Abs(_targetCamera.m_Lens.Dutch) - Mathf.Abs(_defaultTilt) <= 0.01f)
            {
                _targetCamera.m_Lens.Dutch = _defaultTilt;
            }


          
        }
    }
    public void OnStrafe(Vector3 moveDir)
    {
        if(moveDir.x != 0)
        {
            if (moveDir.x < 0) _isTitlingRight = false;
            else if(moveDir.x>0)_isTitlingRight = true;
            _isTitling = true;
        }
    }
    public void OnStopped( )
    {
        _isTitling = false;
    }
    public void Init()
    {
        if (_movementController)
        {
            _movementController.OnNewMoveDirection += OnStrafe;
            _movementController.OnStop += OnStopped;
            if (_targetCamera) _defaultTilt = _targetCamera.m_Lens.Dutch;
            _isInitialised = true;
        }
    }


    private void OnDisable()
    {
        if (_isInitialised && _movementController)
        {
            _movementController.OnNewMoveDirection -= OnStrafe;
            _movementController.OnStop -= OnStopped;
        }
    }
    private void OnDestroy()
    {
        if (_isInitialised && _movementController)
        {
            _movementController.OnNewMoveDirection -= OnStrafe;
            _movementController.OnStop -= OnStopped;
        }
    }
}
