using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAbility : MonoBehaviour
{
    [SerializeField]
    protected BossAbilityData _abilityData;
    protected BaseBoss _owner;

    protected bool _canAttack;
    protected float _currentCooldown;

    public System.Action OnAbilityStarted;
    public System.Action OnAbilityPerformed;



    virtual public void SetUpAbility(BossAbilityData data,BaseBoss owner)
    {
        _abilityData = data;
        _owner = owner;
    }

    protected abstract void Execute();
    protected abstract void Terminate();

}
