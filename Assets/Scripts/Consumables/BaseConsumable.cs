using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConsumable : MonoBehaviour
{
    [Header("Consumable Settings")]
    [SerializeField] protected string ItemName;

    [Header("GFX")]
    [SerializeField] protected Mesh PickUpMesh;

    [SerializeField] protected Material[] PickUpMaterials;

    [SerializeField] protected Sprite Icon;
    protected AudioManager AM;
    protected GameObject _owner;
    public string Name { get { return ItemName; } }

    public Sprite Sprite { get { return Icon; } }
    public AudioManager AudioManager
    {
        get
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
    public Mesh Mesh
    {
        get
        {
            return PickUpMesh;
        }
    }
    public Material[] Materials
    {
        get
        {
            return PickUpMaterials;
        }
    }
    public virtual bool UseConsumable()
    {
        return false;
    }
    public GameObject Owner { get { return _owner; } set { _owner = value; } }
    public virtual void Remove()
    {
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
