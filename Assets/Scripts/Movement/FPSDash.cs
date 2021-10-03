using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSDash : MonoBehaviour,IInitialisable,Controls.IDashActions
{
    [SerializeField] private bool _inDebug;
    [Header("Components")]
    [SerializeField] private FPSMovement _fpsMove;
    [SerializeField] private Rigidbody _rb;


    [Header("Dash Settings")]
    [SerializeField] private float _dashCoolDownTime;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxSpeed;
    private bool _canDash;
    private bool _isDashing;
    private bool _isStopping;
    private float _endDashSpeed;
    private Vector3 _dashDirection;
    private float _currentSpeed;
    Controls _input;
    //Event
    public System.Action OnDashEnd;
    private void Awake()
    {
        if (_inDebug) Init();
    }

    public void Init()
    {
        _canDash = true;
        _input = new Controls();
        _input.Dash.SetCallbacks(this);
        _input.Enable();
      
    }

    private void FixedUpdate()
    {

        if (_isDashing)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _maxSpeed, Time.fixedDeltaTime * _acceleration);
            if (Mathf.Abs(_maxSpeed - _currentSpeed) <= 0.01f) _currentSpeed = _maxSpeed;

            Dashing();
        }
        else if (_isStopping)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0.0f, Time.fixedDeltaTime * _deceleration);


            Vector2 direction = _dashDirection * _currentSpeed;
            if (_fpsMove.GetMoveDirection() == Vector3.zero)
                _rb.velocity = direction;

            if (_currentSpeed <= 0.01f)
            {

                StopDash();
            }
        }
    }


    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && _canDash)
        {
            _canDash = false;
            DoDash();
        }
    }

    public void DoDash() {

        if (_fpsMove.GetCurrentSpeed() > 0f) _endDashSpeed = _fpsMove.GetMaxSpeed();
        else _endDashSpeed = 0f;
        _fpsMove.SetCanMove(false);

        if (_fpsMove.GetMoveDirection() != Vector3.zero) _dashDirection = _fpsMove.GetMoveDirection().normalized;
        else _dashDirection = transform.forward;
        _isDashing = true;
        StartCoroutine(dashTimer());

    }
    public void Dashing()
    {
        Vector3 velocity = _dashDirection * _currentSpeed;
        _rb.velocity = velocity;
    }

    public void StopDash()
    {
        _isStopping = false;
        OnDashEnd?.Invoke();
        _fpsMove.SetCanMove(true);
        if (!_fpsMove.IsCharacterMoving())
            _rb.velocity = Vector3.zero;

    }
    public void EndDash()
    {

        _fpsMove.SetCurrentSpeed(_endDashSpeed);
        _isDashing = false;
        _isStopping = true;
        StartCoroutine(WaitToRefreshDash());

    }

    public void ResetDash()
    {

        _canDash = true;

    }

    private IEnumerator dashTimer()
    {
        yield return new WaitForSeconds(_dashTime);
        EndDash();
    }

    private IEnumerator WaitToRefreshDash()
    {
        yield return new WaitForSeconds(_dashCoolDownTime);
        ResetDash();
    }

}
