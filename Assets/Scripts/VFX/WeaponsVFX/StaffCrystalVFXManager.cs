using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffCrystalVFXManager : MonoBehaviour
{
    [SerializeField] private Material IdleMaterial, ActiveMaterial;
    [SerializeField] private float IdleRotateSpeed, ActiveRotateSpeed;
    private MeshRenderer _renderer;
    private Rotator _rotator;
    private BaseWeapon _parentWeapon;
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        if(_renderer && IdleMaterial)
        {
            _renderer.material = IdleMaterial;
        }
        _rotator = GetComponent<Rotator>();
        if (_rotator)
        {
            _rotator.SetSpeed(IdleRotateSpeed);
        }

        _parentWeapon = GetComponentInParent<BaseWeapon>();
        if (_parentWeapon)
        {
            _parentWeapon.OnAttackStarted += OnActive;
            _parentWeapon.OnAttackEnded += OnIdle;
        }
    }

    public void OnActive()
    {
        _renderer.material = ActiveMaterial;
        _rotator.SetSpeed(ActiveRotateSpeed);
    }

    public void OnIdle()
    {
        _renderer.material = IdleMaterial;
        _rotator.SetSpeed(IdleRotateSpeed);
    }


    private void OnDisable()
    {
        if (_parentWeapon)
        {
            _parentWeapon.OnAttackStarted -= OnActive;
            _parentWeapon.OnAttackEnded -= OnIdle;
        }
    }

    private void OnDestroy()
    {
        if (_parentWeapon)
        {
            _parentWeapon.OnAttackStarted -= OnActive;
            _parentWeapon.OnAttackEnded -= OnIdle;
        }
    }
}
