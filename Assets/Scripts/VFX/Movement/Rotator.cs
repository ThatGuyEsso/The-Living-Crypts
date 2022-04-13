using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private bool UseRandomRotatation;
    [SerializeField] private Vector3 StartRotation;
    [SerializeField] private float RotationtionSpeed;

    private Vector3 _rotationDirection;

    private void Awake()
    {
        if (UseRandomRotatation)
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
        transform.Rotate(_rotationDirection, Time.deltaTime * RotationtionSpeed * RotationtionSpeed);
    }
}
