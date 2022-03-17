using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{ 
    [SerializeField] private bool _inDebug;
    [SerializeField] private GameObject _entryDebugPrefab;
    [SerializeField] private GameObject _exitDebugPrefab;
    [SerializeField] private GameObject _debugVisual;
    [Header("Door Settings")]
    [SerializeField] private Transform _roomSpawnPoint;
    [SerializeField] private bool _isEntry;
    [SerializeField] private Direction _faceDirection;
    [SerializeField] private float _width, _length,_height;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _debugOffset;
    [SerializeField] private Vector2Int _rootCell;
    //Object References
    private Room _parentRoom;
    private Room _linkedRoom;
    bool _isInitialised;
    public void Init()
    {
      
      
        _isInitialised = true;
        if (_inDebug) SpawnDebugVisual();

    }
    public void RegisterOnGrid(Grid2D<GridObject> grid)
    {

        if (_inDebug) SpawnDebugVisual();
    }

    private void OnDrawGizmos()
    {
        if (!_inDebug) return;
        Vector3 centre = transform.position +_offset;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(centre, new Vector3(_width, _height, _length));


    }
 
    //public void GetTargetCells()
    //{
    //    if (_occupiedCells.Count > 0)
    //    {
    //        switch (_faceDirection)
    //        {
    //            case GridDirection.North:
    //                for (int i= 0; i < _occupiedCells.Count;i++ ){
    //                    _targetCells.Add(new Vector2Int(_occupiedCells[i].x, _occupiedCells[i].y + 1));
    //                }
    //            break;
    //            case GridDirection.South:
    //                for (int i = 0; i < _occupiedCells.Count; i++)
    //                {
    //                    _targetCells.Add(new Vector2Int(_occupiedCells[i].x, _occupiedCells[i].y - 1));
    //                }
    //                break;
    //            case GridDirection.West:
    //                for (int i = 0; i < _occupiedCells.Count; i++)
    //                {
    //                    _targetCells.Add(new Vector2Int(_occupiedCells[i].x-1, _occupiedCells[i].y));
    //                }
    //                break;
    //            case GridDirection.East:
    //                for (int i = 0; i < _occupiedCells.Count; i++)
    //                {
    //                    _targetCells.Add(new Vector2Int(_occupiedCells[i].x + 1, _occupiedCells[i].y));
    //                }
    //                break;
    //        }
    //    }
  
    //}
    public Vector2Int GetRootCell() { return _rootCell; }
    public void SpawnDebugVisual()
    {

        if (_isEntry)
        {
            _debugVisual=Instantiate(_entryDebugPrefab, _roomSpawnPoint.position + transform.right*0.5f+_debugOffset, transform.rotation);
        }
        else
        {
            _debugVisual=Instantiate(_exitDebugPrefab, _roomSpawnPoint.position  + transform.right*0.5f + _debugOffset, transform.rotation);
        }
    }

    public Direction GetDirection() { return _faceDirection; }
    public bool IsEntry() { return _isEntry; }

    //public List<Vector2Int> GetCoordinateOfTargetCells() { return _targetCells; }
    public Vector3 GetRoomSpawnPoint()
    {
        return _roomSpawnPoint.position;
    }

    public void SetLinkedRoom(Room link) { _linkedRoom = link; }
    public Room GetLinkedRoom( ) { return _linkedRoom; }

    private void OnDestroy()
    {
        if (_debugVisual) Destroy(_debugVisual);
    }
}
