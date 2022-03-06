using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateProjectileArc : MonoBehaviour
{
    [SerializeField] private Vector3 _origin;
    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private float _launchAngle =50f;
    private Vector3 _initVelocity;
    [SerializeField] private int nPoints =10;
    [SerializeField] private Vector3[] points;
    public void CalculateInitialVelocity()
    {
        //Displacement
        Vector3 Displacement = _targetPoint- _origin;

   
    }
    private void Awake()
    {
        CalculateInitialVelocity();
        points = new Vector3[nPoints];
        for (int i = 0; i < points.Length; i++)
        {
            //float time = Vector3.Distance(_origin, _targetPoint) / (float)nPoints;
            float time = i * 0.1f;
            Vector3 pointPosition = (_initVelocity * time + 0.5f * Physics.gravity * time * time)+_origin;

            points[i] = pointPosition;
        }
        for (int i = 0; i < points.Length; i++)
        {
            if (i + 1 < points.Length)
            {
                Debug.DrawLine(points[i], points[i+1], Color.yellow, 100f);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_origin, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPoint, 0.25f);

        Gizmos.color = Color.yellow;
  
     
    }
}
