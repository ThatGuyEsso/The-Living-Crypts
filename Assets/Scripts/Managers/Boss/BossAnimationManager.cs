using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    //Animations
    [SerializeField] private string IdleAnimation;
    [SerializeField] private string InactiveAnimation;
    [SerializeField] private string WalkAnimation;


    //Components
    private Animator _animator;

    //Animation Events
    public System.Action OnStep;
    public System.Action OnIdleComplete;
    public void Init()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        if (!_animator) Debug.LogError("Weapon has no animator: Check Parent/Root Gameobject");
    }

    public bool IsPlayingWalkAnimation()
    {
        if (!_animator.enabled) return false;

       
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(WalkAnimation)) return true;
        return false;
    }
    public bool IsPlayingIdleAnim()
    {
        if (!_animator.enabled) return false;


        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(IdleAnimation)) return true;
        return false;
    }
    public bool IsPlayingInActiveAnim()
    {
        if (!_animator.enabled) return false;


        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(InactiveAnimation)) return true;
        return false;
    }
    public void OnStepComplete()
    {
        OnStep?.Invoke();
    }
    public void OnIdleCyckeEnd()
    {
        OnIdleComplete?.Invoke();
    }

    public void PlayAnimation(string animName)
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (animName == string.Empty) return;
            _animator.Play(animName, default, 0f);
        }
    }
    public void PlayIdleAnimation()
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (IdleAnimation == string.Empty) return;
            _animator.Play(IdleAnimation, default, 0f);
        }
    }

    public void PlayInActiveAnimation()
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (InactiveAnimation == string.Empty) return;
            _animator.Play(InactiveAnimation, default, 0f);
        }
    }

    public void PlayWalkAnimation()
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (WalkAnimation == string.Empty) return;
            _animator.Play(WalkAnimation, default, 0f);
        }
    }

    public void StopAnimating()
    {
        if (_animator.enabled)
        {
            _animator.enabled = false;
        }
        //Debug.Log("Stop animating");
    }
}
