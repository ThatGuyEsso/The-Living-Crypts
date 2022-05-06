using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BaseConsumable
{
    [SerializeField] private GameObject GrenadePrefab;
    [SerializeField] private float ThrowForce;
    [SerializeField] private string ThrowSFX;
    public override bool UseConsumable()
    {
        Grenade grenade;

        if (ObjectPoolManager.instance)
        {
            grenade = ObjectPoolManager.Spawn
                (GrenadePrefab, _owner.transform.position + _owner.transform.forward+Vector3.up*0.5f,
                Quaternion.identity).GetComponent<Grenade>();
        }
        else
        {
            grenade = Instantiate
           (GrenadePrefab, _owner.transform.position + _owner.transform.forward + Vector3.up * 0.5f,
                Quaternion.identity).GetComponent<Grenade>();
        }

        if (grenade)
        {
            grenade.AudioManager = AM;
            grenade.Owner = _owner;

            Rigidbody rb = grenade.GetComponent<Rigidbody>();

            rb.AddForce(_owner.transform.forward * ThrowForce, ForceMode.Impulse);
            if (!AM)
            {
                AM = AudioManager;
            }
            if (AM)
            {
                AM.PlayThroughAudioPlayer(ThrowSFX, transform.position);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
