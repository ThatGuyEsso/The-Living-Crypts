using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : BasePickUp
{
    [SerializeField] private float HealAmount =40f;
    [SerializeField] private AudioManager AM;
    [SerializeField] private string PickUpSFX;
    [SerializeField] private LayerMask pickUpLayers;
    private bool isEnaled;
    CharacterHealthManager healthManager;
    private void OnCollisionEnter(Collision collision)
    {
        if(pickUpLayers == (pickUpLayers | (1 << collision.gameObject.layer)))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                healthManager = collision.gameObject.GetComponent<CharacterHealthManager>();

                DoPickUp();
            }
        }
    }

    public override void EnablePickUp()
    {
        isEnaled = true;
        if (!AM)
        {
            AM = AudioManager;
        }
    }

    public override bool IsEnabled()
    {
        return isEnaled;
    }

    protected override void DisablePickUp()
    {
        isEnaled = false;
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    protected override void DoPickUp()
    {

        if (healthManager)
        {
            if (healthManager.CurrentHealth >=healthManager.GetMaxHealth())
            {
                return;
            }
            healthManager.CurrentHealth = Mathf.Clamp(healthManager.CurrentHealth += HealAmount,0f, healthManager.GetMaxHealth());
        }

        if (AM)
        {
            AM.PlayThroughAudioPlayer(PickUpSFX, transform.position);
        }
        DisablePickUp();
    

    }

    
    
}
