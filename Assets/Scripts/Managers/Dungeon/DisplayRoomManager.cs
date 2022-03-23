using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRoomManager : MonoBehaviour
{
    [SerializeField] private WeaponPickUp TheLegacyPickUp;
    [SerializeField] private WeaponPickUp LeavateinnPickUp;
    [SerializeField] private WeaponPickUp TheBalancePickUp;

    private List<WeaponPickUp> _pickUps = new List<WeaponPickUp>();



    private void Awake()
    {
        if (GameStateManager.instance)
        {
            if (GameStateManager.instance.GameManager)
            {
                GameStateManager.instance.GameManager.OnNewGamplayEvent += EvaluateGameplayEvent;
            }
        }
    }

    public void EvaluateGameplayEvent(GameplayEvents newEvent){
        switch (newEvent)
        {
            case GameplayEvents.WeaponSelected:
                Init();
                break;
            case GameplayEvents.DungeonInvoked:
                break;
            case GameplayEvents.GameComplete:
                break;
            case GameplayEvents.PlayerDied:
                break;
            case GameplayEvents.PlayerRespawned:
                break;
        }
    }


    public void EvalauteWeaponSelected(string weapon)
    {
        foreach(WeaponPickUp pickup in _pickUps)
        {
            if(pickup.Name != weapon)
            {
                if (!pickup.IsEnabled())
                {
                    pickup.EnablePickUp();
                }
            }
        }
    }
    public void Init()
    {
        _pickUps.Add(TheLegacyPickUp);
        _pickUps.Add(LeavateinnPickUp);
        _pickUps.Add(TheBalancePickUp);

        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped += EvalauteWeaponSelected;
        }
    }


    private void OnDestroy()
    {
        if (GameStateManager.instance)
        {
            if (GameStateManager.instance.GameManager)
            {
                GameStateManager.instance.GameManager.OnNewGamplayEvent -= EvaluateGameplayEvent;
            }
        }

        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped -= EvalauteWeaponSelected;
        }
    }
}
