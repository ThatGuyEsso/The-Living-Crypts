using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectShake : MonoBehaviour
{
    //component references and values

    private Transform _anchorTransform; //relative transform
    private Vector3 _anchorPosition;//position to be relative to


    [Header("Vibration Settings Settings")]
    [SerializeField] private float _bobFreq;
    [SerializeField] private float _bobHorizAmp;
    [SerializeField] private float _bobForwardAmp;
    [SerializeField] private float _bobVertAmp;
    [SerializeField] private float _vertOffsetScalar = 2.0f;

    [SerializeField] [Range(0f, 1f)] private float _bobSmoothing = 0.1f;



    //State
    [SerializeField] private bool _shouldBob = false;
    private float _bobTime;
    private Vector3 _targetOffset;

    public void BeginShake(float freq, float horizAmp, float vertAmp, float smoothing, Vector3 anchorPos)
    {
        //set new values
        _bobFreq = freq;
        _bobHorizAmp = horizAmp;
        _bobVertAmp = vertAmp;
        _bobSmoothing = smoothing;

        //begin bobbing
        _shouldBob = true;
        _anchorPosition = anchorPos;
    }
    public void BeginShake(float freq, float horizAmp, float vertAmp, float smoothing, Transform anchor)
    {
        //set new values
        _bobFreq = freq;
        _bobHorizAmp = horizAmp;
        _bobVertAmp = vertAmp;
        _bobSmoothing = smoothing;

        //begin bobbing
        _shouldBob = true;
        _anchorTransform = anchor;
    }

    public void SetAnchor(Transform anchor)
    {
        _anchorTransform = anchor; 
    }
    public void EndShake()
    {
        _shouldBob = false;
    }
    public void BeginShake()
    {

        _shouldBob = true;
    }
    public void EndViewBob()
    {
        //End bobbing
        _shouldBob = false;
    }
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        //bobbing time counter
        if (!_shouldBob)
        {
            _bobTime = 0;
            if (_anchorTransform)
            {
                if (transform.position != _anchorTransform.position)
                {
                    //Lerp to target offset
                    transform.position = Vector3.Lerp(transform.position, _anchorTransform.position, _bobSmoothing);
                    if (Vector3.Distance(transform.position, _anchorTransform.position) <= 0.05f) transform.position = _anchorTransform.position;
                }
            }
            else
            {
                if (transform.position != _anchorPosition)
                {
                    //Lerp to target offset
                    transform.position = Vector3.Lerp(transform.position, _anchorPosition, _bobSmoothing);
                    if (Vector3.Distance(transform.position, _anchorPosition) <= 0.05f) transform.position = _anchorPosition;
                }
  
            }

        }
        else
        {
      
            ///Get new targetoffset
            _targetOffset = transform.position + CalculateNewShakeOffset(_bobTime);

            //Lerp to target offset
            transform.position = Vector2.Lerp(transform.position, _targetOffset, _bobSmoothing);

            //Snap when too close to tell
            if ((transform.position - _targetOffset).magnitude <= 0.001f) transform.position = _targetOffset;   
        }
      

      
  
    }


    private Vector3 CalculateNewShakeOffset(float tBob)
    {
        float horizOffset = 0;
        float vertOffset = 0;
        float forwardOffset = 0;
        Vector2 newOffset = Vector2.zero;

        if (tBob > 0)
        {
            horizOffset = Mathf.Cos(tBob * _bobFreq) * _bobHorizAmp;
            forwardOffset = Mathf.Tan(tBob * _bobFreq) * _bobForwardAmp;
            vertOffset = Mathf.Sin(tBob * _bobFreq * _vertOffsetScalar) * _bobVertAmp;

            //Calculate new offset in XY plane

            newOffset = transform.right * horizOffset + transform.up * vertOffset+ transform.forward*forwardOffset;
        }

        return newOffset;
    }
}
