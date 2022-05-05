using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterHealthManager))]
public class PlayerBehaviour : MonoBehaviour,Iteam
{
    [SerializeField] private bool InDebug;

    [SerializeField] private IInitialisable[] ComponentsToInit;
    [SerializeField] private ICharacterComponents[] ICharacterComponents;
    [SerializeField] private GameObject[] ManagerToInit;

    [Header("VFX")]
    [SerializeField] private CamShakeSetting DamageShake;

    //Cached referencs
    private CharacterHealthManager _healthManager;
    private GameManager _gameManager;

    public System.Action OnPlayerDied;
    public System.Action<bool> OnInputChanged;
    public System.Action OnPlayerReset;
    private void Awake()
    {
        if (InDebug)
        {
            Init();
        }
    }
    public void Init()
    {
        ComponentsToInit = GetComponents<IInitialisable>();
        ICharacterComponents = GetComponents<ICharacterComponents>();
        foreach (IInitialisable comp in ComponentsToInit)
        {

            comp.Init();
        }

        if (ManagerToInit.Length > 0)
        {
            foreach (GameObject comp in ManagerToInit)
            {
                IInitialisable initComp = comp.GetComponent<IInitialisable>();
                if (initComp!=null)
                {
                    initComp.Init();
                }
            
            }
        }

        if (_healthManager)
        {
            _healthManager.OnHurt += OnDamageScreenShake;
            _healthManager.OnDie += OnPlayerKilled;
        }
        else
        {
            _healthManager = GetComponent<CharacterHealthManager>();
            _healthManager.OnHurt += OnDamageScreenShake;
            _healthManager.OnDie += OnPlayerKilled;
        }
        if (!_gameManager)
        {
            _gameManager = GetGameManager();
        }

        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent += EvaluateGameplayEvents;
        }
    }

    public void EvaluateGameplayEvents(GameplayEvents newState)
    {
        switch (newState)
        {
      
            case GameplayEvents.Restart:
                DisablePlayerComponents();
                break;
            case GameplayEvents.ExitLevel:
                DisablePlayerComponents();
                break;
            case GameplayEvents.Quit:
                DisablePlayerComponents();
                break;

            case GameplayEvents.OnOBossSequenceBegun:
                DisablePlayerComponents();
                break;
            case GameplayEvents.OnBossFightBegun:
                EnablePlayerComponents();
                break;
        }
    }

    public void EvaluateNewGameState(GameState newState)
    {
        switch (newState)
        {
      
            case GameState.GamePaused:
                DisablePlayerComponents();
                break;
            case GameState.GameRunning:
                EnablePlayerComponents();
                break;
        }
    }
    public void OnDamageScreenShake()
    {
        if (CamShake.instance)
        {
            CamShake.instance.DoScreenShake(DamageShake);
        }
    }
    public void DisablePlayerComponents()
    {
        if (ICharacterComponents.Length > 0)
        {
            foreach (ICharacterComponents comp in ICharacterComponents)
            {
                comp.DisableComponent();
            }
        }
        OnInputChanged?.Invoke(false);
    }

    public void EnablePlayerComponents()
    {
        if (ICharacterComponents.Length > 0)
        {
            foreach (ICharacterComponents comp in ICharacterComponents)
            {
                comp.EnableComponent();
            }
        }
        OnInputChanged?.Invoke(true);

    }
    private void OnPlayerKilled()
    {
        OnPlayerDied?.Invoke();
        if (ICharacterComponents.Length > 0)
        {
            foreach(ICharacterComponents comp in ICharacterComponents)
            {
                comp.DisableComponent();
            }
        }

        if (!_gameManager)
        {
            _gameManager =GetGameManager();
        }

        if (_gameManager)
        {
            _gameManager.BeginNewGameplayEvent(GameplayEvents.PlayerDied);
        }
        else
        {
            Debug.LogError("No Game Manager");
        }
    }

    public GameManager GetGameManager()
    {
        if (!GameStateManager.instance)
        {
            return null;
        }
        if (!GameStateManager.instance.GameManager)
        {
            return null;
        }
        return  GameStateManager.instance.GameManager;
    }

   
    private void OnDisable()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnDamageScreenShake;
            _healthManager.OnDie -= OnPlayerKilled;
        }
        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvaluateGameplayEvents;
        }
    }

    private void OnDestroy()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnDamageScreenShake;
            _healthManager.OnDie -= OnPlayerKilled;
        }
        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvaluateGameplayEvents;
        }
    }

    public Team GetTeam()
    {
        return Team.Player;
    }

    public bool IsOnTeam(Team team)
    {
        return Team.Player == team;
    }


    public void ResetCharacter()
    {
        if (ICharacterComponents.Length > 0)
        {
            foreach(ICharacterComponents comp in ICharacterComponents)
            {
                comp.ResetComponent();
            }
        }

        _healthManager.ResetHealth();
      
        OnPlayerReset?.Invoke();
    }





}
