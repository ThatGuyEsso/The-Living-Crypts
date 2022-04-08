using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class FirstPersonCamera : MonoBehaviour,IInitialisable, Controls.IAimingActions
{
    [SerializeField] private bool _inDebug;

    [Header("Components to Effect")]
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _characterTransform;

    [Header("Camera Settings")]
     [Range(1f,60f)]
    [SerializeField] private float _sensitivity;
    [SerializeField] private ViewBob _viewBob;
    //State variables
    private bool _isInitialised;
    private bool _isFollowing;

    //maths variables
    private float _xRot =0f;
    private float _yRot=0f;
    private float _xMove = 0f;
    private float _yMove = 0f;
    //Class instances
    private Controls _input;
    private Vector3 _currentOffset;
    private Rigidbody _playerRB;

    private PlayerBehaviour _player;
    private void Awake()
    {
        if (_inDebug)
        {
            Init();
        
        }
    }
    public void Init()
    {
        if (_targetPosition)
        {
            _isFollowing = true;

            _player =_characterTransform.GetComponent<PlayerBehaviour>();
            if (_player)
            {
                _player.OnPlayerDied += OnPlayerKilled;
            }
        }

        if (_viewBob)
        {
            _viewBob.Init();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _input = new Controls();
        _input.Aiming.SetCallbacks(this);
        _input.Enable();
        _playerRB = _characterTransform.GetComponent<Rigidbody>();
        _isInitialised = true;
        //_xRot = transform.localRotation..x;
        //_yRot = transform.localRotation.eulerAngles.y;
    }



    private void Update()
    {

        float mousePosX = _sensitivity * Time.deltaTime * _xMove;
        float mousePosY = _sensitivity * Time.deltaTime * _yMove;

        _xRot -= mousePosY;
        _xRot = Mathf.Clamp(_xRot, -45f, 45f);

        Vector3 rot = transform.localRotation.eulerAngles;
        _yRot = rot.y + mousePosX;
        transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);
        if (_characterTransform)
            _characterTransform.localRotation = Quaternion.Euler(0f, _yRot, 0f);



    }

    private void LateUpdate()
    {
        if (_isFollowing) transform.position = _targetPosition.position + _currentOffset;
    }

    public void OnAimX(InputAction.CallbackContext context)
    {
        float dirX = context.ReadValue<float>();
       
            if (Mathf.Abs(dirX) < 0.2f) dirX = 0f;
            _xMove = dirX;
    
     
    }
    public void OnAimY(InputAction.CallbackContext context)
    {
        float dirY = context.ReadValue<float>();
   
            if (Mathf.Abs(dirY) < 0.2f) dirY = 0f;
            _yMove = dirY;

    }

    public void SetCurrentOffset(Vector3 offset) { _currentOffset = offset; }
    private void OnDisable()
    {
        if (_isInitialised)
        {
            _input.Disable();
    
        }

        if (_player)
        {
            _player.OnPlayerDied -= OnPlayerKilled;
        }
    }

    private void OnDestroy()
    {
        if (_isInitialised)
        {
            _input.Disable();
         
        }

        if (_player)
        {
            _player.OnPlayerDied -= OnPlayerKilled;
        }
    }

    public void OnPlayerKilled()
    {
        if (_input != null)
        {
            _input.Disable();
        }

    }
}
