using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterHealthManager health = other.GetComponent<CharacterHealthManager>();

        if (!health)
        {
            return;

        }
        health.KillCharacter();
    }
}
