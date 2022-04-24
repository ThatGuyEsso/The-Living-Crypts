using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Follow settings")]
    [SerializeField] private Transform FollowTarget;
    [SerializeField] private Vector3 FollowOffset;
    public bool IsFollowing;



    private void LateUpdate()
    {
        if(IsFollowing && FollowTarget)
        {
            transform.position = FollowTarget.position + FollowOffset;
        }
    }
}
