using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFreeRigidBBody : MonoBehaviour
{
   public void Init()
    {
        gameObject.AddComponent<Rigidbody>();

        Collider[] childColliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in childColliders)
        {
            collider.isTrigger = false;
        }

        transform.SetParent(null);
    }
}
