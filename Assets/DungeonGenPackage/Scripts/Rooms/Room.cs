using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Crypt,
 
    Corridor,

    BossCrypt
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
    private RoomInfo _roomInfo;

    [SerializeField] private int _nCrawlersPresent=0;
    private void OnDrawGizmos()
    {
        if (!_origin) return;
        Vector3 centre = _origin.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(centre+ _offset, new Vector3(_width, _height, _length));

        if (_drawDebug)
        {

            ExtDebug.DrawBoxCastBox(transform.position + Vector3.up * -1 , new Vector3(_width / 2f - 0.5f, _height / 2f - 0.5f, _length / 2f - 0.5f), Quaternion.identity,
                Vector3.up, _height / 2f, Color.cyan) ;
        }
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
            for(int i = 0; i < doors.Length; i++)
            {
                _doors.Add(doors[i]);
                doors[i].Init();
            
            }
        }
    }

    public void SetRoomInfo(RoomInfo info)
    {
        _roomInfo = info;
    
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

    }


    public bool IsOverlapping(LayerMask overLapLayers)
    {
        if(_roomInfo._weight!=0)
        {
            
            RaycastHit[] hits;


            hits = Physics.BoxCastAll(transform.position+ Vector3.up*-1 , new Vector3(_width / 2f- 0.5f, _height/2f-0.5f, _length / 2f- 0.5f), Vector3.up,Quaternion.identity, _height/2f, overLapLayers);
            _drawDebug = true;
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.transform.parent != transform.parent)
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

        for(int i =0; i<_doors.Count; i++)
        {
            if(dirs.Count == 0& !_doors[i].IsEntry())
            {
                dirs.Add(_doors[i].GetDirection());
            }
            else
            {
                if (!dirs.Contains(_doors[i].GetDirection())&& !_doors[i].IsEntry())
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
        for(int i=0; i < _doors.Count; i++)
        {
            if (_doors[i].GetDirection()== direction&&!_doors[i].IsEntry())
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
    
    public void IncrementCrawlers()
    {
        _nCrawlersPresent++;
    }
    public void DecrementCrawlers()
    {
        
        _nCrawlersPresent--;
        if(_nCrawlersPresent <0 )_nCrawlersPresent=0;
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
}