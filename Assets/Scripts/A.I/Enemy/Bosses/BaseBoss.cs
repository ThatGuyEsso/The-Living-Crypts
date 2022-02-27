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
    [Header("Boss Abillities")]
    [SerializeField] protected List<BaseBossAbility> CurrentStageAbility = new List<BaseBossAbility>();
    [SerializeField] protected BossAbilityData TransitionAbilityData;
    [SerializeField] protected BaseBossAbility TransitionAbility;
    [Header("Boss AttackPatterns")]
    [SerializeField] protected BossAttackPattern FirstStageAttackPattern;
    [SerializeField] protected BossAttackPattern SecondStageAttackPattern;
    [SerializeField] protected BossAttackPattern FinalStageAttackPattern;
    protected BossStage _currentStage;
    protected BossStage _previousStage;
    [Header("Boss Components")]
    protected BossUI _bossUI;
    public System.Action<BossStage> OnNewBossStage;
    public System.Action OnBossDefeated;
    //Indices
    protected int _curentAttackIndex;
    //Counters
    protected float _timeBetweenAttack;
    //States
    protected bool _isUsingAttack;
    protected bool _canUseAttack;
    protected bool _bossFightRunning;
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
    virtual protected void SetUpNewAbilities()
    {     
            ClearEquippedAbilities();
        
            switch (_currentStage)
            {
                case BossStage.First:
                    //validation
                    if (FirstStageAttackPattern == null) return;
                    if (FirstStageAttackPattern.AbilityData.Count == 0) return;

                    //spawning
                    for (int i = 0; i < FirstStageAttackPattern.AbilityData.Count; i++)
                    {
                        
                            CurrentStageAbility.Add(FirstStageAttackPattern.CreateNewAbility(this, i));
                            if (FirstStageAttackPattern.AbilityData[i].IsTransitionAbility)
                            {
                                TransitionAbility = CurrentStageAbility[CurrentStageAbility.Count - 1];
                            }
                      
                 
                    }
                    break;
                case BossStage.Second:
                    //validation
                    if (SecondStageAttackPattern == null) return;
                    if (SecondStageAttackPattern.AbilityData.Count == 0) return;

                     //spawning
                    for (int i = 0; i < SecondStageAttackPattern.AbilityData.Count; i++)
                    {
                        CurrentStageAbility.Add(SecondStageAttackPattern.CreateNewAbility(this, i));
                        if (SecondStageAttackPattern.AbilityData[i].IsTransitionAbility)
                        {
                            TransitionAbility = CurrentStageAbility[CurrentStageAbility.Count - 1];
                        }
                    }
                    break;
                case BossStage.Final:
                    //validation
                    if (FinalStageAttackPattern == null) return;
                    if (FinalStageAttackPattern.AbilityData.Count == 0) return;

                    //spawning
                    for (int i = 0; i < FinalStageAttackPattern.AbilityData.Count; i++)
                    {
                        CurrentStageAbility.Add(FinalStageAttackPattern.CreateNewAbility(this, i));
                        if (FinalStageAttackPattern.AbilityData[i].IsTransitionAbility)
                        {
                            TransitionAbility = CurrentStageAbility[CurrentStageAbility.Count - 1];
                        }
                    }
                    break;
            }
        if (!TransitionAbility)
        {
            TransitionAbility = BossAttackPattern.CreateNewAbility(this, TransitionAbilityData);
        }
        
    }

    public void ClearEquippedAbilities()
    {
        if (CurrentStageAbility.Count == 0) return;
        foreach(BaseBossAbility ability in CurrentStageAbility)
        {
            if (ObjectPoolManager.instance)
            {
                if(ability.gameObject)
                    ObjectPoolManager.Recycle(ability.gameObject);
            }
            else
            {
                if (ability.gameObject)
                    Destroy(ability.gameObject);
            }
        }
        if (TransitionAbility.gameObject.activeInHierarchy)
        {
            if (ObjectPoolManager.instance)
            {
                if (TransitionAbility.gameObject)
                    ObjectPoolManager.Recycle(TransitionAbility.gameObject);
            }
            else
            {
                if (TransitionAbility.gameObject)
                    Destroy(TransitionAbility.gameObject);
            }
        }
        TransitionAbility = null;
        CurrentStageAbility.Clear();
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
        PathFinder.Init();
        _bossFightRunning = true;
        _canUseAttack = true;
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

    virtual public void NextAttack()
    {
        if (CurrentStageAbility.Count == 0) return;
        _curentAttackIndex++;

        if(_curentAttackIndex >= CurrentStageAbility.Count)
        {
            _curentAttackIndex = 0;
        }
        CurrentStageAbility[_curentAttackIndex].OnAbilityStarted += OnAttackStarted;
        CurrentStageAbility[_curentAttackIndex].OnAbilityFinished += OnAttackComplete;
    }
    virtual public void SkipAttack()
    {
        if (CurrentStageAbility.Count == 0) return;
        CurrentStageAbility[_curentAttackIndex].OnAbilityStarted -= OnAttackStarted;
        CurrentStageAbility[_curentAttackIndex].OnAbilityFinished -= OnAttackComplete;
        NextAttack();
    }
    virtual protected void ExecuteAbility()
    {
        if (CanUseAbility())
        {
            CurrentStageAbility[_curentAttackIndex].Execute();
            Debug.Log("Use Ability");
        }
  
    }

    virtual protected void OnAttackStarted()
    {
        _isUsingAttack = true;
        CurrentStageAbility[_curentAttackIndex].OnAbilityStarted -= OnAttackStarted;
    }
    virtual protected void OnAttackComplete()
    {
        _isUsingAttack = false;
        _canUseAttack = false;
        CurrentStageAbility[_curentAttackIndex].OnAbilityFinished -= OnAttackComplete;
        _timeBetweenAttack = CurrentStageAbility[_curentAttackIndex].GetTimeBetweenAbilities();
    }
    virtual protected bool InAbilityRange()
    {
        return (CurrentStageAbility[_curentAttackIndex].InAttackRange(CurrentTarget.position));
    }
    virtual protected bool CanUseAbility()
    {
        return (CurrentStageAbility[_curentAttackIndex].CanAttack());
    }
    public void EndTransitionStage()
    {
        if (_previousStage != BossStage.Transition)
        {
            _previousStage++;
            BeginNewStage(_previousStage);
        }
    }

    virtual protected void Update()
    {
        if (!_bossFightRunning) return;
        if(!_canUseAttack && _currentTimeBtwnAttacks > 0)
        {
            _currentTimeBtwnAttacks -= Time.deltaTime;
            if (_currentTimeBtwnAttacks <= 0)
            {
                _canUseAttack = true;
            }
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
