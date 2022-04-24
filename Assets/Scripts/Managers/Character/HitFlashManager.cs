using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashManager : MonoBehaviour
{
    protected MaterialFlash _hurtFlashVFX;
    protected CharacterHealthManager _hManager;


    protected virtual void Awake()
    {
        Init();
    }
    public virtual void Init()
    {
        _hManager = GetComponent<CharacterHealthManager>();
        if (!_hManager) Destroy(this);
        else
        {
            _hManager.OnHurt += BeginFlash;
            _hManager.OnNotHurt += EndFlash;
        }
        _hurtFlashVFX = GetComponent<MaterialFlash>();
        if(_hurtFlashVFX)
            _hurtFlashVFX.Init();
    }
    private void OnEnable()
    {
        if (!_hManager)
        {
            return;
        }

        _hManager.OnHurt += BeginFlash;
        _hManager.OnNotHurt += EndFlash;
    }
    public virtual void BeginFlash()
    {
        if (_hurtFlashVFX)
            _hurtFlashVFX.BeginFlash();
    }
    public virtual void EndFlash()
    {
        if (_hurtFlashVFX)
            _hurtFlashVFX.EndFlash();
    }

    protected virtual void OnDestroy()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= BeginFlash;
            _hManager.OnNotHurt -= EndFlash;
        }
    }

    protected  virtual void OnDisable()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= BeginFlash;
            _hManager.OnNotHurt -= EndFlash;
        }
    }
}
