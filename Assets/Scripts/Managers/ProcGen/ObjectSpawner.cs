using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    [SerializeField] private LayerMask BlockingLayers;

    public System.Action<GameObject> OnObjectSpawned;


    public void SpawnObject(Vector3 point,GameObject ObjectToSpawn)
    {
        Vector3 spawnPoint = point;
        if (!ObjectToSpawn)
        {
            if (ObjectPoolManager.instance)
            {
                if (gameObject)
                {
                    ObjectPoolManager.Recycle(gameObject);
                }
            }
            else
            {

                Destroy(gameObject);
            }
            return;
        }

        GameObject NewObject;
        if (ObjectPoolManager.instance)
        {
            NewObject = ObjectPoolManager.Spawn(ObjectToSpawn, spawnPoint, Quaternion.identity);
        }
        else
        {
            NewObject = Instantiate(ObjectToSpawn, spawnPoint, Quaternion.identity);
        }

        IInitialisable ObjToInit = NewObject.GetComponent<IInitialisable>();

        if (ObjToInit != null)
        {
            ObjToInit.Init();
        }
        if(NewObject && !NewObject.activeInHierarchy)
        {
            if (ObjectPoolManager.instance)
            {
                if (gameObject)
                {
                    ObjectPoolManager.Recycle(gameObject);
                }
            }
            else
            {

                Destroy(gameObject);
            }
            return;
        }

        ObjectBounds bounds = NewObject.GetComponent<ObjectBounds>();

        if (bounds)
        {
            NewObject.transform.position = spawnPoint + Vector3.up * bounds.GetHalfExtents().y + Vector3.up * bounds.GetOffset().y;

            //raycast each direction to see if chracter is in a wall
            RaycastHit hit;

            //Right
            if (Physics.Raycast(NewObject.transform.position, Vector3.right, out hit, bounds.GetHalfExtents().x, BlockingLayers))
            {
                NewObject.transform.position += Vector3.left * bounds.GetHalfExtents().x;
            }


            //left
            if (Physics.Raycast(NewObject.transform.position, Vector3.left, out hit, bounds.GetHalfExtents().x, BlockingLayers))
            {
                NewObject.transform.position += Vector3.right * bounds.GetHalfExtents().x;

         
            }
            //forward
            if (Physics.Raycast(NewObject.transform.position, Vector3.forward, out hit, bounds.GetHalfExtents().z, BlockingLayers))
            {
                NewObject.transform.position += Vector3.back * bounds.GetHalfExtents().z;
            }

            //Right
            if (Physics.Raycast(NewObject.transform.position, Vector3.back, out hit, bounds.GetHalfExtents().z, BlockingLayers))
            {
                NewObject.transform.position += Vector3.forward * bounds.GetHalfExtents().z;
            }
        }
        OnObjectSpawned?.Invoke(NewObject);
        if (ObjectPoolManager.instance)
        {
            if (gameObject)
            {
                ObjectPoolManager.Recycle(gameObject);
            }
        }
        else
        {

            Destroy(gameObject);
        }



    }

}
