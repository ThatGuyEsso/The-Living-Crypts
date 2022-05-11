using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour, Controls.IMovementActions, IInitialisable, ICharacterComponents
{
    [SerializeField] private bool inDebug;
    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    //Components
    private Rigidbody _rb;
   
    //variables 
    private float _currentMovementSpeed = 0f;
    private float _magnitude = 0f;
    Vector3 _movementDir;
    private Controls _input;

    //States
    private bool _isMoving;
    private bool _isInitialised;
    private bool _isStopping;
    private bool _canMove;
    
    //Events
    public Action OnWalk;
    public Action OnStop;
    public Action<Vector3> OnMoving;
    public Action<Vector3> OnNewMoveDirection;


    private void Awake()
    {
        if (inDebug) Init();
    }
    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
        _input = new Controls();
        _input.Movement.SetCallbacks(this);
        _input.Enable();
        _canMove = true;
        _input.Movement.Move.canceled += _ => BeginStop();
        _isInitialised = true;
        Debug.Log("Move");
    }

    private void Update()
    {
     
        if (_isMoving)
        {
            Vector2 dir = _input.Movement.Move.ReadValue<Vector2>();
            _movementDir =(dir.x * transform.right + transform.forward * dir.y).normalized;
      
        }
    }
    private void FixedUpdate()
    {
        if (!_canMove) return;
        if (_isMoving)
        {

            _currentMovementSpeed = Mathf.Lerp(_currentMovementSpeed, _maxSpeed, Time.fixedDeltaTime * _acceleration);
            if (Mathf.Abs(_maxSpeed - _currentMovementSpeed) <= 0.01f) _currentMovementSpeed = _maxSpeed;

            Move();
        }
        else if (_isStopping)
        {
            _currentMovementSpeed = Mathf.Lerp(_currentMovementSpeed, 0.0f, Time.fixedDeltaTime * _deceleration);


            Vector3 direction = _movementDir * _currentMovementSpeed * Time.fixedDeltaTime;
            _rb.velocity = new Vector3(direction.x, _rb.velocity.y, direction.z);
            if (_currentMovementSpeed <= 0.01f)
            {

                Stop();
            }
        }
    }


    public void BeginStop()
    {
        if (!_canMove) return;
        _magnitude = 0.0f;
        _isStopping = true;
        _isMoving = false;
        _movementDir = Vector3.zero;
        OnStop?.Invoke();
     

    }
    public void BeginStop(float decelRate)
    {
        _magnitude = 0.0f;
        _isStopping = true;
        _isMoving = false;
    
       

    }

    public void Stop()
    {
        _currentMovementSpeed = 0f;
        OnStop?.Invoke();
        _isStopping = false;
        _isMoving = false;
        _currentMovementSpeed = 0.0f;
        _movementDir = Vector3.zero;
        _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        if (context.performed && dir != Vector2.zero && dir.magnitude > 0.2f)
        {
            _magnitude = dir.magnitude;
 
            OnNewMoveDirection?.Invoke(dir);
            OnWalk?.Invoke();
            OnMoving?.Invoke(dir);
            _isMoving = true;

        }

    }
    public void ToggleInputs(bool isOn)
    {
        if (_isInitialised)
        {
            if (isOn)
            {
                _input.Enable();


            }
            else
            {
                _input.Disable();


            }
        }

    }


    public void Move()
    {

        Vector3 velocity = _movementDir * _currentMovementSpeed * _magnitude;

        _rb.velocity = new Vector3(velocity.x,_rb.velocity.y,velocity.z);
    }




    private void OnDisable()
    {
        if (_isInitialised&&_input != null)
        {
            _input.Disable();
            _input.Movement.Move.canceled -= _ => BeginStop();
        }
    }

    private void OnDestroy()
    {
        if (_isInitialised&&_input != null)
        {
            _input.Disable();
            _input.Movement.Move.canceled -= _ => BeginStop();
        }
    }

    public float GetMaxSpeed() { return _maxSpeed; }
    public float GetCurrentSpeed() { return _currentMovementSpeed; }

    public bool IsCharacterMoving()
    {
        if (_isStopping || _isMoving) return true;
        else return false;
    }

    public Vector3 GetMoveDirection() { return _movementDir; }
    public void SetCurrentSpeed(float newSpeed) { _currentMovementSpeed = newSpeed; }
    public void SetIsMoving(bool moving) { _isMoving = moving; }

    public void EnableComponent()
    {
        _input.Enable();
    }

    public void DisableComponent()
    {
        _input.Disable();
        Stop();
    }

    public void ResetComponent()
    {
        _input.Enable();
        _canMove = true;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
        if (_isMoving&&_canMove)
        {
            OnWalk?.Invoke();
        }
        else
        {
            OnStop?.Invoke();
        }
  
    }
}
