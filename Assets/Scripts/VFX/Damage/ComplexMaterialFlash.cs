using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexMaterialFlash : MaterialFlash
{
    protected MeshRenderer _meshRenderer;
    public override void Init()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _defaultMaterials = new Material[_meshRenderer.materials.Length];
        for (int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            _defaultMaterials[i] = _meshRenderer.materials[i];
        }
    }
    public override void FlashOn()
    {
        Material[] FlashMats = new Material[_meshRenderer.materials.Length];

        for(int i = 0; i < _meshRenderer.materials.Length; i++)
        {
            FlashMats[i] = _flashMat;
          
        }
 
        _meshRenderer.materials = FlashMats;
    
      
        _isFlashing = true;
    }

    public override void FlashOff()
    {
      
            _meshRenderer.materials = _defaultMaterials;
       

        _isFlashing = true;
    }
}
