using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDamageReceiver : MonoBehaviour,IDamage
{
   [SerializeField] private CharacterHealthManager _healthManager;

    public void OnDamage(float dmg, Vector3 kBackDir, float kBackMag, GameObject attacker,Vector3 point)
    {
        if (_healthManager) _healthManager.OnDamage(dmg, kBackDir, kBackMag, attacker,point);
    }

    private void Start()
    {
        _healthManager = GetComponentInParent<CharacterHealthManager>();
        if (!_healthManager) Destroy(this);
    }



}
