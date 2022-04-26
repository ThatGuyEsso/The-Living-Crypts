using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] protected bool UseRandomRotation =true;
    [SerializeField] protected Vector3 StartRotation;
    [SerializeField] protected float RotationtionSpeed;
    [SerializeField] protected float Acceleration;
    [SerializeField] protected bool ShouldAccelerate;
    protected Vector3 _rotationDirection;
    protected float CurrentSpeed;

    [SerializeField] protected bool IsSpinning;
    protected virtual void Awake()
    {
        if (UseRandomRotation)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);
            _rotationDirection = new Vector3(x, y, z);
        }
        else
        {
            _rotationDirection = StartRotation;
        }
    }
    public void Begin()
    {
        if (ShouldAccelerate)
        {
            CurrentSpeed = 0f;
        }
        IsSpinning = true;
    
    }
    public void Begin(float delay)
    {
        Invoke("Begin", delay);
    }


    public virtual void Stop()
    {
        IsSpinning = false;
    }
    public virtual void SetSpeed(float speed)
    {
        RotationtionSpeed = speed;
    }

    protected virtual void Update()
    {
        if (!IsSpinning)
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

            transform.Rotate(_rotationDirection, Time.deltaTime * CurrentSpeed * RotationtionSpeed);
      
        }
        else{

            transform.Rotate(_rotationDirection, Time.deltaTime * RotationtionSpeed * RotationtionSpeed);
           
        }
    }

    public bool IsRotating { get { return IsSpinning; } }
}
