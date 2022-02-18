using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;
public class BipedalProcAnim : MonoBehaviour
{
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private FastIKFabric RightLeg, LeftLeg;
    [SerializeField] private Transform RightTarget, LeftTarget;
    [SerializeField] private float MaxStrideDistance;
    [SerializeField] private float AnimRate=3f;
    [SerializeField] private bool InDebug;
    [SerializeField] private float MaxTimeBetweenStrides=0.5f;
    private Transform _currentRightTarget, _currentLeftTarget;

    private bool _useRightLeg;
    private bool _canTakeStep;
    private float _currTimeBetweenStrides;
    private void Awake()
    {
        if (InDebug) Init();
    }

    public void Init()
    {
        if (!RightLeg || !LeftLeg)
        {
            Debug.LogError("Missing leg refs");
        }

        //Set up leg  target( snap to floor)

        //Right leg
        _currentRightTarget = new GameObject("Right Current Target").GetComponent<Transform>();
        RaycastHit hitInfo;
        if(Physics.Raycast(RightLeg.transform.position,Vector3.down,out hitInfo, Mathf.Infinity, GroundLayers))
        {
            _currentRightTarget.position = hitInfo.point;
        }
        else
        {
            _currentRightTarget.position = RightLeg.transform.position;
        }
        RightLeg.Target= _currentRightTarget;
        //Left Leg
        _currentLeftTarget = new GameObject("Left Leg Idle Target").GetComponent<Transform>();
        if (Physics.Raycast(LeftLeg.transform.position, Vector3.down, out hitInfo, Mathf.Infinity, GroundLayers))
        {
            _currentLeftTarget.position = hitInfo.point;
        }
        else
        {
            _currentLeftTarget.position = LeftLeg.transform.position;
        }
        LeftLeg.Target = _currentLeftTarget;
        _useRightLeg = true;
        _currTimeBetweenStrides = MaxTimeBetweenStrides;
    }


    private void Update()
    {
        if (!_canTakeStep)
        {
            if ((_currTimeBetweenStrides <= 0f))
            {
                if (_useRightLeg)
                {
                    if (ShouldTakeStep(RightTarget.position, _currentRightTarget.position))
                    {

                        _canTakeStep = true;
                    }
                }
                else
                {
                    if (ShouldTakeStep(LeftTarget.position, _currentLeftTarget.position))
                    {
                        _canTakeStep = true;
                    }
                }
                _currTimeBetweenStrides = MaxTimeBetweenStrides;
            }
            else
            {
                _currTimeBetweenStrides -= Time.deltaTime;
            }
       
        }
        else
        {
            
         
            if (_useRightLeg)
            {
                _currentRightTarget.position =Vector3.MoveTowards(_currentRightTarget.position, RightTarget.position,Time.deltaTime* AnimRate);
                if (Vector3.Distance(_currentRightTarget.position, RightTarget.position) < 0.05)
                {
                    _currentRightTarget.position = RightTarget.position;
                    _useRightLeg = false;
                    _canTakeStep = false;
                }
            }
            else
            {
                _currentLeftTarget.position = Vector3.MoveTowards(_currentLeftTarget.position, LeftTarget.position, Time.deltaTime * AnimRate);
                if (Vector3.Distance(_currentLeftTarget.position, LeftTarget.position) < 0.05)
                {
                    _currentLeftTarget.position = LeftTarget.position;
                    _useRightLeg = true;
                    _canTakeStep = false;
                }
            }
           

        }
     
      
    }


    public bool ShouldTakeStep(Vector3 targetposition, Vector3 currentPosition)
    {
        return Vector3.Distance(currentPosition, targetposition) >= MaxStrideDistance;
    }

}
