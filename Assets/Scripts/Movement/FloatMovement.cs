using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMovement : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float MaxFloatHeight;
    [SerializeField] private float MaxFloatSpeed;
    [SerializeField] private float Acceleration;
    [SerializeField] private float Deceleration;
    [SerializeField] private bool ReachMaxHeightBeforeMovememt;

    private Rigidbody _rb;
    //Movement
    private Vector3 MovementDirection;
    private bool _shouldMove;
    private float _currentSpeed;
    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
        
    }


    public void Init(float MaxHeight, float MaxSpeed,bool UseMaxHeight)
    {
        _rb = GetComponent<Rigidbody>();
        MaxFloatHeight = MaxHeight;
        MaxFloatSpeed = MaxSpeed;
        ReachMaxHeightBeforeMovememt = UseMaxHeight;
        MaxFloatHeight = transform.position.y + MaxFloatHeight;

    }
    public void OnMove(Vector3 direction)
    {
        MovementDirection = new Vector3(direction.x,0f, direction.z);
        _rb.useGravity = false;
        _shouldMove = true;
    }

    public void StopAndDrop()
    {
        _shouldMove = false;
        _rb.useGravity = true;
    }
    public void Stop()
    {
        _shouldMove = false;
   
    }

    public void Accelerate()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, MaxFloatSpeed, Time.deltaTime * Acceleration);
        if (Mathf.Abs(_currentSpeed- MaxFloatSpeed) < 0.01f)
        {
            _currentSpeed = MaxFloatSpeed;
        }
    }

    private void Update()
    {
        if (!_shouldMove)
        {
            return;
        }
        Accelerate();
        if (ReachMaxHeightBeforeMovememt)
        {
            if(transform.position.y < MaxFloatHeight)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, MaxFloatHeight,transform.position.z), _currentSpeed*Time.deltaTime );

                if (Mathf.Abs(transform.position.y - MaxFloatHeight) < 0.01f)
                {
                    transform.position = new Vector3(transform.position.x, MaxFloatHeight, transform.position.z);
                }
            }
        }



        transform.position += MovementDirection * _currentSpeed * Time.deltaTime;


    }


   
}
