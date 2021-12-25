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

 
    private bool _isGrounded;
    private bool _canJump;

  
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
            Debug.Log("Jumping!");
            _canJump = false;
            //Jump consistency.
            //_rb.velocity = Vector3.zero;
            _rb.AddForce(jumpDirection * JumpSettings.JumpForce + Vector3.up * JumpSettings.JumpHeightMultiplier, ForceMode.Impulse);
            
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (_isGrounded) return;
        if (JumpSettings.GroundedLayers == (JumpSettings.GroundedLayers | (1 << collision.gameObject.layer)))
        {
            _isGrounded = true;
            if (JumpSettings.MaxJumpCooldown <= 0f) _canJump = true;
            else {
                StopAllCoroutines();
                StartCoroutine(ResetJumpTimer());
            }
        }
    }


    private IEnumerator ResetJumpTimer()
    {
        yield return new WaitForSeconds(JumpSettings.MaxJumpCooldown);
        ResetJump();
    }
    public void ResetJump()
    {
        _canJump = true;
    }
    public void OnCollisionExit(Collision collision)
    {
        if (!_isGrounded) return;
        if (JumpSettings.GroundedLayers == (JumpSettings.GroundedLayers | (1 << collision.gameObject.layer)))
        {
            _isGrounded = false;
        }
    }
}
