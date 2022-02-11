using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDamageReceiver : MonoBehaviour,IDamage
{
    private CharacterHealthManager _healthManager;

    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker)
    {
        if (_healthManager) _healthManager.OnDamage(dmg, kBackDir, kBackMag, attacker);
    }

    private void Awake()
    {
        _healthManager = GetComponentInParent<CharacterHealthManager>();
        if (!_healthManager) Destroy(this);
    }



}
