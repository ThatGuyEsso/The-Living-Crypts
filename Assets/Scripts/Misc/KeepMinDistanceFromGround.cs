using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMinDistanceFromGround : MonoBehaviour
{
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private float MinDistanceToGround;
    [SerializeField] private float MaxDistanceToGround;
    [SerializeField] private bool IsInitialMaxDistance =true;


    private void Awake()
    {
        if (IsInitialMaxDistance)
        {
            float? initDistance = GetDistanceToGround();
            if (initDistance != null)
            {
                MaxDistanceToGround = initDistance.GetValueOrDefault();
            }
 
        }
    }
    

    public float? GetDistanceToGround()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo,Mathf.Infinity, GroundLayers))
        {
            return Vector3.Distance(transform.position, hitInfo.point);
        }
        else
        {
            return null;
        }
    }


    private void LateUpdate()
    {
        float? currentDistance = GetDistanceToGround();
        if (currentDistance != null)
        {
            if (currentDistance.GetValueOrDefault() < MinDistanceToGround)
            {
                ClampMinDistance(currentDistance.GetValueOrDefault());
            }
            else if(currentDistance.GetValueOrDefault() > MaxDistanceToGround)
            {

            }
        }
    }


    public void ClampMinDistance(float currentDistance)
    {
        float difference = Mathf.Abs(MinDistanceToGround - currentDistance);

        transform.position = new Vector3(transform.position.x, transform.position.y + difference, transform.position.z);
    }
}
