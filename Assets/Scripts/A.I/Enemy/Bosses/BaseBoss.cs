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
    [SerializeField] protected string BossName;
    [SerializeField] protected float PercentageStageTrigger =0.3f;
    [SerializeField] protected List<BaseBossAbility> _currentStageAbility = new List<BaseBossAbility>();
    [SerializeField] protected List<BossAttackPattern> FirstStageAttackPattern = new List<BossAttackPattern>();
    [SerializeField] protected List<BossAttackPattern> SecondStageAttackPattern = new List<BossAttackPattern>();
    [SerializeField] protected List<BossAttackPattern> FinalStageAttackPattern = new List<BossAttackPattern>();

    protected BossStage _currentStage;
    protected BossStage _previousStage;
    [Header("Boss Components")]
    protected BossUI _bossUI;

    public System.Action<BossStage> OnNewBossStage;
    public System.Action OnBossDefeated;

    
    virtual public void InitBossUI(BossUI UI)
    {
        _bossUI = UI;
        if (_hManager)
        {
            _hManager.Init();
            _bossUI.InitialiseUI(BossName, _hManager.GetMaxHealth());
            _hManager.OnHurt += OnHurt;
            _hManager.OnDie += EndBossFight;
        }
    }

    virtual protected void OnHurt()
    {
        if (_bossUI&& _hManager)
        {
            _bossUI.DoHurtUpdate(_hManager.CurrentHealth);
            EvaluateStageTrigger();
        }
    }
    virtual protected void BeginNewStage(BossStage newStage)
    {
        _previousStage = _currentStage;
        _currentStage = newStage;
    }
    virtual public void StartBossFight()
    {
        BeginNewStage(BossStage.First);
    }

    virtual public void EndBossFight()
    {
        Debug.Log("End Boss fight");
        _bossUI.DoHurtUpdate(0.0f);
        _bossUI.HideUI();
    }

    public void EvaluateStageTrigger()
    {
        float healthPercentage = _hManager.CurrentHealth/_hManager.GetMaxHealth();
        switch (_currentStage)
        {
            case BossStage.First:
                if (healthPercentage <= 1f - PercentageStageTrigger)
                {
                    _hManager.CurrentHealth = healthPercentage * _hManager.GetMaxHealth();
                    _bossUI.DoHurtUpdate(_hManager.CurrentHealth);
                    Debug.Log("First Stage Complete");
                    BeginNewStage(BossStage.Transition);
                }
                break;
            case BossStage.Second:
                if (healthPercentage <= 1f - PercentageStageTrigger*2f)
                {
                    _hManager.CurrentHealth = healthPercentage * _hManager.GetMaxHealth();
                    _bossUI.DoHurtUpdate(_hManager.CurrentHealth);
                    Debug.Log("Second Stage Complete");
                    BeginNewStage(BossStage.Transition);
                }
                break;
       
            case BossStage.Transition:
                break;
        }
    }

    public void EndTransitionStage()
    {
        if (_previousStage != BossStage.Transition)
        {
            _previousStage++;
            BeginNewStage(_previousStage);
        }
    }

    virtual protected void OnDestroy()
    {
        if (_hManager)
        {
            
            _hManager.OnHurt -= OnHurt;
        }
    }
}
