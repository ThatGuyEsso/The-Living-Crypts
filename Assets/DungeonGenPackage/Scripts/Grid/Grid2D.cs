using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Direction
{
    North,
    South,
    West,
    East
};
//Grid can be used in a 3D or 2D enviroment but grid only has 2 dimensions
[System.Serializable]
public class Grid2D<TGridObj>
{

    //Grid Dimenstions
    private int _width;

    private int _length;
    private Vector2Int _cellOffset;
    //Cell Dimensions
    private float _cellSize=0f;
    private Vector3 _originPositon;
    private TGridObj[,] _gridArray;
    public Grid2D(int width, int length, float gridCellSize, Vector3 origin)
    {
        _width = width;
        _length = length;
        _cellSize = gridCellSize;
        _originPositon = origin;
        _gridArray = new TGridObj[_width, _length];

    }
    public Grid2D(int width, int length, float gridCellSize, Vector3 origin, Func<int, int, Grid2D<TGridObj>, TGridObj> initGridObj)
    {
        _width = width;
        _length = length;
        _cellSize = gridCellSize;
        _originPositon = origin;
        _gridArray = new TGridObj[_width, _length];
       
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x, y] = initGridObj(x, y, this);
            }
        }
        _cellOffset = Vector2Int.zero;



    }
    public Grid2D(int width, int length,int initX,int initZ, float gridCellSize, Vector3 origin, Func<int, int, Grid2D<TGridObj>, TGridObj> initGridObj)
    {
        _width = width;
        _length = length;
        _cellSize = gridCellSize;
        _originPositon = origin;
        _gridArray = new TGridObj[_width, _length];
        _cellOffset = new Vector2Int(initX, initZ);
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                _gridArray[x, y] = initGridObj(initX+x, initZ+ y, this);
            
            }
        }


    }

    public Vector2Int GetGridOffset()
    {
        return _cellOffset;
    }

    public void InitGridObject(int x, int y, Func<int, int, Grid2D<TGridObj>, TGridObj> initGridObj)
    {
        TGridObj gridObj = initGridObj(x, y, this);
        _gridArray[x, y] = gridObj;



    }
    public void ShowDebugCells()
    {
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1);y++)
            {

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white,  1000f);

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white,  1000f);

            }
        }
        Debug.DrawLine(GetWorldPosition(0, _length), GetWorldPosition(_width, _length), Color.white,1000f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _length), Color.white,1000f);
    }

    //takes x and y position of cell and returns position in world
    public Vector3 GetWorldPosition(int x, int y)
    {
        return (new Vector3(x,0f, y) * _cellSize + _originPositon);
    }

    //Take position in world and converts to cell position
    private void GetWorldToGrid(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPositon).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPositon).z / _cellSize);


        if (x >= 0 && x < _width && y >= 0 &&y < _length)
        {
        }
        else
        {
            y = -1;
            y= -1;
        }

    }

    //Returns value from world position to grid position
    public Vector3 GetPositionOfCell(Vector3 worldPos)
    {
        int x, y;
        GetWorldToGrid(worldPos, out x, out y);

        if (x >= 0)
        {
            return GetWorldPosition(x, y);
        }
        else
        {
            return new Vector3(-0.1f, -0.1f);
        }

    }

    public TGridObj GetGridObject(int x, int y)
    {
        
        return  _gridArray[x, y];
    }

    public TGridObj GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetWorldToGrid(worldPos, out x, out y);
        return _gridArray[x, y];
    }

    public TGridObj[,] GetGridObjectArray()
    {
        return _gridArray;
    }
    public float GetCellSize() { return _cellSize; }
}
