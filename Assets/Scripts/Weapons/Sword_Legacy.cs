using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Legacy : BaseWeapon
{
    protected override void DoPrimaryAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void DoSecondaryAttack()
    {
        throw new System.NotImplementedException();
    }

  
    protected override void ResetPrimaryAttack()
    {
        throw new System.NotImplementedException();
    }

    protected override void ResetSecondaryAttack()
    {
        throw new System.NotImplementedException();
    }


    void FixedUpdate()
    {
        FollowEquipPoint();
        MatchEquipPointRotation();
    }
}
