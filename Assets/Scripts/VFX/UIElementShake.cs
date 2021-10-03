using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementShake : MonoBehaviour
{
    //component references and values

    private Vector2 _initialOffset;//orignal offset
    [SerializeField] private RectTransform rt;

    [Header("Vibration Settings Settings")]
    [SerializeField] private float _bobFreq;
    [SerializeField] private float _bobHorizAmp;
    [SerializeField] private float _bobVertAmp;
    [SerializeField] private float _vertOffsetScalar = 2.0f;

    [SerializeField] private float _vibrateTime;
    private float _currentVibrateTime;
    [SerializeField] [Range(0f, 1f)] private float _bobSmoothing = 0.1f;
    [SerializeField] private ResizeToFitText displayRefit;


    //State
    private bool _shouldBob = false;
    private float _bobTime;
    private Vector2 _targetOffset;
    private void Awake()
    {
        //caching component refs
    


        _currentVibrateTime = _vibrateTime;
        _initialOffset = rt.position;
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

    public void BeginViewBob()
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
            if ((Vector2)rt.position != _initialOffset)
            {
                //Lerp to target offset
                rt.position = Vector2.Lerp(rt.position, _initialOffset, _bobSmoothing);
                if (Vector2.Distance(rt.position, _initialOffset) <= 0.05f) rt.position = _initialOffset;
            }
        }
        else
        {
            _bobTime += Time.deltaTime;
            if (_currentVibrateTime <= 0)
            {
                _shouldBob = false;
                _currentVibrateTime = _vibrateTime;
            }
            else
            {
                _currentVibrateTime -= Time.deltaTime;
            }
        }

        ///Get new targetoffset
        _targetOffset = rt.position + CalculateNewBobOffset(_bobTime);


        //Lerp to target offset
        rt.position = Vector2.Lerp(rt.position, _targetOffset, _bobSmoothing);

        //Snap when too close to tell
        if (((Vector2)rt.position - _targetOffset).magnitude <= 0.001f) rt.position = _targetOffset;
    }


    private Vector3 CalculateNewBobOffset(float tBob)
    {
        float horizOffset = 0;
        float vertOffset = 0;
        Vector2 newOffset = Vector2.zero;

        if (tBob > 0)
        {
            horizOffset = Mathf.Cos(tBob * _bobFreq) * _bobHorizAmp;
            vertOffset = Mathf.Sin(tBob * _bobFreq * _vertOffsetScalar) * _bobVertAmp;

            //Calculate new offset in XY plane

            newOffset = transform.right * horizOffset + transform.up * vertOffset;
        }

        return newOffset;
    }
}
