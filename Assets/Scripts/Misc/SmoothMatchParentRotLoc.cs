using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMatchParentRotLoc : MonoBehaviour
{
    private float _speed;
    private bool isActive;

    
    private void Update()
    {
        if (isActive&&transform.parent)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime * _speed);

            transform.localRotation = Quaternion.Slerp(transform.localRotation,Quaternion.identity,_speed*Time.deltaTime);

            if(transform.localRotation ==Quaternion.identity && transform.localPosition == Vector3.zero)
            {
                isActive = false;
            }
        }
    }

    public void ResetChild(float speed)
    {
        _speed = speed;
        isActive = true;
    }

    public void Stop()
    {

        isActive = false;
    }
}
