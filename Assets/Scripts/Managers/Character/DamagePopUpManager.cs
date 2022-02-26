using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject DamageVFX;
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
            _hManager.OnDamageReceived += SpawnDamagePopUp;
        }
    }
    private void SpawnDamagePopUp(float maxHealth, float dmg,float knockBackMag, Vector3 kBackDir,Vector3 point)
    {
        IDamagePopUp popUp = Instantiate(DamageVFX, transform.position, Quaternion.identity).GetComponent<IDamagePopUp>();
        if (popUp != null)
        {
            popUp.InitDamageNumber(maxHealth, dmg, kBackDir, point);
        }
    }

    private void OnDestroy()
    {
        if(_hManager) _hManager.OnDamageReceived -= SpawnDamagePopUp;
    }

    private void OnDisable()
    {
        if (_hManager) _hManager.OnDamageReceived -= SpawnDamagePopUp;
    }
}
