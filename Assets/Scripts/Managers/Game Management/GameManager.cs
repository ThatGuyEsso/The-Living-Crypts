using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameplayEvents
{
    WeaponSelected,
    DungeonInvoked,
    GameComplete,
    PlayerDied,
    PlayerRespawned
};

public class GameManager : MonoBehaviour, IManager, IInitialisable
{

    [SerializeField] private GameObject RoomManagerPrefab;
    [SerializeField] private GameObject[] dungeonManagersToInit;
    [SerializeField] private Material SkyBox;
    [SerializeField] private LightingSettings lightSettings;
    // Start is called before the first frame update

    public System.Action<GameplayEvents> OnNewGamplayEvent;

    private bool _isWaiting;

    private RoomManager _roomManager;
    private HUDManager _HUDManager;
    private SceneTransitionManager _sceneManager;
    private Transform _spawnPoint;
    private GameObject _player;
    private GameplayEvents _currentGameplayEvent;
    public void BindToGameStateManager()
    {
        GameStateManager.instance.OnNewGameState += EvaluateGameState;
    }

    public void EvaluateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.GamePaused:
             
                break;
            case GameState.GameRunning:

                break;




        }
    }

    public void BeginNewGameplayEvent(GameplayEvents Event)
    {
        _currentGameplayEvent = Event;
        OnNewGamplayEvent?.Invoke(_currentGameplayEvent);
    }

    public void Init()
    {
        if (GameStateManager.instance)
        {
            GameStateManager.instance.GameManager = this;
            BindToGameStateManager();
        }
    }
    public void InitGame()
    {
        RenderSettings.skybox = SkyBox;

        StartCoroutine(SetUpGameScene());
    }
    private void InitDungeonManagers()
    {
        foreach (GameObject manager in dungeonManagersToInit)
        {
            GameObject currManager = Instantiate(manager, Vector3.zero, Quaternion.identity);
            IInitialisable init = currManager.GetComponent<IInitialisable>();
            if (init != null) init.Init();


        }

    }

    private IEnumerator SetUpGameScene()
    {
     
        if(!_roomManager)
        {
            if (RoomManagerPrefab)
            {
                _roomManager = Instantiate(RoomManagerPrefab, transform).GetComponent<RoomManager>();
                if (!_roomManager)
                {
                    Debug.LogError("No room manager reference --- Check Prefab");
                }
            }
                
        }


        //Loading SpawnRoom
        _isWaiting = true;
        _roomManager.OnRoomLoadComplete += OnWaitComplete;
        _roomManager.BeginRoomLoad(SceneIndex.SpawnRoom, Vector3.zero);
        while (_isWaiting)
        {
            yield return null;
        }

        //Loading Hub Corridor
        _spawnPoint = GameObject.Find("SpawnPoint").transform ;
        Vector3 attachPoint = Vector3.zero;
          //Loading SpawnRoom
          _isWaiting = true;

        if (_roomManager.GetLastRoom())
        {
            attachPoint = _roomManager.GetLastRoom().GetConnectingPoint();
        }
        _roomManager.BeginRoomLoad(SceneIndex.EntranceCorridor, attachPoint);
        while (_isWaiting)
        {
            yield return null;
        }


        //Loading Hub room
        _isWaiting = true;

        if (_roomManager.GetLastRoom())
        {
            attachPoint = _roomManager.GetLastRoom().GetConnectingPoint();
        }
        _roomManager.BeginRoomLoad(SceneIndex.HubRoom, attachPoint);
        while (_isWaiting)
        {
            yield return null;
        }
        _roomManager.OnRoomLoadComplete -= OnWaitComplete;

        StartCoroutine(SetUpPlayer());
    }

    private IEnumerator SetUpPlayer()
    {

        if (!_sceneManager)
        {
            if (GameStateManager.instance)
         
            {
              
                if (!GameStateManager.instance.SceneManager)
                {
                    Debug.LogError("No room manager reference --- Check Prefab");
                }
                else
                {
                    _sceneManager = GameStateManager.instance.SceneManager;
                }
            }

        }
        _isWaiting = true;
        _sceneManager.OnSceneAdded += OnWaitComplete;
        _sceneManager.AddNewScene(SceneIndex.PlayerScene);
        while (_isWaiting)
        {
            yield return null;
        }


        _player = GameObject.FindGameObjectWithTag("Player");

        if (!_player)
        {
            Debug.LogError("No player found reference --- Check if scene is loaded");

        }
        _player.transform.position = _spawnPoint.position;

        _player.GetComponent<PlayerBehaviour>().Init();
        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped += OnPlayerHasEquippedWeapon;
        }

        _isWaiting = true;
        _sceneManager.AddNewScene(SceneIndex.HUDscene);
        while (_isWaiting)
        {
            yield return null;
        }
        _HUDManager = FindObjectOfType<HUDManager>();
        _sceneManager.OnSceneAdded -= OnWaitComplete;

        GameStateManager.instance.BeginNewState(GameState.GameSceneSetUpComplete);
    }

    public RoomManager GetRoomManager()
    {
        return _roomManager;
    }

    private void OnWaitComplete()
    {
        _isWaiting = false;
    }


    public HUDManager HUDManager
    {
        get
        {
            return _HUDManager;
        }
    }

    public void OnPlayerHasEquippedWeapon(string weapon)
    {
        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped -= OnPlayerHasEquippedWeapon;
        }

        BeginNewGameplayEvent(GameplayEvents.WeaponSelected);
    }

    public GameplayEvents Event { get {return _currentGameplayEvent; } }
}