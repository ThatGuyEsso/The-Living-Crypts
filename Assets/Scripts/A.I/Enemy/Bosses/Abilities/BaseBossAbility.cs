using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAbility : MonoBehaviour
{
    [SerializeField]
    protected BossAbilityData _abilityData;
    protected BaseBoss _owner;
    [SerializeField] protected float HoldFinalPoseTime;
    [Tooltip("How long the ready up pose is held before attack is executed")]
    [SerializeField] protected float MaxPoseTime;
    protected bool _canAttack = true;
    protected float _currentCooldown;

    public System.Action OnAbilityStarted;
    public System.Action OnAbilityPerformed;
    public System.Action OnAbilityFinished;
    protected bool _isInitialised;
    protected float currentCoolDown = 0;
    protected float currentTimeToSkip = 0;
    protected bool IsActive;
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


    virtual protected IEnumerator WaitToReset(float time)
    {
        yield return new WaitForSeconds(time);
        OnReset();
    }

    virtual protected void OnReset()
    {

    }
    virtual protected void OnReadyUpBegin()
    {
        

    }
    virtual protected void OnReadyUpComplete()
    {
      

    }


    virtual protected void OnAttackEnd()
    {
      
    }

    public virtual void CancelAttack()
    {

    }

    public float GetTimeBetweenAbilities()
    {
        return _abilityData.AttackCooldown;
    }

    public BossAbilityData GetAbilityData()
    {
        return _abilityData;
    }
    virtual public void PerformAttack()
    {

    }

    virtual protected void OnDestroy()
    {
        CancelAttack();
    }

    virtual protected void OnDisable()
    {
        CancelAttack();
    }
}
