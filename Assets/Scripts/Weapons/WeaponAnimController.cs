using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimController : MonoBehaviour
{
    [SerializeField] private string[] _primaryAttackAnimations;
    [SerializeField] private string[] _secondaryAttackAnimations;
    [SerializeField] private string[] _idleAnimator;
    private Animator _animator;


    public void Init()
    {

    }
}
