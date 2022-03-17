using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Crypt,
 
    Corridor
};

public class Room : MonoBehaviour
{
    //Dimensions

    [SerializeField] private int _width, _length;

    [SerializeField] private Vector3 _offset;
  
    [SerializeField] private Transform _origin;

    private List<Door> _doors = new List<Door>();


    [SerializeField] private bool _useGridOffset = false;

    private bool _drawDebug;
    private RoomInfo _roomInfo;

    [SerializeField] private int _nCrawlersPresent=0;
    private void OnDrawGizmos()
    {
        if (!_origin) return;
        Vector3 centre = _origin.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(centre+ _offset, new Vector3(_width, 5f, _length));

        if (_drawDebug)
        {

            ExtDebug.DrawBoxCastBox(transform.position, new Vector3(_width / 2 - 0.5f, 2.5f, _length / 2 - 0.5f), transform.rotation,
                Vector3.up,2.5f, Color.cyan) ;
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
        if(RoomManager._instance) RoomManager._instance.OnNewRoomLoaded(this);
    }


    public bool IsOverlapping(LayerMask overLapLayers)
    {
        if(_roomInfo._weight!=0)
        {
            
            RaycastHit[] hits;


            hits = Physics.BoxCastAll(transform.position , new Vector3(_width / 2-0.5f, 2.5f, _length / 2-0.5f), Vector3.up, transform.rotation, 2.5f, overLapLayers);
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
}