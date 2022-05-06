using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSDash : MonoBehaviour, IInitialisable, Controls.IDashActions, ICharacterComponents
{
    [SerializeField] private bool _inDebug;
    [Header("Components")]
    [SerializeField] private FPSMovement _fpsMove;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private EssoGravity _gravity;

    [Header("Dash Settings")]
    [SerializeField] private float _dashCoolDownTime;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxSpeed;
    [Header("Dash VFX")]
    [SerializeField] private GameObject DashVFX;
    [SerializeField] private Transform FootPoint;
    private bool _canDash;
    private bool _isDashing;
    private bool _isStopping;
    private float _endDashSpeed;
    private Vector3 _dashDirection;
    private float _currentSpeed;
    private float _initGravScalar;
    Controls _input;
    //Event
    public System.Action OnDashEnd;
    public System.Action OnBeginDash;
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
        if (_gravity) _initGravScalar = _gravity.GravityScale;
        Debug.Log("Dash");
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


            Vector3 direction = _dashDirection * _currentSpeed * Time.fixedDeltaTime;
            if (_fpsMove.GetMoveDirection() == Vector3.zero)
                _rb.velocity = new Vector3(direction.x, _rb.velocity.y, direction.z); ;

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

    public void SpawnDashVFX()
    {
        if (DashVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(DashVFX, FootPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(DashVFX, FootPoint.position, Quaternion.identity);
            }
        }
    }
    public void DoDash()
    {

        if (_fpsMove.GetCurrentSpeed() > 0f) _endDashSpeed = _fpsMove.GetMaxSpeed();
        else _endDashSpeed = 0f;
        _fpsMove.SetCanMove(false);

        if (_fpsMove.GetMoveDirection() != Vector3.zero) _dashDirection = _fpsMove.GetMoveDirection().normalized;
        else _dashDirection = transform.forward.normalized;
        if (_gravity)
            _gravity.GravityScale = 0f;
        _isDashing = true;
        OnBeginDash?.Invoke();
        StartCoroutine(dashTimer());
        SpawnDashVFX();

    }
    public void Dashing()
    {
        Vector3 velocity = _dashDirection * _currentSpeed;
        _rb.velocity = new Vector3(velocity.x, _rb.velocity.y, velocity.z);
    }

    public void StopDash()
    {
        _isStopping = false;
        OnDashEnd?.Invoke();
        _dashDirection = Vector3.zero;
        _fpsMove.SetCanMove(true);
        if (!_fpsMove.IsCharacterMoving())
            _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);

    }
    public void EndDash()
    {
        if (_gravity)
            _gravity.GravityScale = _initGravScalar;
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

    public void EnableComponent()
    {
        StopAllCoroutines();
        ResetDash();
        if (_input != null)
        {
            _input.Enable();
        }
    }


    public void DisableComponent()
    {
        EndDash();
        StopAllCoroutines();
        _canDash = false;

        if (_input != null)
        {
            _input.Disable();
        }
    }

    public void ResetComponent()
    {
        EnableComponent();
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_input != null)
        {
            _input.Disable();
        }
    }
}
