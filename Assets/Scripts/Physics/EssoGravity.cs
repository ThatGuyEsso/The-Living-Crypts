using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EssoGravity : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] private  float gravityScale = 1.0f;

    public static float globalGravity = -9.81f;


    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

    }
    private void OnDisable()
    {
        rb.useGravity = true;
    }
    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
    

    public float GravityScale { set  { gravityScale = value;} 
    get { return gravityScale; }
    }
}
