using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EssoUtility : MonoBehaviour
{

    //Converts angle into vector
    public static float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }
    public static bool InSameDirection(Vector3 a,Vector3 b, float errorMargin)
    {
        float dot = Vector2.Dot(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
        if (Mathf.Abs(dot + errorMargin)>1) dot = 1;
        return dot >= 1;
  
    }
    //returns angle from vector direction

    public static Vector3 GetVectorFromAngle(float angle)
    {
        //angle -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static Vector2 GetVectorToPointer(Camera camRef, Vector3 orign)
    {
        //Get mouse position in world space
        Vector3 pointerPos = camRef.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 orignToMouse = pointerPos - orign;//calculate vector direction between player and cursor

        return orignToMouse.normalized;//Return normalised direction
    }


    //only works for orthographic cameras
    public static Vector3 GetOrthCameraWorldBounds(Camera activeCamera)
    {

        Vector3 bounds = activeCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, activeCamera.transform.position.z));
        return bounds;
    }

    public static Vector2 MaxCamBounds(Camera activeCamera)
    {

        Vector3 bounds = activeCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, activeCamera.transform.position.z));
        return bounds;

    }
    public static Vector2 MinCamBounds(Camera activeCamera)
    {

        Vector3 bounds = activeCamera.ScreenToWorldPoint(new Vector3(0f, 0f, activeCamera.transform.position.z));
        return bounds;

    }
    public static Vector3 GetLowerOrthCameraWorldBounds(Camera activeCamera)
    {

        Vector3 bounds = activeCamera.ScreenToWorldPoint(new Vector3(-Screen.width, -Screen.height, activeCamera.transform.position.z));
        return bounds;
    }

    public static Vector3[] GetVectorsInArc(Vector3 dir, int count, float arc, float spread)
    {
        Vector3[] vectors = new Vector3[count];

        for (int i = 0; i < vectors.Length; i++)
        {
            float startingAngle = (GetAngleFromVector(dir) - arc / 2);
            //startingAngle -= 90f;


            float randOffset = UnityEngine.Random.Range(-spread, spread);

            vectors[i] = GetVectorFromAngle(randOffset + startingAngle + arc);
        }

        return vectors;
    }

    public static Vector3 GetCameraLookAtPoint(Camera cam, float raycastDist,LayerMask  targetLayers)
    {
        //Vector3 centrePoint = cam.ViewportToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, cam.nearClipPlane));
        RaycastHit hitInfo;
        Vector3 lookAtPoint = cam.transform.position + cam.transform.forward*raycastDist;
       

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, raycastDist, targetLayers))
        {
            lookAtPoint = hitInfo.point;
        }
        return lookAtPoint;
    }
    public static float GetAspectRatio()
    {
        float aspectRatio = Screen.width / Screen.height;

        return aspectRatio;
    }

}
