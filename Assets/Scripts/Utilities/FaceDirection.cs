using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour
{
    [SerializeField] private float RotationRate;


    private float smoothRot;

  
    public void SmoothRotDirection(Vector3 direction)
    {

        float targetAngle = EssoUtility.GetAngleFromVector((direction.normalized));
        /// turn offset -Due to converting between forward vector and up vector
        if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothRot, RotationRate);//rotate player smoothly to target angle
        if (Mathf.Abs(targetAngle - angle) <= 0.1) angle = targetAngle;//snap when close
        transform.rotation = Quaternion.Euler(0f, angle, 0f);//update angle




    }
    public void ConstantRotInDirection(Vector3 direction)
    {

        float targetAngle = EssoUtility.GetAngleFromVector((direction.normalized));
        /// turn offset -Due to converting between forward vector and up vector
        if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.MoveTowards(transform.eulerAngles.y, targetAngle,  Time.deltaTime* RotationRate);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, angle, 0f);//update angle




    }
}
