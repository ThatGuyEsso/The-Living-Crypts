using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablePickUp : BasePickUp
{
    [SerializeField] private GameObject ConsumablePrefab;
    [SerializeField] private MeshRenderer MeshRenderer;
    [SerializeField] private MeshFilter GFX;
    [SerializeField] private string PickUpName;
    [Header("SFX")]
    [SerializeField] private string EquipSFX;
    [SerializeField] private AudioManager AM;
    private Collider _pickUpCollider;
    private bool _isEnabled;
    private void Awake()
    {
        _pickUpCollider = GetComponent<Collider>();

   

    
        _isEnabled = true;
        BaseConsumable consumable = ConsumablePrefab.GetComponent<BaseConsumable>();
        if (consumable)
        {
            PickUpName = consumable.Name;
            MeshRenderer.materials = consumable.Materials;
            GFX.mesh = consumable.Mesh;
        }
        else
        {
            Destroy(gameObject);
        }
        


    }

    private AudioManager GetAudioManager()
    {
        if (AM)
        {
            return AM;
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }
            else
            {
                return GameStateManager.instance.AudioManager;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isEnabled)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {

            if (ConsumablePrefab)
            {
                ConsumableManager._instance.OnNewConsumableAvailable(PickUpName, this);
            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
     
        if (other.CompareTag("Player"))
        {

            if (ConsumablePrefab)
            {
                ConsumableManager._instance.OnNoConsumableAvailable();
            }


        }
    }

    public void UpdatePickUp(GameObject newPrefab)
    {
        if (!AM)
        {
            AM = GetAudioManager();
        }
        if (AM)
        {
            AM.PlayThroughAudioPlayer(EquipSFX, transform.position);
        }
        ConsumablePrefab = newPrefab;
        if (ConsumablePrefab )
        {
            BaseConsumable consumable = newPrefab.GetComponent<BaseConsumable>();
        
            if (ConsumableManager._instance&& consumable)
            {
                PickUpName = consumable.Name;
                ConsumableManager._instance.OnNewConsumableAvailable(PickUpName, this);
                GFX.mesh = consumable.Mesh;
                MeshRenderer.materials = consumable.Materials;
            }
        }
        else
        {
            if (ConsumableManager._instance)
            {

                ConsumableManager._instance.OnNoConsumableAvailable();
                DisablePickUp();
            }
        }
      
    }

    public override bool IsEnabled()
    {
        return _isEnabled;
    }

    protected override void DoPickUp()
    {
        //
    }

    public GameObject GetConsumable()
    {
        return ConsumablePrefab;
    }

    protected override void DisablePickUp()
    {
        _isEnabled = false;
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void EnablePickUp()
    {
        _isEnabled = true;
    }
}
