using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMaths : MonoBehaviour
{
    [SerializeField] private Vector3 _origin;
    [SerializeField] private Vector3 _targetPoint;
    [SerializeField] private float MaxHeight =25f;
    [SerializeField] private Vector3 _initVelocity;
    [SerializeField] private int Resolution =10;
    [SerializeField] private bool InDebug;


    public struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
    public void CalculateInitialVelocity()
    {

        _initVelocity = CalculateLaunchData().initialVelocity;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_origin, 0.25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPoint, 0.25f);

        Gizmos.color = Color.yellow;


    }

 

  


    LaunchData CalculateLaunchData()
    {
        float displacementY = _targetPoint.y - _origin.y;
        Vector3 displacementXZ = new Vector3(_targetPoint.x - _origin.x, 0, _targetPoint.z - _origin.z);
        float time = Mathf.Sqrt(-2 * MaxHeight /- Physics.gravity.magnitude) + Mathf.Sqrt(2 * (displacementY - MaxHeight) / -Physics.gravity.magnitude);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * -Physics.gravity.magnitude * MaxHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(-Physics.gravity.magnitude), time);
    }

    public static LaunchData CalculateLaunchData(Vector3 origin, Vector3 targetPoint, float MaxArcHeight,float gravity)
    {
        float displacementY = targetPoint.y - origin.y;
        Vector3 displacementXZ = new Vector3(targetPoint.x - origin.x, 0, targetPoint.z - origin.z);
        float time = Mathf.Sqrt(-2 * MaxArcHeight / gravity) + Mathf.Sqrt(2 * (displacementY - MaxArcHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * -Physics.gravity.magnitude * MaxArcHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
    void DrawPath(LaunchData launchData)
    {

        Vector3 previousDrawPoint = _origin;

     
        for (int i = 1; i <= Resolution; i++)
        {
            float simulationTime = i / (float)Resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * -Physics.gravity.magnitude * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = _origin + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }

    public static void DrawPath(LaunchData launchData,Vector3 origin, float gravity, int resolution)
    {

        Vector3 previousDrawPoint = origin;


        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = origin + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
        }
    }
}
