using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaterialFlash : MonoBehaviour, IInitialisable
{
    [SerializeField] private Material _flashMat;
    [SerializeField] private float _flashRate = 0.2f;
    private Material[] _defaultMaterials;
    private MeshRenderer[] _meshRenderers;
    private bool _shouldFlash = false;
    private bool _isFlashing;
    private float _currentTimeToFlash;
    public void Init()
    {
       
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _defaultMaterials = new Material[_meshRenderers.Length];
        for (int i=0; i<_meshRenderers.Length; i++)
        {
            _defaultMaterials[i] = _meshRenderers[i].material;
        }
 

    }

    public void BeginFlash()
    {
        _shouldFlash = true;
        _currentTimeToFlash = _flashRate;
        FlashOn();
    }
    public void FlashOn()
    {
        foreach (MeshRenderer mr in _meshRenderers)
        {
            mr.material = _flashMat;
        }
        _isFlashing = true;
    }

    public void EndFlash()
    {
        _shouldFlash = false;
        FlashOff();
    }
    public void FlashOff()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            if (i >= _meshRenderers.Length) return;
            _meshRenderers[i].material = _defaultMaterials[i];
        }
        _isFlashing = false;

    }
   

    private void Update()
    {

        if(_shouldFlash)
        {
            if(_currentTimeToFlash <= 0f)
            {
                if (_isFlashing) FlashOff();
                else FlashOn();
                _currentTimeToFlash = _flashRate;
            }
            else
            {
                _currentTimeToFlash -= Time.deltaTime;
            }
        }
    }


}
