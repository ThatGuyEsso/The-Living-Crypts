using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Legacy : BaseWeapon
{
    public override void StopTryToPrimaryAttack()
    {
        _isPrimaryAttacking = false;
    }

    public override void StopTryToSecondaryAttack()
    {
        _isSecondaryAttacking = false;
    }

    public override void TryToPrimaryAttack()
    {
        _isPrimaryAttacking = true;
        if (_canPrimaryAttack)
        {
            DoPrimaryAttack();
        }
    }

    public override void TryToSecondaryyAttack()
    {
        _isSecondaryAttacking = true;
        if (_canSecondaryAttack)
        {
            DoSecondaryAttack();
        }
    }

    protected override void DoPrimaryAttack()
    {
        Debug.Log("Primary Attack");
    }

    protected override void DoSecondaryAttack()
    {
        Debug.Log("Secondary Attack");
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
