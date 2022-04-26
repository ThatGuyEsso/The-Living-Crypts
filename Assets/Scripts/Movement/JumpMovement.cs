using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct JumpData
{
    public LayerMask GroundedLayers;
    public float JumpForce;
    public float JumpHeightMultiplier;
    public float MaxJumpCooldown;
}
[RequireComponent(typeof(Rigidbody))]
public class JumpMovement : MonoBehaviour,IInitialisable
{
    [Header("Jump Settings")]
    [SerializeField] private JumpData JumpSettings;


    [SerializeField] private bool _isGrounded;
    private bool _canJump;
    private bool _isResetting;
  
    private Rigidbody _rb;
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
        ResetJump();
    }

    public void DoJump(Vector3 jumpDirection)
    {
        if(_rb && _isGrounded && _canJump)
        {
            //Debug.Log("Jumping!");
            _canJump = false;
            //Jump consistency.
            _rb.velocity = Vector3.zero;
            _rb.AddForce(jumpDirection * JumpSettings.JumpForce + Vector3.up * JumpSettings.JumpHeightMultiplier, ForceMode.Impulse);
            
        }
    }
    public void DoJump(Vector3 jumpDirection, JumpData newSettings)
    {
        if (_rb && _isGrounded && _canJump)
        {
            //Debug.Log("Jumping!");
            _canJump = false;
            //Jump consistency.
            _rb.velocity = Vector3.zero;
            _rb.AddForce(jumpDirection * newSettings.JumpForce + Vector3.up * newSettings.JumpHeightMultiplier, ForceMode.Impulse);

        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        
        if (JumpSettings.GroundedLayers == (JumpSettings.GroundedLayers | (1 << collision.gameObject.layer)))
        {
            _isGrounded = true;
            if (JumpSettings.MaxJumpCooldown <= 0f) _canJump = true;
            else {
                if (_isResetting) return;
                if (!gameObject.activeInHierarchy)
                {
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(ResetJumpTimer());
            }
        }
    }


    private IEnumerator ResetJumpTimer()
    {
        _isResetting = true;
        yield return new WaitForSeconds(JumpSettings.MaxJumpCooldown);
     
        ResetJump();
    }
    public void ResetJump()
    {
        _isResetting=false;
        _canJump = true;
    }
    public void OnCollisionExit(Collision collision)
    {
       
        if (JumpSettings.GroundedLayers == (JumpSettings.GroundedLayers | (1 << collision.gameObject.layer)))
        {
            _isGrounded = false;
        }
    }

    public JumpData JumpData { get { return JumpSettings; } set { { JumpSettings = value; } } }
}
