using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{

    [SerializeField] private GameObject SpawnExplosionVFXPrefab;
    [SerializeField] private GameObject SpawnPrepareVFXPrefab;
    [SerializeField] private float SpawnTime;
    [SerializeField] private LayerMask BlockingLayers;


    private GameObject spawnPrepareVFX;
    private GameObject enemyToSpawn;
    private Vector3 spawnPoint;

    public System.Action<CharacterSpawner, GameObject> OnEnemySpawned;

    public void BeginEnemySpawn(Vector3 spawnPoint, GameObject enemy )
    {
        if (ObjectPoolManager.instance)
        {
            spawnPrepareVFX= ObjectPoolManager.Spawn(SpawnPrepareVFXPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            spawnPrepareVFX = Instantiate(SpawnPrepareVFXPrefab, spawnPoint, Quaternion.identity);
        }
 
        this.spawnPoint = spawnPoint;
        enemyToSpawn = enemy;
        Invoke("SpawnEnemy", SpawnTime);

    }

    private void SpawnEnemy()
    {
        if (!enemyToSpawn)
        {
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

        if(enemyInit!= null)
        {
            enemyInit.Init();
        }

        ObjectBounds bounds = enemyObject.GetComponent<ObjectBounds>();

        if (bounds)
        {
            enemyObject.transform.position = spawnPoint + Vector3.up * bounds.GetHalfExtents().y * bounds.GetOffset().y ;

            //raycast each direction to see if chracter is in a wall
            RaycastHit hit;

            //Right
            if(Physics.Raycast(enemyObject.transform.position, Vector3.right,out hit, bounds.GetHalfExtents().x, BlockingLayers)){
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
                enemyObject.transform.position += Vector3.back * bounds.GetHalfExtents().z ;
            }

            //Right
            if (Physics.Raycast(enemyObject.transform.position, Vector3.back, out hit, bounds.GetHalfExtents().z, BlockingLayers))
            {
                enemyObject.transform.position += Vector3.back * bounds.GetHalfExtents().z ;
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
                return;
            }
            enemy.SetTarget(GameStateManager.instance.GameManager.Player.transform);
            OnEnemySpawned?.Invoke(this, enemyObject);
        }

      
    }

}
