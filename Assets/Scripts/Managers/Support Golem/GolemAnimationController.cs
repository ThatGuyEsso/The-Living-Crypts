using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAnimationController : MonoBehaviour
{
    [SerializeField] private string _attackAnim;
    [SerializeField] private string _walkAnim;


    //Components
    private Animator _animator;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        if (!_animator) Debug.LogError("Support has no animator: Check Parent/Root Gameobject");
    }
    public void Stop()
    {
        if(_animator && _animator.enabled)
        {
            _animator.enabled = false;
        }
    }
    public void PlayAttackAnim()
    {
        if (_animator)
        {
            if (!_animator.enabled)
            {
                _animator.enabled = true;
            }

            _animator.Play(_attackAnim, default, 0f);
   
        }
    }

    public void PlayWalkCycle()
    {
        if (_animator)
        {
            if (!_animator.enabled)
            {
                _animator.enabled = true;
            }

            _animator.Play(_walkAnim, default, 0f);

        }
    }
    public bool IsPlayingAttackAnimation()
    {
        if (!_animator.enabled)
        {
            return false;
        }


        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_attackAnim))
        {
            return true;
        }
      
        return false;
    }


}
