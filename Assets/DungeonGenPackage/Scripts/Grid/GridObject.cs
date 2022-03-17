using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[System.Serializable]
public class GridObject
{
    private int _x;
    private int _y;
    private Vector2Int _offset;

    private TextMeshPro _debugText;
    private TextMeshPro _debugObjectName;
    private Grid2D<GridObject> _grid;
    private GameObject _aboveObject = null;
    public GridObject(int x, int y, Grid2D<GridObject> grid)
    {
        _x = x;
        _y = y;
        _grid = grid;
        _offset = _grid.GetGridOffset();

    }

    public void SetOffset(Vector2Int offset){
        _offset = offset;
    }

    public void ShowDebugCoords()
    {
        if (_debugText)
        {
            if(!_debugText.gameObject.activeInHierarchy) _debugText.gameObject.SetActive(true);
        }
        _debugText = CreateWorldText.NewWorldText(null,
            _grid.GetWorldPosition(_x-_offset.x, _y-_offset.y) + new Vector3(1f, -0.5f, 1f) *
            _grid.GetCellSize() / 2f, 4, "[" + _x + "," + _y + "]", Color.white, 0, TMPro.TextAlignmentOptions.Center);
        _debugText.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    
  
    }


    public void ShowDebugAboveObject()
    {
        //if (_debugObjectName)
        //{
        //    if (!_debugObjectName.gameObject.activeInHierarchy) _debugObjectName.gameObject.SetActive(true);
        //}
        //if (!_aboveObject) return;
        //_debugObjectName = CreateWorldText.NewWorldText(null,
        //     _grid.GetWorldPosition(_x - _offset.x, _y - _offset.y) + new Vector3(1f, 0f, 1f) *
        //    _grid.GetCellSize() / 2f, 4, _aboveObject.name, Color.red, 0, TMPro.TextAlignmentOptions.Center);
        //_debugObjectName.transform.rotation = Quaternion.Euler(90f, 0f, 0f);


    }

    public void SetAboveObject(GameObject go)
    {
        _aboveObject = go;
        if (_aboveObject)
        {
            if (_debugObjectName)
            {
                if (!_debugObjectName.gameObject.activeInHierarchy) _debugObjectName.gameObject.SetActive(true);
                _debugObjectName.text = _aboveObject.name;
            }
            else
            {
                ShowDebugAboveObject();
            }
        }
        else{
            if (_debugObjectName)
            {
                if (!_debugObjectName.gameObject.activeInHierarchy) _debugObjectName.gameObject.SetActive(true);
                _debugObjectName.text = "";
            }
        }
    }

    public Vector2Int GetCellCoordinate()
    {
        return new Vector2Int(_x - _offset.x, _y - _offset.y);
    }
    public GameObject GetAboveObject()
    {
        return _aboveObject;
    }

    public bool IsOccupied()
    {
        return _aboveObject == true;
    }
    public void HideDebugCoords()
    {
        if (_debugText) _debugText.gameObject.SetActive(false);
    }
}
