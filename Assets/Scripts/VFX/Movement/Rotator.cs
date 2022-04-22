using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private bool UseRandomRotation =true;
    [SerializeField] private Vector3 StartRotation;
    [SerializeField] private float RotationtionSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private bool ShouldAccelerate;
    private Vector3 _rotationDirection;
    private float CurrentSpeed;
    private void Awake()
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

    public void SetSpeed(float speed)
    {
        RotationtionSpeed = speed;
    }

    private void Update()
    {
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
}
