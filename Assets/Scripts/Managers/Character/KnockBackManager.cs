using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KnockBackManager : MonoBehaviour
{
    private CharacterHealthManager _hManager;
    private Rigidbody _rb;
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        _hManager = GetComponent<CharacterHealthManager>();
        if (!_hManager) Destroy(this);
        else
        {
            _hManager.OnDamageReceived += ApplyKnockBack;
        
            _rb = GetComponent<Rigidbody>();
        }
 
    }


    private void ApplyKnockBack(float maxHealth, float dmg, float knockBackMag, Vector3 kBackDir, Vector3 point)
    {
        if (_rb)
            _rb.AddForce(kBackDir * knockBackMag, ForceMode.Impulse);
    }


    private void OnDestroy()
    {
        if (_hManager) _hManager.OnDamageReceived -= ApplyKnockBack;
    }

    private void OnDisable()
    {
        if (_hManager) _hManager.OnDamageReceived -= ApplyKnockBack;
    }
}
