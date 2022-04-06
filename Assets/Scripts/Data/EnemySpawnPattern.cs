using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "New Enemy Spawn Data")]
public class EnemySpawnPattern : ScriptableObject
{
    [Tooltip("Chance of using this spawn data pattern")]
    [Range(1,10)]
    public int Weight;
    public int MinEnemiesToSpawn, MaxEnemiesToSpawn;
    public int MinEnemiesPerWave, MaxEnemiesPerWave;

    public float MinSpawnLagTime, MaxLagTime;

    public EnemySpawnData[] AvailableEnemies;


    public GameObject GetWeightedEnemy(int weight)
    {
        if (AvailableEnemies.Length > 0)
        {
       
            List<GameObject> validEnemies = new List<GameObject>();

            foreach (EnemySpawnData enemyData in AvailableEnemies)
            {
                if (enemyData.Weight >= weight)
                {
                    validEnemies.Add(enemyData.Enemy);
                }
            }


            if (validEnemies.Count > 0)
            {
                return validEnemies[Random.Range(0, validEnemies.Count)]; 

            }
        }
        return null;
    


  

    }

}

[System.Serializable]

public class EnemySpawnData
{
    [Tooltip("Chance of this enemy spawnData")]
    [Range(1, 10)]
    public int Weight;

    public GameObject Enemy;

}
