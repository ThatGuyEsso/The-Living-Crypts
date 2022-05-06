using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptEnemyManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnPattern[] AvailableSpawnPatterns;

    [SerializeField] private LayerMask GroundLayers;

    [SerializeField] private int MaxWeight = 10, MinWeight = 1;
    [SerializeField] private int EnemySpawnAttempts = 5;
    [SerializeField] private GameObject CharacterSpawnerPrefab;



    private Room _owner;

    private List<CharacterSpawner> _enemySpawners = new List<CharacterSpawner>();
    private List<GameObject> enemies = new List<GameObject>();
    private EnemySpawnPattern _currentSpawnPattern;

    [SerializeField] private int _currentWaveEnemyCount;
    [SerializeField] private int _enemiesToSpawnLeft;
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

    public void OnEnemySpawned(CharacterSpawner spawner, GameObject enemy)
    {
        enemies.Add(enemy);
        if (_enemySpawners.Count > 0)
        {
            CharacterSpawner spawnerToRemove=_enemySpawners.Find(  item => item.gameObject == spawner.gameObject);
            if (spawnerToRemove)
            {
                spawnerToRemove.OnEnemySpawned -= OnEnemySpawned;
                _enemySpawners.Remove(spawnerToRemove);
            }
            if (spawnerToRemove.gameObject)
            {
                if (ObjectPoolManager.instance)
                {
                    ObjectPoolManager.Recycle(spawnerToRemove.gameObject);
                }
                else
                {
                    Destroy(spawnerToRemove.gameObject);
                }
            }
        }

   

        if (enemy)
        {
            CryptCharacterManager manager = enemy.GetComponent<CryptCharacterManager>();

            if (manager)
            {
         
                OnAddCharacter(manager);
                _enemiesToSpawnLeft--;
              
            }
        }
    }

    public void OnAddCharacters(List<CryptCharacterManager> CharactersAdded)
    {
        if (CharactersAdded.Count > 0)
        {
            foreach(CryptCharacterManager character in CharactersAdded)
            {
                if (character)
                {
                    character.OnCharacterRemoved += OnCharacterRemoved;
                    character.OnAddCharacter += OnAddCharacter;
                    _currentWaveEnemyCount++;

                }
            }
        }
   

    }
    public void OnAddCharacter(CryptCharacterManager CharacterAdded)
    {
        if (CharacterAdded)
        {
            CharacterAdded.OnCharacterRemoved += OnCharacterRemoved;
            CharacterAdded.OnAddCharacter += OnAddCharacter;

            _currentWaveEnemyCount++;
        }
     


    }

    public void OnCharacterRemoved(CryptCharacterManager CharacterRemoved)
    {
        if (CharacterRemoved)
        {
            CharacterRemoved.OnCharacterRemoved -= OnCharacterRemoved;
            CharacterRemoved.OnAddCharacter -= OnAddCharacter;
      
            _currentWaveEnemyCount--;
            
        }
        EvaluateWave();
    }

    public void EvaluateWave()
    {
        
        if(_currentWaveEnemyCount <= 0)
        {
            enemies.Clear();
            Debug.Log("Wave complete");
            if (_enemiesToSpawnLeft>0)
            {
                Debug.Log("Next wave");
                BeginSpawnCurrentWave();
            }
            else
            {
                OnEnemiesCleared?.Invoke();
                Debug.Log("Room cleared");
            }
   
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
                while(!isPlaced && attempts < EnemySpawnAttempts)
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
        int randomX = Random.Range(-_owner.GetRoomHalfExtents().x+2, _owner.GetRoomHalfExtents().x-2);
        int randomZ = Random.Range(-_owner.GetRoomHalfExtents().y+2, _owner.GetRoomHalfExtents().y-2);
        Vector3 pointInSpace = _owner.transform.position + new Vector3(randomX, 0f , randomZ);

        RaycastHit hit;
        CharacterSpawner spawner;
        if (Physics.Raycast(pointInSpace,Vector3.down, out hit, Mathf.Infinity, GroundLayers)){
       
            if (ObjectPoolManager.instance)
            {
                spawner = ObjectPoolManager.Spawn(CharacterSpawnerPrefab, hit.point, Quaternion.identity).GetComponent<CharacterSpawner>();
                if (spawner)
                {
                    _enemySpawners.Add(spawner);
                    spawner.OnEnemySpawned += OnEnemySpawned;
                    spawner.BeginEnemySpawn(hit.point, EnemyPrefab);
                }
                return true;
            }
            else
            {
                spawner =  Instantiate(CharacterSpawnerPrefab, hit.point, Quaternion.identity).GetComponent<CharacterSpawner>();
                if (spawner)
                {
                    _enemySpawners.Add(spawner);
                    spawner.OnEnemySpawned += OnEnemySpawned;
                    spawner.BeginEnemySpawn(hit.point, EnemyPrefab);
                }
                return true;
            }
            
        }

        return false;
    }
}
