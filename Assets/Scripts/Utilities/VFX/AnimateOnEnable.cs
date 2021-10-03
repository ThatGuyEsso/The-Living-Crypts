using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnEnable : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animName;
    private void OnEnable()
    {
        if (animator)
        {
            animator.enabled = true;
            animator.Play(animName, -1, 0f);

        }
    }
}
