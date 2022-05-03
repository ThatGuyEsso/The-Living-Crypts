using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public static BossRoomManager Instance;
    [Header("Room Components")]
    [SerializeField] private BaseBoss Boss;
    [SerializeField] private Room Room;
    [SerializeField] private GameObject BossUIPrefab;
    [SerializeField] private bool InDebug;
    [SerializeField] private AudioManager AM;
    private BossUI _bossUI;
    private void Awake()
    {
        if (Room)
        {
            Room.OnBossRoomTriggered += InitiateBossFight;
        }
    }


    private void InitiateBossFight()
    {

        if (!AM)
        {
            AM = GetAudioManager();
        }
        if (Room)
        {
            Room.OnBossRoomTriggered -= InitiateBossFight;
        }

       

        if (!_bossUI)
        {
            if (ObjectPoolManager.instance)
            {
                _bossUI = ObjectPoolManager.Spawn(BossUIPrefab, transform).GetComponent<BossUI>();
            }
            else
            {
                _bossUI = Instantiate(BossUIPrefab, transform).GetComponent<BossUI>();
            }
        }
        _bossUI.AudioManager = AM;
        Boss.Init();
        _bossUI.OnUISpawned += StartBossFight;
        Boss.InitBossUI(_bossUI);
        if (AM)
        {
            Boss.AudioManager = AM;
        }
    }

    public void StartBossFight()
    {
        if (_bossUI) _bossUI.OnUISpawned -= StartBossFight;
        Boss.StartBossFight();
    }

    private AudioManager GetAudioManager()
    {
        if (AM)
        {
            return AM;
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }
            else
            {
                return GameStateManager.instance.AudioManager;
            }
        }
    }

}
