using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLootGen : MonoBehaviour
{
    public GameObject[] PossibleLoot;

    
    public void Awake()
    {
        Invoke("SpawnLoot", 5f);
      
    }

    public void SpawnLoot()
    {
        if (PossibleLoot.Length > 0)
        {
            int rand = Random.Range(0, PossibleLoot.Length);

            GameObject obj = PossibleLoot[rand];

            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(obj, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(obj, transform.position, Quaternion.identity);
            }
        }

    }
}
