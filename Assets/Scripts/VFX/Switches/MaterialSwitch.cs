using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitch : MonoBehaviour
{
    [Header("Materials")]
    private Material[] OnMaterials;
    [SerializeField] private Material[] OffMaterials;
    [Header("Settings")]
    [SerializeField] private bool ShouldStartOn;

    //Mesh Renderer
    protected MeshRenderer _meshRenderer;

    private void Awake()
    {
        if (!_meshRenderer)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        if (!_meshRenderer)
        {
            Destroy(this);
            return;
        }
        OnMaterials = _meshRenderer.materials;



    }
    // Start is called before the first frame update
    void Start()
    {
        if (!ShouldStartOn)
        {
            SwitchOff();
        }
    }

    public void SwitchOn()
    {
        _meshRenderer.materials = OnMaterials;
    }

    public void SwitchOff()
    {
        _meshRenderer.materials = OffMaterials;
    }
}
