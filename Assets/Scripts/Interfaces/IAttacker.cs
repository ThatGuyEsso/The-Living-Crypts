using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public float MinDamage;
    public float MaxDamage;
    public float MinKnockBack;
    public float MaxKnockBack;
    public AttackData(float minDMG, float maxDMG, float minKnockback,float maxKnockback)
    {
        MinDamage = minDMG;
        MaxDamage = maxDMG;
        MinKnockBack = minKnockback;
        MaxKnockBack = maxKnockback;
    }
};
public interface IAttacker
{

    public AttackData GetAttackData();
  
}
