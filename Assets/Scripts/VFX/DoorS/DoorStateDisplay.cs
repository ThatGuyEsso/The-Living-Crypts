using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStateDisplay : MonoBehaviour
{
    [SerializeField] private Material OpenMaterial, ClosedMaterial;
    [SerializeField] private Color OpenColour, ClosedColour;
    [SerializeField] private MeshRenderer _meshRenderer;
    private Light _light;
    private Door _parentDoor;


    private void Awake()
    {
        _parentDoor = GetComponentInParent<Door>();
        _light = GetComponentInChildren<Light>();
        if (!_meshRenderer)
        {

             _meshRenderer = GetComponent<MeshRenderer>();
        }

        if (_parentDoor)
        {
            _parentDoor.OnDoorLocked += OnLocked;
            _parentDoor.OnDoorUnlocked += OnUnlocked;
        }

    }

    public void OnLocked()
    {
        if (_light)
        {
            _light.color = ClosedColour;
        }

        if (_meshRenderer)
        {
            _meshRenderer.material = ClosedMaterial;
        }
    }
    public void OnUnlocked()
    {
        if (_light)
        {
            _light.color = OpenColour;
        }
        if (_meshRenderer)
        {
            _meshRenderer.material = OpenMaterial;
        }
    }


    private void OnDisable()
    {
        if (_parentDoor)
        {
            _parentDoor.OnDoorLocked -= OnLocked;
            _parentDoor.OnDoorLocked -= OnUnlocked;
        }

    }

    private void OnDestroy()
    {
        if (_parentDoor)
        {
            _parentDoor.OnDoorLocked -= OnLocked;
            _parentDoor.OnDoorLocked -= OnUnlocked;
        }
    }
}
