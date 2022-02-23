using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody))]
public class WalkMovement : MonoBehaviour
{
    [SerializeField] private bool inDebug;
    [Header("Movement Settings")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float StoppingDistance;
    //Components
    private Rigidbody _rb;

    //variables 
    private float _currentMovementSpeed = 0f;
    Vector3 _movementDir;
    Vector3 _targetPoint;
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
        _canMove = true;
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

    public void MoveToPoint(Vector3 point)
    {
        _targetPoint = point;
        Vector3 dir = (point - transform.position).normalized;
        _movementDir = new Vector3(dir.x, 0.0f, dir.z);
        _isMoving = true;
    }

    public void BeginStop()
    {
        if (!_canMove)
        {
            Stop();
            return;
        }
     
        _isStopping = true;
        _isMoving = false;
        _movementDir = Vector3.zero;
        OnStop?.Invoke();


    }
    public void BeginStop(float decelRate)
    {
    
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

    public void Move()
    {

        Vector3 velocity = _movementDir * _currentMovementSpeed;

        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
    }

    public void StopAtPoint()
    {
        if (Vector3.Distance(transform.position, _targetPoint) <= StoppingDistance)
        {
            BeginStop();
        }
    }

}
