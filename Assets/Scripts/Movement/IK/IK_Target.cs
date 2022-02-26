using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Target : MonoBehaviour
{
    [SerializeField] private Transform TargetBody;
    [SerializeField] private LayerMask GroundLayers;


    public void Update()
    {
        if (TargetBody)
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(TargetBody.transform.position,Vector3.down,out hitInfo, Mathf.Infinity, GroundLayers))
            {
                transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
            }
        }
    }
}
