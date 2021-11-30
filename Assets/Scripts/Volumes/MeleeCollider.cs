using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    IWeapon parentWeapon;
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        parentWeapon = transform.parent.GetComponent<IWeapon>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parentWeapon!=null)
        {
            parentWeapon.ApplyDamageToTarget(other.gameObject);
        }
    }
}
