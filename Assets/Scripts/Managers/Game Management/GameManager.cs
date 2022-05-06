using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum GameplayEvents
{
    WeaponSelected,
    DungeonInvoked,
    DungeonBegunGenerating,
    DungeonGenComplete,
    GameComplete,
    PlayerDied,
    PlayerRespawned,
    PlayerRespawnBegun,
    EnteredCombat,
    LeftCombat,
    Restart,
    ExitLevel,
    Quit,
    OnOBossSequenceBegun,
    OnBossFightBegun,
    OnBossKilled,
    OnBossFightEnd
};

public class GameManager : MonoBehaviour, IManager, IInitialisable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject RoomManagerPrefab;
    [SerializeField] private GameObject DungeonGenerationManagerPrefab;
    [Header("Skybox")]
    [SerializeField] private Material SkyBox;
    [Header("Lighting settings")]
    [SerializeField] private LightingSettings lightSettings;
    [Header("Post processing profiles")]
    [SerializeField] private VolumeProfile DeathProfile, GameProfile;
    private Volume _gameVolume;

    [Header("Timers")]
    [SerializeField] private float TimeToRespawn =3f;

    [Header("SFX")]
    [SerializeField] private string SpawnSFX,DeathSFX,InvokeDungeonSFX;

    [Header("Music")]
    [SerializeField] private string DungeonAmbience;
    [SerializeField] private string CombatSong;
    [SerializeField] private string BossSong;
    //Manager References
    private MusicManager _musicManager;
    private AudioManager _audioManager;
    private RoomManager _roomManager;
    private DungeonGenerator _generationManager;
    private HUDManager _HUDManager;
    private SceneTransitionManager _sceneManager;

    //Scene references
    private Transform _spawnPoint;

    //Player references
    private GameObject _player;

    //States
    private bool _isWaiting;

    //Events
    private GameplayEvents _currentGameplayEvent;
    public System.Action<GameplayEvents> OnNewGamplayEvent;
    private Room _hubRoom;
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


        switch (_currentGameplayEvent)
        {
            case GameplayEvents.DungeonInvoked:
                PlaySFX(InvokeDungeonSFX);
                InitDungeonGeneration();
                break;
            case GameplayEvents.PlayerDied:
                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                InitRespawn();
                if (ConsumableManager._instance)
                {
                    ConsumableManager._instance.Clear();
                }
                PlayDeathSequence();
                Invoke("InitRespawn", TimeToRespawn);
                break;

            case GameplayEvents.PlayerRespawnBegun:
                StartCoroutine(DoPlayerRespawn());
                break;

            case GameplayEvents.PlayerRespawned:

                   PlaySong(DungeonAmbience);
                break;
            case GameplayEvents.Restart:

                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                InitRespawn();
                if (ConsumableManager._instance)
                {
                    ConsumableManager._instance.Clear();
                }
                break;
            case GameplayEvents.ExitLevel:

                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                if (_sceneManager)
                {
                    _sceneManager.BeginLoadMenuScreen(SceneIndex.TitleScreen);
                }
                if (_generationManager)
                {
                    _generationManager.StopBuilding();
                }
                InitRespawn();
                if (ConsumableManager._instance)
                {
                    ConsumableManager._instance.Clear();
                }
                break;

            case GameplayEvents.OnBossFightEnd:

                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                if (_sceneManager)
                {
                    _sceneManager.BeginLoadMenuScreen(SceneIndex.TitleScreen);
                }
                break;
            case GameplayEvents.OnOBossSequenceBegun:
                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                break;
            case GameplayEvents.OnBossFightBegun:
                PlaySong(BossSong);
                break;
            case GameplayEvents.Quit:

                if (_musicManager)
                {
                    _musicManager.StopMusic();
                }
                if (!GameStateManager.instance || !GameStateManager.instance.LoadingScreenManager)
                {
                    Debug.LogError("No loading screen");
                    Application.Quit();
                    break;
                }
                GameStateManager.instance.LoadingScreenManager.OnFadeComplete += OnQuitGame;
                GameStateManager.instance.LoadingScreenManager.BeginFadeIn();
                if (_generationManager)
                {
                    _generationManager.StopBuilding();
                }
                break;
            case GameplayEvents.EnteredCombat:
                PlaySong(CombatSong);
                break;

            case GameplayEvents.LeftCombat:
                PlaySong(DungeonAmbience);
                break;
        }
    }
    public void OnQuitGame()
    {
        if (!GameStateManager.instance)
        {
            Debug.LogError("Can't quit from gamestatemanager");
            Application.Quit();
            return;
        }
        GameStateManager.instance.LoadingScreenManager.OnFadeComplete -= OnQuitGame;
        GameStateManager.instance.QuitGame();
    }
    public void Init()
    {
        if (GameStateManager.instance)
        {
            GameStateManager.instance.GameManager = this;
            BindToGameStateManager();
        }

        if (!_gameVolume)
        {
            _gameVolume = FindObjectOfType<Volume>();
           
        }

        if (_gameVolume)
        {
            _gameVolume.profile = GameProfile;
        }
    }
    public void InitGame()
    {
        RenderSettings.skybox = SkyBox;

        StartCoroutine(SetUpGameScene());
    }

    private void PlaySFX(string sfx)
    {
        if (!_audioManager)
        {
            _audioManager = AudioManager;
        }
        if (_audioManager)
        {
            _audioManager.PlayThroughAudioPlayer(sfx, _player.transform.position);
        }
    }

    private void PlaySong(string song)
    {
        if (!_musicManager)
        {
            _musicManager = MusicManager;
        }
        if (_musicManager)
        {
            _musicManager.BeginSongFadeIn(song, 4, 6, 8);
        }
    }
    public void PlayDeathSequence()
    {
        if (!_gameVolume)
        {
            _gameVolume = FindObjectOfType<Volume>();

        }

        if (_gameVolume)
        {
            _gameVolume.profile = DeathProfile;
        }

        PlaySFX(DeathSFX);
    
    }


    private void InitRespawn()
    {
        if(!GameStateManager.instance || !GameStateManager.instance.LoadingScreenManager)
        {
            Debug.LogError("No loading screen");
            return;
        }

        GameStateManager.instance.LoadingScreenManager.OnFadeComplete += BeginRespawn;

        GameStateManager.instance.LoadingScreenManager.BeginFadeIn();
    }


    private void BeginRespawn()
    {
        if (!GameStateManager.instance || !GameStateManager.instance.LoadingScreenManager)
        {
            Debug.LogError("No loading screen");
            return;
        }
        GameStateManager.instance.LoadingScreenManager.OnFadeComplete -= BeginRespawn;

        BeginNewGameplayEvent(GameplayEvents.PlayerRespawnBegun);

        
    }


    private IEnumerator DoPlayerRespawn()
    {
        //Unload Dungeon
        List<Room> rooms = _roomManager.GetRoomsOfType(RoomType.Crypt);
        _roomManager.OnRoomUnloadComplete += OnWaitComplete;

        //unload crypts
        if (rooms.Count > 0)
        {
            foreach (Room room in rooms)
            {
                _isWaiting = true;
                _roomManager.BeginRemoveRoom(room);
                while (_isWaiting)
                {
                   yield return null;
                }
            }
       
        }
        //unload corridors
        rooms = _roomManager.GetRoomsOfType(RoomType.Corridor);
        if (rooms.Count > 0)
        {
            foreach (Room room in rooms)
            {
                _isWaiting = true;
                _roomManager.BeginRemoveRoom(room);
                while (_isWaiting)
                {
                    yield return null;
                }
            }

        }
        //Loot crypts
        rooms = _roomManager.GetRoomsOfType(RoomType.LootCrypt);
        if (rooms.Count > 0)
        {
            foreach (Room room in rooms)
            {
                _isWaiting = true;
                _roomManager.BeginRemoveRoom(room);
                while (_isWaiting)
                {
                    yield return null;
                }
            }

        }

        //Boss Crypt
        rooms = _roomManager.GetRoomsOfType(RoomType.BossCrypt);
        if (rooms.Count > 0)
        {
            foreach (Room room in rooms)
            {
                _isWaiting = true;
                _roomManager.BeginRemoveRoom(room);
                while (_isWaiting)
                {
                    yield return null;
                }
            }

        }
        _player.transform.position = _spawnPoint.position;
        _player.transform.rotation = _spawnPoint.rotation;
        PlayerBehaviour player = _player.GetComponent<PlayerBehaviour>();
        if (player)
        {
            player.ResetCharacter();
        }
        if (WeaponManager._instance)
        {
            WeaponManager._instance.ResetManager();
        }
      
        if (_gameVolume)
        {
            _gameVolume.profile = GameProfile;
        }
        if (WeaponManager._instance)
        {
            WeaponManager._instance.OnWeaponEquipped += OnPlayerHasEquippedWeapon;
        }


        BeginNewGameplayEvent(GameplayEvents.PlayerRespawned);
        PlaySFX(SpawnSFX);
     
        GameStateManager.instance.LoadingScreenManager.BeginFadeOut();

    }
    public void InitDungeonGeneration()
    {
        if (!_generationManager)
        {
            if (ObjectPoolManager.instance)
            {
                _generationManager = ObjectPoolManager.Spawn(DungeonGenerationManagerPrefab, transform).GetComponent<DungeonGenerator>();
            }
            else
            {
                _generationManager = Instantiate(DungeonGenerationManagerPrefab, transform).GetComponent<DungeonGenerator>();
            }
        }

        if (_generationManager)
        {
            _generationManager.Init();
            _generationManager.OnDungeonComplete += OnDungeonCompleted;
            _generationManager.BeginDungeonGeneration(_hubRoom, Direction.North,_roomManager);
            BeginNewGameplayEvent(GameplayEvents.DungeonBegunGenerating);
            
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

        Room currentRoom;
        //Loading SpawnRoom
        _isWaiting = true;
        _roomManager.OnRoomLoadComplete += OnWaitComplete;
        _roomManager.BeginRoomLoad(SceneIndex.SpawnRoom, Vector3.zero);
        while (_isWaiting)
        {
            yield return null;
        }
        currentRoom = _roomManager.GetLastRoom();
        if (currentRoom)
        {
            currentRoom.SetRoomInfo(new RoomInfo(SceneIndex.SpawnRoom, 0,Direction.North, RoomType.NonDungeonRoom));
            currentRoom.Init();
        }
        //Loading Hub Corridor
        _spawnPoint = GameObject.Find("SpawnPoint").transform ;
        Vector3 attachPoint = Vector3.zero;
          //Loading SpawnRoom
          _isWaiting = true;

        if (currentRoom)
        {
            attachPoint = currentRoom.GetConnectingPoint();
        }

        _roomManager.BeginRoomLoad(SceneIndex.EntranceCorridor, attachPoint);
        while (_isWaiting)
        {
            yield return null;
        }

        currentRoom = _roomManager.GetLastRoom();
        if (currentRoom)
        {
            currentRoom.SetRoomInfo(new RoomInfo(SceneIndex.EntranceCorridor, 0, Direction.North, RoomType.NonDungeonRoom));
            currentRoom.Init();
        }
        //Loading Hub room
        _isWaiting = true;

        if (currentRoom)
        {

            attachPoint = currentRoom.GetConnectingPoint();
        }
        _roomManager.BeginRoomLoad(SceneIndex.HubRoom, attachPoint);
        while (_isWaiting)
        {
            yield return null;
        }
        currentRoom = _roomManager.GetLastRoom();
        if (currentRoom)
        {
            currentRoom.SetRoomInfo(new RoomInfo(SceneIndex.HubRoom, 0, Direction.North, RoomType.NonDungeonRoom));
            currentRoom.Init();

        }
        _hubRoom = currentRoom;
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
        _player.transform.rotation = _spawnPoint.rotation;
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

        if (_HUDManager &&_player)
        {
            _HUDManager.Init(_player,this);
        }

        _sceneManager.AddNewScene(SceneIndex.PauseScreen);
        while (_isWaiting)
        {
            yield return null;
        }
        PlaySFX(SpawnSFX);
        PlaySong(DungeonAmbience);
        GameStateManager.instance.BeginNewState(GameState.GameSceneSetUpComplete);
    }

    public RoomManager GetRoomManager()
    {
        return _roomManager;
    }
    public DungeonGenerator GetGenerationManager()
    {
        return _generationManager;
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

    public void OnDungeonCompleted()
    {
        BeginNewGameplayEvent(GameplayEvents.DungeonGenComplete);
        if (!_generationManager)
        {
            return;
        }
        _generationManager.OnDungeonComplete -= OnDungeonCompleted;
    }

    public GameObject Player { get { return _player; } }

    public AudioManager AudioManager {
        get
        {
            if (_audioManager)
            {
                return _audioManager;
            }
            else
            {
                if(!GameStateManager.instance || !GameStateManager.instance.AudioManager)
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


    public MusicManager MusicManager
    {
        get
        {
            if (_musicManager)
            {
                return _musicManager;
            }
            else
            {
                if (!GameStateManager.instance || !GameStateManager.instance.MusicManager)
                {
                    return null;
                }
                else
                {
                    return GameStateManager.instance.MusicManager;
                }
            }
        }
    }
}
