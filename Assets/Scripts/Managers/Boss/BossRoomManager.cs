using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class BossRoomManager : MonoBehaviour
{
    public static BossRoomManager Instance;
    [Header("Room Components")]
    [SerializeField] private BaseBoss Boss;
    [SerializeField] private Room Room;
    [SerializeField] private GameObject BossUIPrefab;
    [SerializeField] private bool InDebug;
    [SerializeField] private AudioManager AM;
    [SerializeField] private Transform PlayerTransform;

    [Header("Cutscene")]
    [SerializeField] private CinemachineVirtualCamera CutsceneCamera;
    [SerializeField] private PlayableDirector Director;
    [SerializeField] private PlayableAsset BossIntro;
    [SerializeField] private float PostCutSceneHoldTime;
    private GameObject playerCameraObject;
    private BossUI _bossUI;
    private void Awake()
    {
        Director = GetComponent<PlayableDirector>();
        if (Director)
        {
            Director.enabled = false;
        }
     
        if (Room)
        {
            Room.OnBossRoomTriggered += InitiateBossFight;
        }
        if(CutsceneCamera)
        {
            CutsceneCamera.gameObject.SetActive(false);
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
        _bossUI.OnUISpawned += AwakenBoss;
        Boss.InitBossUI(_bossUI);
        if (AM)
        {
            Boss.AudioManager = AM;
        }
        StartBossIntro();
    }

    public void StartBossIntro()
    {
        playerCameraObject = CamShake.instance.brain.ActiveVirtualCamera.VirtualCameraGameObject;
        playerCameraObject.SetActive(false);
        CutsceneCamera.gameObject.SetActive(true);
        Director.enabled = true;
        Director.playableAsset = BossIntro;
        Director.time = 0f;
     
        Director.Play();
    }
    public void EndBossIntro()
    {
        CutsceneCamera.gameObject.SetActive(false);

        playerCameraObject.SetActive(true);
        Director.enabled = false;
    }
    public void WaitToStartBossFight()
    {
        Invoke("StartBossFight", PostCutSceneHoldTime);
    }
    public void AwakenBoss()
    {
      
        if (_bossUI) _bossUI.OnUISpawned -= AwakenBoss;
        Boss.AwakenBoss();
    }
    public void StartBossFight()
    {
        EndBossIntro();
        if (_bossUI) _bossUI.OnUISpawned -= StartBossFight;
        if (!PlayerTransform)
        {
            PlayerTransform = GetPlayerTransform();
        }


        if (PlayerTransform)
        {
            Boss.SetTarget(PlayerTransform);
        }


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
    private Transform GetPlayerTransform()
    {
        if (PlayerTransform)
        {
            return PlayerTransform;
        }
        else
        {
            if (!GameStateManager.instance && !GameStateManager.instance.GameManager && !GameStateManager.instance.GameManager.Player)
            {
                return null;
            }
            else
            {
                return GameStateManager.instance.GameManager.Player.transform;
            }
        }
    }
}
