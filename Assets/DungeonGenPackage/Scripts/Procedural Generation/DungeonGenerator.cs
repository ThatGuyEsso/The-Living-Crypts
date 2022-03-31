using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private bool _inDebug;

    [Header("Cached References")]
    [SerializeField] private DungeonGenData _genData;
    [SerializeField] private List<DungeonBuilder> _builders = new List<DungeonBuilder>();


    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private Room _debugStartingRoom;
    private bool _canSpawnDungeon;
    private int _buildersCompleteCount = 0;
    private int _currentBuilderIndex =0;
    private Direction _initDirection;
    RoomInfo _initRoomInfo;

    private void Awake()
    {
        if (_inDebug) Init();
    }
    public void Init()
    {
      
        
        _canSpawnDungeon = true;

        //Set random seed
        Random.InitState(System.DateTime.Now.Hour * 365000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Millisecond);
     
     
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
        Debug.Log("Builder Complete"+ _buildersCompleteCount);
        if (_buildersCompleteCount >= _builders.Count)
        {
            Debug.Log("Generation Complete");
            ClearBuilders();
        }
        else
        {
            Debug.Log("Builder"+ _currentBuilderIndex + "Done building but process is not");
            EvaluateCanBuild();
        }
    }


    public void CleanUp()
    {
        foreach(DungeonBuilder builder in _builders)
        {
            Destroy(builder.gameObject);
        }
        _builders.Clear();
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
        if (_currentBuilderIndex >= _builders.Count) _currentBuilderIndex = 0;
    }
    public void EvaluateCanBuild()
    {
        if(_buildersCompleteCount < _builders.Count)
        {
            IncrementCurrentBuilderIndex();
            BuilderNTakeStep();
            //Invoke("BuilderNTakeStep", 0.1f);
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
}