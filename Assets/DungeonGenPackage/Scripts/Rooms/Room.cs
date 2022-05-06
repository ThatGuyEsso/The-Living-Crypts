using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Crypt,

    LootCrypt,

    Corridor,

    BossCrypt,

    NonDungeonRoom,
};

public class Room : MonoBehaviour
{
    [SerializeField] private bool _inDebug;
    //Dimensions

    [SerializeField] private int _width, _length, _height;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _connectingPoint;

    private List<Door> _doors = new List<Door>();

    private RoomManager _roomManager;
    [SerializeField] private bool _useGridOffset = false;

    private bool _drawDebug;
    [SerializeField] private RoomInfo _roomInfo;

    [SerializeField] private int _nCrawlersPresent = 0;

    [Header("Prefabs")]
    [SerializeField] private GameObject EnemySpawnManagerPrefab;
    [Header("SFX")]
    private string RoomClearedSFX = "PlayerSpawnSFX";
    private CryptEnemyManager enemyManager;
    protected AudioManager AM;
    public System.Action OnBossRoomTriggered;
    public GameManager GM;

    private void OnDrawGizmos()
    {
        if (!_origin) return;
        Vector3 centre = _origin.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(centre + _offset, new Vector3(_width, _height, _length));

        if (_drawDebug)
        {

            ExtDebug.DrawBoxCastBox(transform.parent.position + _offset, new Vector3(_width / 2f - 0.25f, _offset.y, _length / 2f - 0.25f), Quaternion.identity,
                Vector3.up, _height / 2f, Color.cyan);
        }
    }

    public void BeginRoomEncounter()
    {

        if (_doors.Count > 0)
        {
            foreach (Door door in _doors)
            {
                if (door)
                {
                    door.OnPlayerEnteredRoom -= BeginRoomEncounter;
                }
            }
        }
        switch (_roomInfo._roomType)
        {
            case RoomType.Crypt:

                BeginEnemyEcounter();

                break;

            case RoomType.LootCrypt:

                BeginLootEcounter();
                break;
            case RoomType.BossCrypt:
                BeginBossEnounter();


                break;
        }
    }


    public void BeginEnemyEcounter()
    {
        LockDoors();
        if (EnemySpawnManagerPrefab)
        {
            if (ObjectPoolManager.instance)
            {
                enemyManager = ObjectPoolManager.Spawn(EnemySpawnManagerPrefab.transform).GetComponent<CryptEnemyManager>();

            }
            else
            {
                enemyManager = Instantiate(EnemySpawnManagerPrefab.transform).GetComponent<CryptEnemyManager>();
            }

            if (enemyManager)
            {
                enemyManager.Init(this);
                enemyManager.Begin();
                enemyManager.OnEnemiesCleared += OnRoomCleared;
            }
        }
        if (GM)
        {
            GM.BeginNewGameplayEvent(GameplayEvents.EnteredCombat);
        }
    }

    public void BeginBossEnounter()
    {
        LockDoors();
        OnBossRoomTriggered?.Invoke();


    }
    public void OnRoomCleared()
    {
        if (enemyManager)
        {
            enemyManager.OnEnemiesCleared -= OnRoomCleared;
            UnlockDoors();
            PlaySFX(RoomClearedSFX, false);
        }
        if (GM)
        {
            GM.BeginNewGameplayEvent(GameplayEvents.LeftCombat);
        }

    }
    public void BeginLootEcounter()
    {

    }
    private void Awake()
    {
        if (_inDebug)
        {
            Init();
        }
    }
    public void Init()
    {

        Door[] doors = GetComponentsInChildren<Door>();


        if (doors != null)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                _doors.Add(doors[i]);
                doors[i].Init();
                doors[i].OnPlayerEnteredRoom += BeginRoomEncounter;
                if (_inDebug)
                {
                    doors[i].OpenDoor();
                }
            }


        }


    }

    public void SetRoomInfo(RoomInfo info)
    {
        _roomInfo = info;
        switch (_roomInfo._roomType)
        {
            case RoomType.Corridor:
                foreach (Door door in _doors)
                {
                    if (door.IsEntry())
                    {
                        door.OpenDoor();
                    }
                    else
                    {
                        door.ToggleDoorLock(false);
                    }
                }
                break;


            case RoomType.Crypt:
                foreach (Door door in _doors)
                {

                    door.OpenDoor();
                }
                break;
            case RoomType.LootCrypt:
                foreach (Door door in _doors)
                {

                    door.OpenDoor();
                }
                break;
            case RoomType.BossCrypt:
                foreach (Door door in _doors)
                {

                    door.OpenDoor();
                }
                break;
        }

    }
    public RoomInfo GetRoomInfo()
    {
        return _roomInfo;
    }

    private void OnEnable()
    {
        if (FindObjectOfType<DebugController>())
        {
            if (!_roomManager)
            {
                _roomManager = FindObjectOfType<RoomManager>();
                _roomManager.OnNewRoomLoaded(this);
            }

            if (!_roomManager)
            {
                Debug.LogError("No Room Manager");
                return;
            }
        }
        if (!GameStateManager.instance) return;
        if (!GameStateManager.instance.GameManager) return;
        if (!GameStateManager.instance.GameManager.GetRoomManager()) return;
        GameStateManager.instance.GameManager.GetRoomManager().OnNewRoomLoaded(this);
        if (!GM)
        {
            if (GameStateManager.instance && GameStateManager.instance.GameManager)
            {
                GM = GameStateManager.instance.GameManager;
            }
        }
    }

    public bool IsOverlapping(LayerMask overLapLayers)
    {


        RaycastHit[] hits;


        hits = Physics.BoxCastAll(transform.parent.position + _offset + Vector3.up * -1f,
            new Vector3(_width / 2f - 0.25f, _offset.y, _length / 2f - 0.25f), Vector3.up, Quaternion.identity, _height / 2f, overLapLayers);
        _drawDebug = true;

        Debug.Log(transform.parent.name);
        if (hits.Length > 0)
        {

            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject);
                if (hit.transform.parent)
                {

                    if (hit.collider.transform.parent != transform.parent)
                    {

                        return true;
                    }
                }
                else
                {
                    if (hit.collider.gameObject != transform.parent)
                    {
                        return true;
                    }
                }

            }
        }



        return false;
    }

    private float GetMaxRaycastLength()
    {
        if (_roomInfo._roomDirection == Direction.East || _roomInfo._roomDirection == Direction.West)
            return _width;
        else return _length;
    }


    private Vector3 GetRaycastDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return transform.forward;
            case Direction.South:
                return -transform.forward;
            case Direction.West:
                return -transform.right;
            case Direction.East:
                return transform.right;

        }
        return transform.forward;
    }

    public List<Direction> GetAvailableDirections()
    {
        List<Direction> dirs = new List<Direction>();

        if (_doors.Count <= 0) return dirs;

        for (int i = 0; i < _doors.Count; i++)
        {
            if (dirs.Count == 0 & !_doors[i].IsEntry())
            {
                dirs.Add(_doors[i].GetDirection());
            }
            else
            {
                if (!dirs.Contains(_doors[i].GetDirection()) && !_doors[i].IsEntry())
                {
                    dirs.Add(_doors[i].GetDirection());
                }
            }
        }
        return dirs;
    }

    public List<Door> GetDoorsInDirection(Direction direction)
    {
        List<Door> doors = new List<Door>();

        if (doors.Count > 0) return doors;
        for (int i = 0; i < _doors.Count; i++)
        {
            if (_doors[i].GetDirection() == direction && !_doors[i].IsEntry())
            {
                doors.Add(_doors[i]);
            }
        }
        return doors;
    }


    public Vector2Int GetRoomExtents()
    {
        return new Vector2Int(_width, _length);
    }
    public Vector2Int GetRoomHalfExtents()
    {
        return new Vector2Int(_width/2, _length/2);
    }

    public void IncrementCrawlers()
    {
        _nCrawlersPresent++;
    }
    public void DecrementCrawlers()
    {

        _nCrawlersPresent--;
        if (_nCrawlersPresent < 0) _nCrawlersPresent = 0;
    }
    public int GetNCrawlers()
    {
        return _nCrawlersPresent;
    }

    public int GetDoorCount()
    {
        return _doors.Count;
    }
    public Vector3 GetConnectingPoint()
    {
        if (_connectingPoint)
        {
            return _connectingPoint.position;
        }
        else
        {
            return transform.position;
        }

    }

    public void DisableRedudantDoors()
    {
        if (_doors.Count > 0)
        {
            foreach (Door door in _doors)
            {
                if (!door.GetLinkedRoom() && !door.IsEntry())
                {
                    door.DisableDoor();
                }
            }
        }
    }

    public void LockDoors()
    {
        if (_doors.Count > 0)
        {
            foreach(Door door in _doors)
            {
                door.ToggleDoorLock(true);
                door.CloseDoor();
            }
        }
    }

    public void UnlockDoors()
    {
        if (_doors.Count > 0)
        {
            foreach (Door door in _doors)
            {
                door.ToggleDoorLock(false);
               
            }
        }
    }

    private void OnDisable()
    {
        if (_doors.Count > 0)
        {
            foreach (Door door in _doors)
            {
                if (door)
                {
                    door.OnPlayerEnteredRoom -= BeginRoomEncounter;
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (_doors.Count > 0)
        {
            foreach (Door door in _doors)
            {
                if (door)
                {
                    door.OnPlayerEnteredRoom -= BeginRoomEncounter;
                }
            }
        }
    }


    public virtual AudioPlayer PlaySFX(string sfxName, bool randPitch)
    {
        if (AM)
        {
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }

            AM = GameStateManager.instance.AudioManager;
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
    }
}