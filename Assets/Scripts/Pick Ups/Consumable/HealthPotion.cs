using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : BaseConsumable
{
    [SerializeField] private float HealAmount;
    [SerializeField] private string UseSFX;

    public override bool UseConsumable()
    {
    

        CharacterHealthManager healthManager = _owner.GetComponent<CharacterHealthManager>();
        if (healthManager)
        {
            if (healthManager.CurrentHealth >= healthManager.GetMaxHealth())
            {
                return false;
            }
            healthManager.CurrentHealth = Mathf.Clamp(healthManager.CurrentHealth += HealAmount, 0f, healthManager.GetMaxHealth());
        }

        if (!AM)
        {
            AM = AudioManager;
        }
        if (AM)
        {
            AM.PlayThroughAudioPlayer(UseSFX, transform.position);
        }
        return true;
    }
}
