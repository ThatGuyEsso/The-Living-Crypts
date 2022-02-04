using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackPattern 
{
   
    [SerializeField] private List<BossAbilityData> Abillities = new List<BossAbilityData>();


    public BaseBossAbility CreateNewAbility(BaseBoss owner,int index)
    {
        
        BaseBossAbility ability = ObjectPoolManager.Spawn(Abillities[index].AbilityPrefab, owner.transform).GetComponent<BaseBossAbility>();

        ability.SetUpAbility(Abillities[index], owner);

        return ability;
    }
}
