using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private HUDPrompt Prompt;

    [SerializeField] private PlayerHealthBar PlayerHealthBar;
    [SerializeField] private GameObject MiniMap;
    private CharacterHealthManager _playerHealthManager;

    private GameManager _gameManager;

    private WeaponDisplayManager _weaponDisplayManager;

    private ItemDisplayManager _itemDisplayManager;

    private List<GameObject> HUDElements = new List<GameObject>();
    private void Awake()
    {
        if (Prompt)
        {
            HUDElements.Add(Prompt.gameObject);

        }
        if (PlayerHealthBar)
        {
            HUDElements.Add(PlayerHealthBar.gameObject);

        }
        if (MiniMap)
        {
            HUDElements.Add(MiniMap.gameObject);

        }
        if (WeaponDisplayManager)
        {
            HUDElements.Add(_weaponDisplayManager.gameObject);

        }
        if (ItemDisplayManager)
        {
            HUDElements.Add(_itemDisplayManager.gameObject);

        }

        HideHUD();
        Prompt.gameObject.SetActive(true);
    }

    public void HideHUD()
    {
        if (HUDElements.Count > 0)
        {
            foreach(GameObject element in HUDElements)
            {
                if (element)
                {
                    element.SetActive(false);
                }
            }
        }
    }

    public void ShowHUD()
    {
        if (HUDElements.Count > 0)
        {
            foreach (GameObject element in HUDElements)
            {
                if (element)
                {
                    element.SetActive(true);
                }
            }
        }
    }
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
    public ItemDisplayManager ItemDisplayManager
    {
        get
        {
            if (_itemDisplayManager)
            {
                return _itemDisplayManager;
            }
            else
            {
                _itemDisplayManager = GetComponentInChildren<ItemDisplayManager>();
                return _itemDisplayManager;
            }
        }
    }

    public WeaponDisplayManager WeaponDisplayManager
    {
        get
        {
            if (_weaponDisplayManager)
            {
                return _weaponDisplayManager;
            }
            else
            {
                _weaponDisplayManager = GetComponentInChildren<WeaponDisplayManager>();
                return _weaponDisplayManager;
            }
        }
    }

    public void EvalauteGameplayEvent(GameplayEvents newEvents)
    {
        switch (newEvents)
        {
            case GameplayEvents.WeaponSelected:
                if (WeaponDisplayManager)
                {
                    WeaponDisplayManager.gameObject.SetActive(true);
                }
                break;
            case GameplayEvents.DungeonInvoked:
                break;
            case GameplayEvents.DungeonBegunGenerating:
                break;
            case GameplayEvents.DungeonGenComplete:
                if (MiniMap)
                {
                    MiniMap.SetActive(true);
                }
                PlayerHealthBar.gameObject.SetActive(true);
                break;
            case GameplayEvents.GameComplete:
                break;
            case GameplayEvents.PlayerDied:
                HideHUD();
                break;
            case GameplayEvents.PlayerRespawned:
                Prompt.gameObject.SetActive(true);
                break;
            case GameplayEvents.EnteredCombat:
                if (MiniMap)
                {
                    MiniMap.SetActive(false);
                }
                break;
            case GameplayEvents.LeftCombat:
                if (MiniMap)
                {
                    MiniMap.SetActive(true);
                }
                break;
            case GameplayEvents.Restart:
                HideHUD();
                break;
            case GameplayEvents.OnOBossSequenceBegun:
                HideHUD();
                break;
            case GameplayEvents.OnBossFightBegun:
                PlayerHealthBar.gameObject.SetActive(true);
                _weaponDisplayManager.gameObject.SetActive(true);
                ItemDisplayManager.gameObject.SetActive(true);
                break;
        }
    }
    public void Init(GameObject player, GameManager manager)
    {
        if (MiniMap)
        {
            MiniMap.SetActive(false);
        }
        _gameManager = manager;
        _gameManager.OnNewGamplayEvent += EvalauteGameplayEvent;
        _playerHealthManager = player.GetComponent<CharacterHealthManager>();
        if (WeaponDisplayManager)
        {
            WeaponDisplayManager.gameObject.SetActive(false);
        }
        if (ItemDisplayManager)
        {
            ItemDisplayManager.gameObject.SetActive(false);
        }
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
