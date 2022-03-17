using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    public static WorldGridManager _instance;
    [SerializeField] private GameObject _cellDebugTextPrefab;
    [SerializeField] private bool _inDebug;
    [SerializeField] private bool _showDebug;
    [Header("Grid Settings")]
    [SerializeField] private float _cellSize;


    [SerializeField] private List<Grid2D<GridObject>> _grids = new List<Grid2D<GridObject>>();


    private void Awake()
    {
        if (_inDebug) Init();


    }


    public void Init()
    {
        if (!_instance)
        {
            _instance = this;
      
        }
        else
        {
            Destroy(this);
        }
    }

    
    public void AddNewGrid(Grid2D<GridObject> grid)
    {
        if (grid != null) _grids.Add(grid);
        if (_showDebug)
        {
            {
           
                foreach (GridObject obj in grid.GetGridObjectArray())
                {
                
                    obj.ShowDebugAboveObject();
                    obj.ShowDebugCoords();
                }
            }
        }
        grid.ShowDebugCells();
    }
    public float GridCellSize() { return _cellSize; }
    public List<Grid2D<GridObject>> GetGrids() { return _grids; }

    private void LateUpdate()
    {
        //if (_inDebug)
        //{
        //    if (_worldGrid!=null) _worldGrid.ShowDebugCells();
            
        //}
    }



}
