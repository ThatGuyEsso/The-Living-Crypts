using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimController : MonoBehaviour
{
    //Animations
    [SerializeField] private string[] _primaryAttackAnimations;
    [SerializeField] private string[] _secondaryAttackAnimations;
    [SerializeField] private string[] _idleAnimation;

    //Components
    private Animator _animator;

    //Animation Events
    public Action OnAttackAnimBegin;
    public Action OnAttackAnimEnd;
    public void Init()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        if (!_animator) Debug.LogError("Weapon has no animator: Check Parent/Root Gameobject");
    }

    public bool IsPlayingPrimaryAttack()
    {
        if (!_animator.enabled) return false;

        for(int i= 0; i<_primaryAttackAnimations.Length; i++)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_primaryAttackAnimations[i])) return true;
        }
        return false;
    }
    public bool IsPlayingSecondaryAttack()
    {
        if (!_animator.enabled) return false;

        for (int i = 0; i < _secondaryAttackAnimations.Length; i++)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_secondaryAttackAnimations[i])) return true;
        }
        return false;
    }
    public void BeginAttack()
    {
        OnAttackAnimBegin?.Invoke();
    }
    public void EndAttackAttack()
    {
        OnAttackAnimEnd?.Invoke();
    }

    public void PlayPrimaryAttackAnimation(int index)
    {
        if (_animator)
        {
            if(!_animator.enabled)  _animator.enabled = true;
            if (index >= _primaryAttackAnimations.Length) return;
            if (_primaryAttackAnimations[index] == string.Empty ) return;
             _animator.Play(_primaryAttackAnimations[index], default, 0f);
        }
    }
    public void PlayAnimation(string animName)
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if ( animName == string.Empty) return;
            _animator.Play(animName, default, 0f);
        }
    }
    public void PlaySecondaryAttackAnimation(int index)
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (index >= _secondaryAttackAnimations.Length) return;
            if (_secondaryAttackAnimations[index] == string.Empty) return;
            _animator.Play(_secondaryAttackAnimations[index], default, 0f);
        }
    }

    public void PlayIdleAnimation(int index)
    {
        if (_animator)
        {
            if (!_animator.enabled) _animator.enabled = true;
            if (index >= _idleAnimation.Length) return;
            if (_idleAnimation[index] == string.Empty) return;
            _animator.Play(_idleAnimation[index], default, 0f);
        }
    }

    public void StopAnimating()
    {
        if (_animator.enabled) _animator.enabled = false;
        //Debug.Log("Stop animating");
    }
}
