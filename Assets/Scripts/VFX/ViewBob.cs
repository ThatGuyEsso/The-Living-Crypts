using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ViewBob : MonoBehaviour, IInitialisable
{
    [SerializeField] private bool inDebug;
    //component references and values
    private CinemachineVirtualCamera _vCam;//actual camera position
    private CinemachineTransposer _body;//Cinemachine trasposer body ( where values I need to update are stored)
    private Vector3 _initialOffset;//orignal offset
    [SerializeField] private FPSMovement moveController;

    [Header("View Bob Settings")]
    [SerializeField] private float _bobFreq;
    [SerializeField] private float _bobHorizAmp;
    [SerializeField] private float _bobVertAmp;
    [SerializeField] private float _vertOffsetScalar = 2.0f;
    [SerializeField] [Range(0f, 1f)] private float _bobSmoothing = 0.1f;


    //State
    private bool _shouldBob = false;
    private bool _isInitialised;
    private float _bobTime;
    private Vector3 _targetOffset;
    private void Awake()
    {
        if (inDebug) Init();
    }
    public void BeginViewBob()
    {

        //begin bobbing
        _shouldBob = true;
    }
    public void BeginViewBob(float freq, float horizAmp, float vertAmp, float smoothing)
    {
        //set new values
        _bobFreq = freq;
        _bobHorizAmp = horizAmp;
        _bobVertAmp = vertAmp;
        _bobSmoothing = smoothing;

        //begin bobbing
        _shouldBob = true;
    }
    public void EndViewBob()
    {
        //End bobbing
        _shouldBob = false;
    }
    private void Update()
    {
        //bobbing time counter
        if (_shouldBob)
        {

            _bobTime += Time.deltaTime;
            ///Get new targetoffset
            _targetOffset = _initialOffset + CalculateNewBobOffset(_bobTime);


            //Lerp to target offset
            _body.m_FollowOffset = Vector3.Lerp(_body.m_FollowOffset, _targetOffset, _bobSmoothing);

            //Snap when too close to tell
            if ((_body.m_FollowOffset - _targetOffset).magnitude <= 0.001f) _body.m_FollowOffset = _targetOffset;
        }

     
    }


    private Vector3 CalculateNewBobOffset(float tBob)
    {
        float horizOffset = 0;
        float vertOffset = 0;
        Vector3 newOffset = Vector3.zero;

        if (tBob > 0)
        {
            horizOffset = Mathf.Cos(tBob * _bobFreq) * _bobHorizAmp;
            vertOffset = Mathf.Sin(tBob * _bobFreq * _vertOffsetScalar) * _bobVertAmp;

            //Calculate new offset in XY plane

            newOffset = transform.right * horizOffset + transform.up * vertOffset;
        }
        return newOffset;
    }

    public void Init()
    {
        //caching component refs
        _vCam = gameObject.GetComponent<CinemachineVirtualCamera>();

        //Setting initial offsets
        _body = _vCam.GetCinemachineComponent<CinemachineTransposer>();

        _initialOffset = _body.m_FollowOffset;

        if (moveController)
        {
            moveController.OnWalk += BeginViewBob;
            moveController.OnStop += EndViewBob;
            _isInitialised = true;
        }
    }

    private void OnDisable()
    {
        if (_isInitialised && moveController)
        {
            moveController.OnWalk -= BeginViewBob;
            moveController.OnStop -= EndViewBob;
        }

    }

    private void OnDestroy()
    {
        if (_isInitialised && moveController)
        {
            moveController.OnWalk -= BeginViewBob;
            moveController.OnStop -= EndViewBob;
        }
    }
}
