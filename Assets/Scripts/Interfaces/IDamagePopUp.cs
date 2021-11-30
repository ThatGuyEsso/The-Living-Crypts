using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagePopUp 
{
    void InitDamageNumber(float thisMaxHealth, float damage, Vector3 damageDirection);
}
