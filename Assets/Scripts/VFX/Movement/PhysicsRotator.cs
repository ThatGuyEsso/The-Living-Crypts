using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PhysicsRotator : Rotator
{
    private Rigidbody _rb;

    protected override void Awake()
    {
        base.Awake();

        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }

        
    }

    protected override void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        if (!IsSpinning)
        {
            return;
        }
        if (!_rb)
        {
            return;
        }
        if (ShouldAccelerate)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, RotationtionSpeed, Time.deltaTime * Acceleration);
            if (Mathf.Abs(CurrentSpeed - RotationtionSpeed) < 0.01f)
            {
                CurrentSpeed = RotationtionSpeed;
            }

            

        }

        _rb.angularVelocity = CurrentSpeed * _rotationDirection * Time.fixedDeltaTime;
    }
}
