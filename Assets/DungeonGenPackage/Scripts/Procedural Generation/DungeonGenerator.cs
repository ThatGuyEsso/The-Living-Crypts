using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private bool _inDebug;


    [Header("Prefab ")]
    [SerializeField] private GameObject ObstacleManagerPrefab;
    [SerializeField] private GameObject DecorationManagerPrefab;
    [Header("Cached References")]
    [SerializeField] private DungeonGenData _genData;
    [SerializeField] private List<DungeonBuilder> _builders = new List<DungeonBuilder>();

    [Header("Perfromance settings")]
    [SerializeField] private float TimeBetweenSteps=0.05f;

    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private Room _debugStartingRoom;
    [SerializeField] private PropGenerationManager _obstacleGenerator;
    [SerializeField] private PropGenerationManager _litterGenerator;
    private bool _canSpawnDungeon;
    private bool _isWaitingForRoom;
    private int _buildersCompleteCount = 0;
    private int _currentBuilderIndex =0;
    private Direction _initDirection;
    RoomInfo _initRoomInfo;

    public System.Action OnFirstBuilderDone;
    public System.Action OnDungeonGenerationComplete;
    public System.Action OnBossRoomBuilt;
    public System.Action OnBossRoomFailedToBuild;
    public System.Action OnSpecialRoomsBuilt;
    public System.Action OnDungeonComplete;

    private void Awake()
    {
        if (_inDebug) Init();
    }
    public void Init()
    {
      
        
        _canSpawnDungeon = true;
        _buildersCompleteCount = 0;
        //Set random seed
        Random.InitState(System.DateTime.Now.Hour * 365000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Millisecond);
     
     
    }
    public void BeginDungeonGeneration(Room startingRoom, Direction startDirection, RoomManager roomManager)
    {
        _roomManager = roomManager;
        if (_canSpawnDungeon && _roomManager)
        {
            _canSpawnDungeon = false;
            _initDirection = startDirection;
            SpawnInitCorridor(startingRoom, startDirection);
        }
    }
    public void BeginDungeonGeneration(Room startingRoom, Direction startDirection)
    {
        if (_canSpawnDungeon&& _roomManager)
        {
            _canSpawnDungeon = false;
            _initDirection = startDirection;
            SpawnInitCorridor(startingRoom, startDirection);
        }
    }
    public void DebugBeginDungeonGeneration( Direction startDirection)
    {
        if (_canSpawnDungeon && _roomManager)
        {
            _canSpawnDungeon = false;
            _initDirection = startDirection;
            SpawnInitCorridor(_debugStartingRoom, startDirection);
        }
    }

    public void BeginBuildDungeon()
    {
        _roomManager.OnRoomLoadComplete -= BeginBuildDungeon;
        Room currRoom = _roomManager.GetLastRoom();
        currRoom.SetRoomInfo(_initRoomInfo);
        InitCrawlers(_genData, _initDirection);
     
    }
    private void SpawnInitCorridor(Room startingRoom, Direction startDirection)
    {
     
        if (!_roomManager)
        {
            if (GameStateManager.instance)
            {
                if (!GameStateManager.instance.GameManager)
                {
                    return;
                }
                _roomManager = GameStateManager.instance.GameManager.GetRoomManager();

                if (!_roomManager)
                {
                    Debug.LogError("No room manager reference --- Check Prefab");
                    return;
                }
            }
        }
        
            _roomManager.OnRoomLoadComplete += BeginBuildDungeon;
            List<Door> targetDoors = startingRoom.GetDoorsInDirection(startDirection);
            Door targetDoor = null;
            if (targetDoors.Count>0)
            {
                targetDoor = targetDoors[Random.Range(0, targetDoors.Count)];
            }


            if (targetDoor)
            {

                Vector3 spawnPoint = targetDoor.GetRoomSpawnPoint();
                switch (startDirection)
                {
                case Direction.North:
                    _roomManager.BeginRoomLoad(SceneIndex.N_StandardCorridor, targetDoor.GetRoomSpawnPoint());
                    _initRoomInfo = new RoomInfo(SceneIndex.N_StandardCorridor, 1, startDirection, RoomType.Corridor);
                    break;
                case Direction.South:
                    _roomManager.BeginRoomLoad(SceneIndex.S_StandardCorridor, targetDoor.GetRoomSpawnPoint());
                    _initRoomInfo = new RoomInfo(SceneIndex.S_StandardCorridor, 1, startDirection, RoomType.Corridor);
                    break;
                case Direction.West:
                    _roomManager.BeginRoomLoad(SceneIndex.W_StandardCorridor, targetDoor.GetRoomSpawnPoint());
                    _initRoomInfo = new RoomInfo(SceneIndex.W_StandardCorridor, 1, startDirection, RoomType.Corridor);
                    break;
                case Direction.East:
                    _roomManager.BeginRoomLoad(SceneIndex.E_StandardCorridor, targetDoor.GetRoomSpawnPoint());
                    _initRoomInfo = new RoomInfo(SceneIndex.E_StandardCorridor, 1, startDirection, RoomType.Corridor);
                    break;
            }
               
            }
        
    }
    public void InitCrawlers(DungeonGenData data, Direction startDirection)
    {
     
        for (int i = 0; i < data._numberOfCrawlers; i++)
        {
            DungeonBuilder newBuilder = new GameObject("Dungeon Builder"+ _builders.Count).AddComponent<DungeonBuilder>();

            newBuilder.InitBuilder(data, startDirection, _roomManager.GetLastRoom(),Random.Range(data._minSteps,data._maxSteps+1), _roomManager,this);

            _builders.Add(newBuilder);
        }

        BuilderNTakeStep();
    }

    public void GenComplete()
    {
        _buildersCompleteCount++;

        if(_buildersCompleteCount == 1)
        {
            OnFirstBuilderDone?.Invoke();
        }
        //Debug.Log("Builder Complete"+ _buildersCompleteCount);
        if (_buildersCompleteCount >= _builders.Count)
        {
            OnDungeonGenerationComplete?.Invoke();
            if (_roomManager)
            {
                _roomManager.TidyLoadedRoomList();
            }
            _currentBuilderIndex = 0;
            PlaceBossRoom();
        }
        else
        {
            //Debug.Log("Builder"+ _currentBuilderIndex + "Done building but process is not");
            EvaluateCanBuild();
        }
    }


    public void CleanUp()
    {
        if (_builders.Count > 0)
        {

            foreach(DungeonBuilder builder in _builders)
            {
                Destroy(builder.gameObject);
            }
            _builders.Clear();
        }
    }
    private IEnumerator BuildDungeon()
    {
     
        for(int i =0; i< _builders.Count; i++)
        {
        
            if(!_builders[i].IsWalking())
                _builders[i].TakeStep();
                   
                
      

            while (_builders[i].IsWalking())
            {
                Debug.Log(i + " WAiting");
                yield return null;
            }
        }
        EvaluateCanBuild();


    }

    private void BuilderNTakeStep()
    {
        if (!_builders[_currentBuilderIndex].IsWalking())
        {
            Debug.Log("Take step " + _builders[_currentBuilderIndex].gameObject);
            _builders[_currentBuilderIndex].TakeStep();
            
        }
            
    }

    public void IncrementCurrentBuilderIndex()
    {
        _currentBuilderIndex++;
        if (_currentBuilderIndex >= _builders.Count)
        {
            _currentBuilderIndex = 0;
        }
    }
    public void EvaluateCanBuild()
    {
        if(_buildersCompleteCount < _builders.Count)
        {
            IncrementCurrentBuilderIndex();
            if (TimeBetweenSteps > 0f)
            {
                StartCoroutine(WaitBuilderNTakeStep());
            }
            else
            {
                BuilderNTakeStep();
            }
          
        }
        else Debug.Log("All builders complete");
    }

    public void ClearBuilders()
    {
        foreach(DungeonBuilder builder in _builders)
        {
            Destroy(builder.gameObject);
        }

        _builders.Clear();
    }

    private IEnumerator WaitBuilderNTakeStep()
    {
        yield return new WaitForSeconds(TimeBetweenSteps);
        if (!_builders[_currentBuilderIndex].IsWalking())
        {
            Debug.Log("Take step " + _builders[_currentBuilderIndex].gameObject);
            _builders[_currentBuilderIndex].TakeStep();

        }
    }
    public void PlaceBossRoom()
    {
        if (_builders[_currentBuilderIndex])
        {
            Debug.Log("Attempt to buid boss step " + _builders[_currentBuilderIndex].gameObject);
            _builders[_currentBuilderIndex].TryAndPlaceBossRoomCorridor();

        }
    }

    public void PlaceLootRoom()
    {
        if (_builders[_currentBuilderIndex])
        {
            Debug.Log("Attempt to buid boss step " + _builders[_currentBuilderIndex].gameObject);
            _builders[_currentBuilderIndex].TryAndPlaceLootRoom();

        }
        
      
    }



    public void TriedToBuildBossRoom(bool WasSuccessful)
    {
        if (WasSuccessful)
        {
            OnBossRoomBuilt?.Invoke();
            _roomManager.TidyLoadedRoomList();
            PlaceLootRoom();
            Debug.Log("Boss Room added");
        }
        else
        {
            if (_currentBuilderIndex >= _builders.Count)
            {
                OnBossRoomFailedToBuild?.Invoke();
                
                Debug.Log("Failed To Build Room");
            }
            else
            {
                IncrementCurrentBuilderIndex();
                PlaceBossRoom();
            }

        }

    }
    public void TriedToPlaceLootRoom()
    {
        if (_currentBuilderIndex >= _builders.Count-1)
        {
            OnSpecialRoomsBuilt?.Invoke();
            CleanUpDeadEnds();
            Debug.Log("Loot Rooms  added");
        }
        else
        {
            IncrementCurrentBuilderIndex();
            PlaceLootRoom();
        }

            
    }


    public void CleanUpDeadEnds()
    {
        CleanUp();
        _roomManager.TidyLoadedRoomList();
        List<Room> corridors = _roomManager.GetRoomsOfType(RoomType.Corridor);

        List<Room> deadEnds = new List<Room>();
        if (corridors != null && corridors.Count > 0)
        {
            deadEnds = GetDeadEnds(corridors);
        }
        else
        {
            RemoveRedudantDoors();
            return;
        }

        if (deadEnds != null && deadEnds.Count > 0)
        {
            StartCoroutine(RemoveRoomsOverTime(deadEnds));
        }
        else
        {
            RemoveRedudantDoors();
            return;
        }

    }
    public List<Room> GetDeadEnds(List<Room> corridors)
    {
        List<Room> deadEnds = new List<Room>();

        foreach (Room room in corridors)
        {
            List<Direction> directions = room.GetAvailableDirections();
            if (directions.Count > 0)
            {
                List<Door> doors = room.GetDoorsInDirection(directions[0]);
                if (doors.Count > 0)
                {
                    if (doors[0].GetLinkedRoom() == null)
                    {
                        deadEnds.Add(room);
                    }

                }
            }
         
        }


        return deadEnds;
    }




    private IEnumerator RemoveRoomsOverTime(List<Room> rooms)
    {
        foreach(Room room in rooms)
        {
            if (room)
            {
                _isWaitingForRoom = true;
                _roomManager.OnRoomUnloadComplete += OnRoomWaitComplete;
                _roomManager.BeginUnload(room.gameObject.scene);
                yield return null;
            }
       
        }
        CleanUpDeadEnds();
    }

    private void OnRoomWaitComplete()
    {
        _roomManager.OnRoomUnloadComplete -= OnRoomWaitComplete;
        _isWaitingForRoom = false;
    }

    public void RemoveRedudantDoors()
    {
        _roomManager.TidyLoadedRoomList();
        List<Room> rooms = _roomManager.GetRoomsOfType(RoomType.Crypt);

        if(rooms!=null && rooms.Count > 0)
        {
          
            foreach(Room room in rooms)
            {
                if (room)
                {
                    room.DisableRedudantDoors();
                }
            }
        }
        rooms = _roomManager.GetRoomsOfType(RoomType.LootCrypt);
        if (rooms != null && rooms.Count > 0)
        {

            foreach (Room room in rooms)
            {
                if (room)
                {
                    room.DisableRedudantDoors();
                }
            }
        }

        CleanUpBuilders();
    }

    public void SpawnObstacles()
    {
        if (!_obstacleGenerator)
        {
            if (ObjectPoolManager.instance)
            {
                _obstacleGenerator = ObjectPoolManager.Spawn(ObstacleManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PropGenerationManager>();
            }
            else
            {
                _obstacleGenerator = Instantiate(ObstacleManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PropGenerationManager>();
            }
        }


        if (_obstacleGenerator)
        {
            List<Room> BossRooms = _roomManager.GetRoomsOfType(RoomType.BossCrypt);
            List<Room> rooms = _roomManager.GetRoomsOfType(RoomType.Crypt);
            foreach (Room BossRoom in BossRooms)
            {
                rooms.Add(BossRoom);
            }
            if (rooms.Count == 0)
            {
                return;
            }
            _obstacleGenerator.OnObstaclesSpawned += OnObstacleSpawnComplete;
            _obstacleGenerator.BeginFillRooms(rooms);

        }
    }

    public void SpawnLitter()
    {
        if (!_litterGenerator)
        {
            if (ObjectPoolManager.instance)
            {
                _litterGenerator = ObjectPoolManager.Spawn(DecorationManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PropGenerationManager>();
            }
            else
            {
                _litterGenerator = Instantiate(DecorationManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PropGenerationManager>();
            }
        }


        if (_litterGenerator)
        {
            List<Room> BossRooms = _roomManager.GetRoomsOfType(RoomType.BossCrypt);
            List<Room> lootRooms = _roomManager.GetRoomsOfType(RoomType.LootCrypt);
            List<Room> corridors = _roomManager.GetRoomsOfType(RoomType.Corridor);
            List<Room> rooms = _roomManager.GetRoomsOfType(RoomType.Crypt);
            foreach (Room BossRoom in BossRooms)
            {
                rooms.Add(BossRoom);
            }
            foreach (Room corridor in corridors)
            {
                rooms.Add(corridor);
            }
            foreach (Room loot in lootRooms)
            {
                rooms.Add(loot);
            }
            if (rooms.Count == 0)
            {
                return;
            }
            _litterGenerator.OnObstaclesSpawned += OnLitterSpawnComplete;
            _litterGenerator.BeginFillRooms(rooms);

        }
    }
    public void OnObstacleSpawnComplete()
    {
        if (_obstacleGenerator)
        {
            _obstacleGenerator.OnObstaclesSpawned -= OnObstacleSpawnComplete;
        }
        SpawnLitter();
    }
    public void OnLitterSpawnComplete()
    {
        if (_litterGenerator)
        {
            _litterGenerator.OnObstaclesSpawned -= OnLitterSpawnComplete;
        }
        OnDungeonComplete?.Invoke();
    }
    public void CleanUpBuilders()
    {
        if (_builders.Count > 0)
        {
            foreach(DungeonBuilder builder in _builders)
            {
                if (builder)
                {
                    Destroy(builder.gameObject);
                }
            }
        }
        SpawnObstacles();
    }
}