using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexHitFlashManager : HitFlashManager
{
    protected MaterialFlash[] _flashVFXs;

    [SerializeField] protected bool EnableonAwake;
    protected override void Awake()
    {
        if (EnableonAwake)
        {
            Init();
        }
    }
    public override void Init()
    {

       _hManager = GetComponent<CharacterHealthManager>();
        if (!_hManager) Destroy(this);
        else
        {
            _hManager.OnHurt += BeginFlash;
            _hManager.OnNotHurt += EndFlash;
        }
        _flashVFXs = GetComponentsInChildren<MaterialFlash>();

        foreach (MaterialFlash flash in _flashVFXs)
        {
            flash.Init();
        }
        _isInitialised = true;

    }



    public override void BeginFlash()
    {
        if (_flashVFXs.Length > 0)
        {
            foreach(MaterialFlash flash in _flashVFXs)
            {
                flash.BeginFlash();
            }
        }
    }

    public override void EndFlash()
    {
        if (_flashVFXs.Length > 0)
        {
            foreach (MaterialFlash flash in _flashVFXs)
            {
                flash.EndFlash();
            }
        }
    }
}
