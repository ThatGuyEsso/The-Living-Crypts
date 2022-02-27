using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAbility : MonoBehaviour
{
    [SerializeField]
    protected BossAbilityData _abilityData;
    protected BaseBoss _owner;
    [SerializeField] protected float HoldFinalPoseTime;
    protected bool _canAttack = true;
    protected float _currentCooldown;

    public System.Action OnAbilityStarted;
    public System.Action OnAbilityPerformed;
    public System.Action OnAbilityFinished;
    protected bool _isInitialised;

    virtual  public void Init()
    {
        _isInitialised = true;
    }
    virtual public void SetUpAbility(BossAbilityData data,BaseBoss owner)
    {
        _abilityData = data;
        _owner = owner;
    }

    public abstract void Execute();
    public abstract void Terminate();

    public bool CanAttack()
    {
        return _canAttack;
    }
    public bool InAttackRange(Vector3 targetPoint)
    {
        return Vector3.Distance(targetPoint, _owner.transform.position) <= _abilityData.AttackRange || _abilityData.AttackRange < 0;
    }
    virtual protected IEnumerator WaitToExecuteAttack(float time)
    {
        yield return new WaitForSeconds(time);
        PerformAttack();
    }
    virtual protected IEnumerator WaitToEndAttack(float time)
    {
        yield return new WaitForSeconds(time);
        Terminate();
    }
    public float GetTimeBetweenAbilities()
    {
        return _abilityData.AttackCooldown;
    }
    virtual public void PerformAttack()
    {

    }
}
