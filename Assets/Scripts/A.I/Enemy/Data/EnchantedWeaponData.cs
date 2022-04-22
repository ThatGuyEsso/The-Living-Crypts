using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enchanted Weapon Data", menuName = "NewEnchanted Weapon Data")]
[System.Serializable]
public class EnchantedWeaponData : ScriptableObject
{
    public GameObject EnchantedWeaponPrefab;
    public AttackData attackData;
    public float MovementSpeed;
    public float MinFloatHeight=0.5f, MaxFloatHeight =1f;
    public float MinMoveTime, MaxMoveTime;

    public float GetFloatHeight()
    {
        return Random.Range(MinFloatHeight, MaxFloatHeight);
    }
}
