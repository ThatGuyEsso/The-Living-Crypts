using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker, Vector3 point);
}
