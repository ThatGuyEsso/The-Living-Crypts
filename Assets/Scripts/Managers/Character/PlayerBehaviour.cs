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
    }


    public void OnDamageScreenShake()
    {
        if (CamShake.instance)
        {
            CamShake.instance.DoScreenShake(DamageShake);
        }
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
            GetGameManager();
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

    public void GetGameManager()
    {
        if (!GameStateManager.instance)
        {
            return;
        }
        if (!GameStateManager.instance.GameManager)
        {
            return;
        }
        _gameManager = GameStateManager.instance.GameManager;
    }

   
    private void OnDisable()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnDamageScreenShake;
            _healthManager.OnDie -= OnPlayerKilled;
        }
    }

    private void OnDestroy()
    {
        if (_healthManager)
        {
            _healthManager.OnHurt -= OnDamageScreenShake;
            _healthManager.OnDie -= OnPlayerKilled;
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
