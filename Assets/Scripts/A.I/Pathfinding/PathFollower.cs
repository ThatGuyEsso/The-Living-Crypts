using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PathFollower : MonoBehaviour
{
    [SerializeField] private int _currentCornerIndex;
    [SerializeField] private bool InDebug;
    [SerializeField] private float _minDistanceToPoint =0.5f;
    private int _currentMaxCorners;
    private NavMeshPath _currentPath;

    private void Awake()
    {
        _currentPath = new NavMeshPath();
    }
    public Vector3 GetCurrentPathPoint()
    {
        return _currentPath.corners[_currentCornerIndex];
    }

    private void OnDrawGizmos()
    {
        if (InDebug && _currentPath != null)
        {
            for (int i = 0; i < _currentPath.corners.Length - 1; i++)
                Debug.DrawLine(_currentPath.corners[i], _currentPath.corners[i + 1], Color.yellow);
        }
    }
    //Checks if owner is at the end of path, and if they should move to next path poínt
    public bool EvaluatePath(NavMeshPath path, Vector3 currentPosition)
    {
        _currentPath = path;
        if (path.corners.Length <= 0) return false; //Path is invalid no where to move to

        if (_currentMaxCorners != path.corners.Length)
        {
            _currentMaxCorners = path.corners.Length;
            _currentCornerIndex = 0;
        }

       
        //if distance to next point is less than 0.1f character is basically there so move to next point
        float distanceToPoint = Vector3.Distance(path.corners[_currentCornerIndex], currentPosition);
        if (Vector3.Distance(path.corners[_currentCornerIndex], currentPosition) <= _minDistanceToPoint)
        {
            //if at the last index 
            if(_currentCornerIndex >= path.corners.Length-1)
            {
                _currentCornerIndex = 0;
                return false;
            }
            else
            {
                NextPoint();
            }
        }
        return true;
    }

    public int ValidateIndex(int index)
    {
        if (_currentPath != null)
        {
            if (index >= _currentPath.corners.Length) index = _currentPath.corners.Length - 1;
        }
        return index;
    }
    public void NextPoint()
    {
        _currentCornerIndex++;
        if (_currentCornerIndex >= _currentMaxCorners) _currentCornerIndex = _currentMaxCorners - 1;
    }

}
