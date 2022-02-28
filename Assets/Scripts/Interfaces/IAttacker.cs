using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackData
{
    float Damage;
    float KnockBackMagnitude;

    public AttackData(float dmg, float kBackMag)
    {
        Damage = dmg;
        KnockBackMagnitude = kBackMag;
    }
};
public interface IAttacker
{

    public AttackData GetAttackData();
  
}
