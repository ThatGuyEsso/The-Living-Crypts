using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ObjectSpawnData", menuName = "New Object Spawn Data")]
public class ObjectSpawnPattern : ScriptableObject
{
    [Tooltip("Chance of using this spawn data pattern")]
    [Range(1, 10)]
    public int Weight;
    public int MinObstacles, MaxObstacles;

    public ObjectSpawnData[] AvailableObjects;


    public GameObject GetWeightedObject(int weight)
    {
        if (AvailableObjects.Length > 0)
        {

            List<GameObject> validObject = new List<GameObject>();

            foreach (ObjectSpawnData ObjectData in AvailableObjects)
            {
                if (ObjectData.Weight >= weight)
                {
                    validObject.Add(ObjectData.Object);
                }
            }


            if (validObject.Count > 0)
            {
                return validObject[Random.Range(0, validObject.Count)];

            }
        }
        return null;

    }

}

[System.Serializable]

public class ObjectSpawnData
{
    [Tooltip("Chance of this enemy spawnData")]
    [Range(1, 10)]
    public int Weight;

    public GameObject Object;

}
