using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    void OnDamage(float dmg, Vector2 kBackDir, float kBackMag, GameObject attacker);
}
