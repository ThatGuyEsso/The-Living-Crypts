using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGenerationManager : MonoBehaviour
{
    [SerializeField] private ObjectSpawnPattern[] AvailableSpawnPatterns;

    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private int MaxWeight = 10, MinWeight = 1;
    [SerializeField] private int EnemySpawnAttempts = 5;
    [SerializeField] private GameObject ObjectSpawnerPrefab;

    private List<Room> RoomsToPopulate = new List<Room>();
    private bool _isFillingRoom;

    private ObjectSpawnPattern _currentSpawnPattern;

    public System.Action OnObstaclesSpawned;

    public void BeginFillRooms(List<Room> rooms)
    {
        RoomsToPopulate = rooms;
        _currentSpawnPattern = GetSpawnPattern(Random.Range(MinWeight, MaxWeight + 1));
        StartCoroutine(FillRooms());
    }

    public ObjectSpawnPattern GetSpawnPattern(int weight)
    {

        if (AvailableSpawnPatterns.Length > 0)
        {
            List<ObjectSpawnPattern> patterns = new List<ObjectSpawnPattern>();


            foreach (ObjectSpawnPattern pattern in AvailableSpawnPatterns)
            {
                if (pattern.Weight >= weight) patterns.Add(pattern);
            }

            if (patterns.Count > 0)
            {
                return patterns[Random.Range(0, patterns.Count)];
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;
        }

    }



    private IEnumerator FillRooms()
    {
        foreach(Room room in RoomsToPopulate)
        {
            FillRoom(room);
            yield return null;

        }
        OnObstaclesSpawned?.Invoke();

    }
    public void FillRoom(Room RoomToFill)
    {
        int nIteration = Random.Range(_currentSpawnPattern.MinObstacles, _currentSpawnPattern.MinObstacles);

        for(int i =0; i < nIteration; i++)
        {
            int randomX = Random.Range(-RoomToFill.GetRoomHalfExtents().x + 1, RoomToFill.GetRoomHalfExtents().x - 1);
            int randomZ = Random.Range(-RoomToFill.GetRoomHalfExtents().y + 1, RoomToFill.GetRoomHalfExtents().y - 1);
            Vector3 pointInSpace = RoomToFill.transform.position + new Vector3(randomX, 0f, randomZ);

            RaycastHit hit;
            ObjectSpawner spawner;
            if (Physics.Raycast(pointInSpace, Vector3.down, out hit, Mathf.Infinity, GroundLayers))
            {

                if (ObjectPoolManager.instance)
                {
                    spawner = ObjectPoolManager.Spawn(ObjectSpawnerPrefab, hit.point, Quaternion.identity).GetComponent<ObjectSpawner>();
                    if (spawner)
                    {


                        spawner.SpawnObject(hit.point, _currentSpawnPattern.GetWeightedObject(Random.Range(MinWeight, MaxWeight + 1)));
                    }

                }
                else
                {
                    spawner = Instantiate(ObjectSpawnerPrefab, hit.point, Quaternion.identity).GetComponent<ObjectSpawner>();
                    if (spawner)
                    {

                        spawner.SpawnObject(hit.point, _currentSpawnPattern.GetWeightedObject(Random.Range(MinWeight, MaxWeight + 1)));
                    }

                }

            }
        }
       
    }

}
