using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [SerializeField] private float RotationRate;
    [SerializeField] private Transform Target;

    private float smoothRot;

    public void SetTarget(Transform newTarget) { Target = newTarget; }
    public void FaceCurrentTarget()
    {
        if (Target != false)
        {
     
            Vector2 toVCursor = Target.position - transform.position;
            float targetAngle = Mathf.Atan2(toVCursor.y, toVCursor.x) * Mathf.Rad2Deg;//get angle to rotate
          
                               //if (targetAngle < 0) targetAngle += 360f;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, RotationRate);//rotate player smoothly to target angle
       
            transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
        }
        
     

    }
    public void SetRotationRate(float rate) { RotationRate = rate; }


}
