using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDPrompt Prompt;

    [SerializeField] private PlayerHealthBar PlayerHealthBar;

    private CharacterHealthManager _playerHealthManager;

    private GameManager _gameManager;
    public HUDPrompt PromptManager
    { get
        {
            if (Prompt)
            {
                return Prompt;
            }
            else
            {
                Prompt = GetComponentInChildren<HUDPrompt>();
                return Prompt;
            }
        }
    }

    public void EvalauteGameplayEvent(GameplayEvents newEvents)
    {
        switch (newEvents)
        {
            case GameplayEvents.WeaponSelected:
                break;
            case GameplayEvents.DungeonInvoked:
                break;
            case GameplayEvents.DungeonBegunGenerating:
                break;
            case GameplayEvents.DungeonGenComplete:
                PlayerHealthBar.gameObject.SetActive(true);
                break;
            case GameplayEvents.GameComplete:
                break;
            case GameplayEvents.PlayerDied:
                break;
            case GameplayEvents.PlayerRespawned:
                PlayerHealthBar.gameObject.SetActive(false);
                break;
        }
    }
    public void Init(GameObject player, GameManager manager)
    {
        _gameManager = manager;
        _gameManager.OnNewGamplayEvent += EvalauteGameplayEvent;
        _playerHealthManager = player.GetComponent<CharacterHealthManager>();
        if (PlayerHealthBar)
        {
            if (_playerHealthManager)
            {
                PlayerHealthBar.SetUpHealthBar(_playerHealthManager.CurrentHealth, _playerHealthManager.GetMaxHealth());
                _playerHealthManager.OnHealthUpdated += OnHealthChanged;

            }
            PlayerHealthBar.gameObject.SetActive(false);
        }
    }
    public void OnHealthChanged(float health)
    {
        PlayerHealthBar.OnUpdateDamageDisplay(health);
    }

    private void OnDisable()
    {
        if (_playerHealthManager)
        {
            _playerHealthManager.OnHealthUpdated -= OnHealthChanged;

        }

        if (_gameManager)
        {
            _gameManager.OnNewGamplayEvent -= EvalauteGameplayEvent;
        }
    }

}
