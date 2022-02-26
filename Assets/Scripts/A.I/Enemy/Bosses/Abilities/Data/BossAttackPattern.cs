using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackPattern 
{
   
    [SerializeField] private List<BossAbilityData> Abillities = new List<BossAbilityData>();

    public List<BossAbilityData> AbilityData { get { return Abillities; } }
    public BaseBossAbility CreateNewAbility(BaseBoss owner,int index)
    {
        BaseBossAbility ability;
        if (ObjectPoolManager.instance)
        {

            ability = ObjectPoolManager.Spawn(Abillities[index].AbilityPrefab, owner.transform).GetComponent<BaseBossAbility>();
        }
        else
        {

            ability = Object.Instantiate(Abillities[index].AbilityPrefab, owner.transform).GetComponent<BaseBossAbility>();
        }

        ability.SetUpAbility(Abillities[index], owner);

        return ability;
    }

    public static BaseBossAbility CreateNewAbility(BaseBoss owner, BossAbilityData newAbility)
    {
        BaseBossAbility ability;
        if (ObjectPoolManager.instance)
        {

            ability = ObjectPoolManager.Spawn(newAbility.AbilityPrefab, owner.transform).GetComponent<BaseBossAbility>();
        }
        else
        {

            ability = Object.Instantiate(newAbility.AbilityPrefab, owner.transform).GetComponent<BaseBossAbility>();
        }

        ability.SetUpAbility(newAbility, owner);

        return ability;
    }
}

