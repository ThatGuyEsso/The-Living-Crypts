using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptEnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnPattern[] AvailableSpawnPatterns;

    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private int MaxWeight = 10, MinWeight = 1;
    [SerializeField] private int EnemySpawnAttempts = 5;

    private Room _owner;

    private EnemySpawnPattern _currentSpawnPattern;

    private int _currentWaveEnemyCount;
    private int _enemiesToSpawnLeft;

    public System.Action OnEnemiesCleared;
    public void Init(Room owner)
    {
        _owner = owner;

        _currentSpawnPattern = GetSpawnPattern(Random.Range(MinWeight, MaxWeight + 1));
    }


    public EnemySpawnPattern GetSpawnPattern( int weight)
    {

        if (AvailableSpawnPatterns.Length > 0)
        {
            List<EnemySpawnPattern> patterns = new List<EnemySpawnPattern>();


            foreach (EnemySpawnPattern pattern in AvailableSpawnPatterns)
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


    public void Begin()
    {
        if (!_currentSpawnPattern)
        {
            return;
        }


        _enemiesToSpawnLeft = Random.Range(_currentSpawnPattern.MinEnemiesToSpawn, _currentSpawnPattern.MaxEnemiesToSpawn+1);
        BeginSpawnCurrentWave();


    }

    public void BeginSpawnCurrentWave()
    {
        float spawnLagTime = Random.Range(_currentSpawnPattern.MinSpawnLagTime, _currentSpawnPattern.MaxLagTime);
        int nToSpawn = Random.Range(_currentSpawnPattern.MinEnemiesPerWave, _currentSpawnPattern.MaxEnemiesPerWave);

        if(nToSpawn> _enemiesToSpawnLeft)
        {
            nToSpawn = _enemiesToSpawnLeft;
        }
        _enemiesToSpawnLeft-= nToSpawn;
        _currentWaveEnemyCount = nToSpawn;


        List<GameObject> enemies = new List<GameObject>();

        for(int i=1; i <= nToSpawn; i++)
        {
            int weight = Random.Range(MinWeight, MaxWeight + 1);

            GameObject enemy = _currentSpawnPattern.GetWeightedEnemy(weight);
            if (enemy)
            {
                enemies.Add(enemy);
            }
        }

        StartCoroutine(SpawnWaveCurrentWave(enemies, spawnLagTime));
    }
    IEnumerator SpawnWaveCurrentWave(List<GameObject> enemiesToSpawn, float lagTime)
    {
        if (enemiesToSpawn.Count > 0)
        {
            foreach(GameObject enemy in enemiesToSpawn)
            {
                bool isPlaced = false;
                int attempts = 0;
                while(!isPlaced ||attempts < EnemySpawnAttempts)
                {
                    isPlaced =SpawnEnemy(enemy);
                    attempts++;
                }
    


                yield return new WaitForSeconds(lagTime);
            }
      
        }
    
  
    }

    

    public bool SpawnEnemy(GameObject EnemyPrefab)
    {
        int randomX = Random.Range(-_owner.GetRoomHalfExtents().x, _owner.GetRoomHalfExtents().x);
        int randomY = Random.Range(-_owner.GetRoomHalfExtents().y, _owner.GetRoomHalfExtents().y);
        Vector3 pointInSpace = _owner.transform.position + new Vector3(randomX, randomY, 0f);

        RaycastHit hit;

        if(Physics.Raycast(pointInSpace,Vector3.down, out hit, Mathf.Infinity, GroundLayers)){

            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(EnemyPrefab, hit.point, Quaternion.identity);
                return true;
            }
            else
            {
                Instantiate(EnemyPrefab, hit.point, Quaternion.identity);
                return true;
            }
        }

        return false;
    }
}
