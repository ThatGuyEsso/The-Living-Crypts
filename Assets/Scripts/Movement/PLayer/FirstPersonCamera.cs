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
     [Range(1f,25f)]
    [SerializeField] private float _sensitivity;
   
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
    private void Awake()
    {
        if (_inDebug)
        {
            Init();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Init()
    {
        if (_targetPosition) _isFollowing = true;

        _input = new Controls();
        _input.Aiming.SetCallbacks(this);
        _input.Enable();

        _isInitialised = true;
        //_xRot = transform.localRotation..x;
        //_yRot = transform.localRotation.eulerAngles.y;
    }


    private void Update()
    {
        float mousePosX = _sensitivity * Time.fixedDeltaTime * _xMove;
        float mousePosY = _sensitivity * Time.fixedDeltaTime * _yMove;

        _xRot -= mousePosY;
        _xRot = Mathf.Clamp(_xRot, -45f, 45f);

        Vector3 rot = transform.localRotation.eulerAngles;
        _yRot = rot.y + mousePosX;

        transform.localRotation = Quaternion.Euler(_xRot, _yRot, 0f);
        if(_characterTransform)
            _characterTransform.localRotation = Quaternion.Euler(0f, _yRot, 0f);


    }

    private void LateUpdate()
    {

        if (_isFollowing) transform.position = _targetPosition.position;
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

    private void OnDisable()
    {
        if (_isInitialised)
        {
            _input.Disable();
    
        }
    }

    private void OnDestroy()
    {
        if (_isInitialised)
        {
            _input.Disable();
         
        }
    }
}
