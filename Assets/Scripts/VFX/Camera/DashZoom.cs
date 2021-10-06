using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DashZoom : MonoBehaviour,IInitialisable
{
    [SerializeField] private bool inDebug;
    [Header("Components")]
    [SerializeField] private FPSDash _dashController;
    [SerializeField] private CinemachineVirtualCamera _targetCamera;
    [Header("Settings")]
    [SerializeField] private float _percentageZoom;
    [SerializeField] private float _zoomInRate;
    [SerializeField] private float _zoomOutRate;


    private float _defaultFOV;
    private float _targetFOV;
    private bool isDashing;
    private bool isInitialised;

    public void Init()
    {
        if (_dashController)
        {
            _dashController.OnBeginDash += OnDashBegin;
            _dashController.OnDashEnd += OnDashEnd;
            _defaultFOV = _targetCamera.m_Lens.FieldOfView;
            isInitialised = true;
        }
    }

    private void Awake()
    {
        if (inDebug) Init();
    }


    private void LateUpdate()
    {
        if (isDashing)
        {
            ZoomInOverTime();
        }else if(!isDashing && _targetCamera.m_Lens.FieldOfView != _defaultFOV)
        {
            ZoomOutOverTime();
        }
    }
    public void OnDashBegin()
    {
        _targetFOV = _defaultFOV - (_defaultFOV * _percentageZoom);
        isDashing = true;
    }
    public void OnDashEnd()
    {
        isDashing = false;
    }
    private void ZoomInOverTime()
    {
        if (_targetCamera.m_Lens.FieldOfView != _targetFOV)
        {
            _targetCamera.m_Lens.FieldOfView = Mathf.Lerp(_targetCamera.m_Lens.FieldOfView, _targetFOV, _zoomInRate*Time.fixedDeltaTime);
            if(Mathf.Abs(_targetCamera.m_Lens.FieldOfView - _targetFOV)<= 0.01f)
            {
                _targetCamera.m_Lens.FieldOfView = _targetFOV;
            }
        }
    }

    private void ZoomOutOverTime()
    {
        if (_targetCamera.m_Lens.FieldOfView != _defaultFOV)
        {
            _targetCamera.m_Lens.FieldOfView = Mathf.Lerp(_targetCamera.m_Lens.FieldOfView, _defaultFOV, _zoomOutRate* Time.fixedDeltaTime);
            if (Mathf.Abs(_targetCamera.m_Lens.FieldOfView - _defaultFOV) <= 0.01f)
            {
                _targetCamera.m_Lens.FieldOfView = _defaultFOV;
            }
        }
    }

    private void OnDisable()
    {
        if (isInitialised&&_dashController)
        {
            _dashController.OnBeginDash -= OnDashBegin;
            _dashController.OnDashEnd -= OnDashEnd;
        }
    }

    private void OnDestroy()
    {
        if (isInitialised && _dashController)
        {
            _dashController.OnBeginDash -= OnDashBegin;
            _dashController.OnDashEnd -= OnDashEnd;
        }
    }
}
