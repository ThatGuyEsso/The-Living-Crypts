using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlashManager : MonoBehaviour
{
    private MaterialFlash _hurtFlashVFX;
    private CharacterHealthManager _hManager;
    private void Awake()
    {
        Init();
    }
    public void Init()
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

    private void BeginFlash()
    {
        if (_hurtFlashVFX)
            _hurtFlashVFX.BeginFlash();
    }
    private void EndFlash()
    {
        if (_hurtFlashVFX)
            _hurtFlashVFX.EndFlash();
    }

    private void OnDestroy()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= BeginFlash;
            _hManager.OnNotHurt -= EndFlash;
        }
    }

    private void OnDisable()
    {
        if (_hManager)
        {
            _hManager.OnHurt -= BeginFlash;
            _hManager.OnNotHurt -= EndFlash;
        }
    }
}
