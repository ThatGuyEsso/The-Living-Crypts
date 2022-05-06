using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [SerializeField] private ObjectSpawnPattern LootTable;
    [SerializeField] private float MinDropForce,MaxDropForce;
    [SerializeField] private bool ActivateOnDestroy;
    public void SpawnLoot()
    {
        List<GameObject> lootToSpawn = new List<GameObject>();
        if (LootTable.AvailableObjects.Length > 0)
        {
            int nAttempts = Random.Range(LootTable.MinObstacles, LootTable.MaxObstacles );
    
            for (int i = 0; i < nAttempts; i++)
            {
                int randValue = Random.Range(1,10);

                GameObject loot = LootTable.GetWeightedObject(randValue);
                if (loot)
                {
                    lootToSpawn.Add(loot);
                }
            }
        }
        if (lootToSpawn.Count > 0)
        {
            foreach(GameObject loot in lootToSpawn)
            {
                BasePickUp pickUp;
                if (ObjectPoolManager.instance)
                {
                    pickUp= ObjectPoolManager.Spawn
                        (loot,transform.position,Quaternion.identity).GetComponent<BasePickUp>();
                }
                else
                {
                    pickUp = Instantiate
                         (loot, transform.position, Quaternion.identity).GetComponent<BasePickUp>();
                }
          

                if (!pickUp)
                {
                    Destroy(loot);
               
                }
                pickUp.EnablePickUp();
                Rigidbody rb = loot.GetComponent<Rigidbody>();

                if (rb)
                {
                    float force = Random.Range(MinDropForce, MaxDropForce);
                    float randX = Random.Range(-1f, 1f);
                    float randZ = Random.Range(-1f, 1f);
                    Vector3 launchDir = new Vector3(randX, 1f, randZ);
                    rb.AddForce(force * launchDir, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (ActivateOnDestroy)
        {
            SpawnLoot();
        }
    }

    private void OnDisable()
    {
        if (ActivateOnDestroy)
        {
            SpawnLoot();
        }
    }
}
