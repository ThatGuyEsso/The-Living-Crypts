using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public void  ApplyDamageToTarget(GameObject target, Vector3 point);
}
