using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{

    [SerializeField] private GameObject SpawnExplosionVFXPrefab;
    [SerializeField] private GameObject SpawnPrepareVFXPrefab;
    [SerializeField] private float SpawnTime;
    [SerializeField] private LayerMask BlockingLayers;

    [SerializeField] private string SpawnSFX;
    private GameObject spawnPrepareVFX;
    private GameObject enemyToSpawn;
    private Vector3 spawnPoint;
    protected AudioManager AM;
    protected CryptEnemyManager _enemyManager;
    public System.Action<CharacterSpawner, GameObject> OnEnemySpawned;

    public void BeginEnemySpawn(Vector3 spawnPoint, GameObject enemy, CryptEnemyManager manager )
    {
        if (ObjectPoolManager.instance)
        {
            spawnPrepareVFX= ObjectPoolManager.Spawn(SpawnPrepareVFXPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            spawnPrepareVFX = Instantiate(SpawnPrepareVFXPrefab, spawnPoint, Quaternion.identity);
        }
        _enemyManager = manager;
        this.spawnPoint = spawnPoint;
        enemyToSpawn = enemy;
        Invoke("SpawnEnemy", SpawnTime);

    }
    public virtual AudioPlayer PlaySFX(string sfxName, bool randPitch)
    {
        if (AM)
        {
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }

            AM = GameStateManager.instance.AudioManager;
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
    }
    private void SpawnEnemy()
    {
        if (!enemyToSpawn)
        {
            _enemyManager.OnEnemySpawnFailed();
            return;
        }

        if (spawnPrepareVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(spawnPrepareVFX);
                spawnPrepareVFX = null;
            }
            else
            {
                Destroy(spawnPrepareVFX);
                spawnPrepareVFX = null;
            }

        }
        GameObject enemyObject;
        if (ObjectPoolManager.instance)
        {
             enemyObject = ObjectPoolManager.Spawn(enemyToSpawn, spawnPoint, Quaternion.identity);
        }
        else
        {
             enemyObject  = Instantiate(enemyToSpawn, spawnPoint, Quaternion.identity);
        }

        IInitialisable enemyInit = enemyObject.GetComponent<IInitialisable>();

        if(enemyInit== null)
        {
            _enemyManager.OnEnemySpawnFailed();
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Recycle(enemyObject);
            }
            else
            {
                Destroy(enemyObject);
            }
            return;

        }
        enemyInit.Init();
        ObjectBounds bounds = enemyObject.GetComponent<ObjectBounds>();
        if (!bounds)
        {
            bounds = enemyObject.GetComponentInChildren<ObjectBounds>();
        }

        if (bounds)
        {
            enemyObject.transform.position = spawnPoint + Vector3.up * bounds.GetHalfExtents().y + Vector3.up * bounds.GetOffset().y ;

            //raycast each direction to see if chracter is in a wall
            RaycastHit hit;

            //Right
            if (Physics.Raycast(enemyObject.transform.position, Vector3.right, out hit, bounds.GetHalfExtents().x, BlockingLayers))
            {
                enemyObject.transform.position += Vector3.left * bounds.GetHalfExtents().x;
            }


            //left
            if (Physics.Raycast(enemyObject.transform.position, Vector3.left, out hit, bounds.GetHalfExtents().x, BlockingLayers))
            {
                enemyObject.transform.position += Vector3.right * bounds.GetHalfExtents().x;
            }

            //forward
            if (Physics.Raycast(enemyObject.transform.position, Vector3.forward, out hit, bounds.GetHalfExtents().z, BlockingLayers))
            {
                enemyObject.transform.position += Vector3.back * bounds.GetHalfExtents().z;
            }

            //Right
            if (Physics.Raycast(enemyObject.transform.position, Vector3.back, out hit, bounds.GetHalfExtents().z, BlockingLayers))
            {
                enemyObject.transform.position += Vector3.back * bounds.GetHalfExtents().z;
            }

        }

        if (ObjectPoolManager.instance)
        {
           ObjectPoolManager.Spawn(SpawnExplosionVFXPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
             Instantiate(SpawnExplosionVFXPrefab, enemyObject.transform.position, Quaternion.identity);
        }

        IEnemy enemy = enemyObject.GetComponent<IEnemy>();
        if (enemy!=null)
        {
            if(!GameStateManager.instance || !GameStateManager.instance.GameManager || !GameStateManager.instance.GameManager.Player)
            {
                _enemyManager.OnEnemySpawnFailed();
                if (ObjectPoolManager.instance)
                {
                    ObjectPoolManager.Recycle(enemyObject);
                }
                else
                {
                    Destroy(enemyObject);
                }
                return;
            }


            CryptCharacterManager CharacterManager = enemyObject.GetComponent<CryptCharacterManager>();

            if (!CharacterManager)
            {
                _enemyManager.OnEnemySpawnFailed();
                if (ObjectPoolManager.instance)
                {
                    ObjectPoolManager.Recycle(enemyObject);
                }
                else
                {
                    Destroy(enemyObject);
                }
            }
            CharacterManager.OnCharacterSpawned(_enemyManager);
            enemy.SetTarget(GameStateManager.instance.GameManager.Player.transform);
            OnEnemySpawned?.Invoke(this, enemyObject);
            PlaySFX(SpawnSFX, true);


        }

        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
