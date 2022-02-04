using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BossStage
{
    First,
    Second,
    Final,
    Transition
};
public abstract class BaseBoss : BaseEnemy
{
    [Header("Boss Settings")]
    [SerializeField] protected HealthData HealthData;
    [SerializeField] protected List<BaseBossAbility> CurrentStageAbilities = new List<BaseBossAbility>();
    [SerializeField] protected List<BossAttackPattern> FirstStageAttackPattern = new List<BossAttackPattern>();
    [SerializeField] protected List<BossAttackPattern> SecondStageAttackPattern = new List<BossAttackPattern>();
    [SerializeField] protected List<BossAttackPattern> FinalStageAttackPattern = new List<BossAttackPattern>();

  protected BossStage _currentStage;


    public System.Action<BossStage> OnNewBossStage;
    public System.Action OnBossDefeated;
    
    virtual protected void BeginNewStage(BossStage newStage)
    {

    }
}
