using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PathFinder : MonoBehaviour, IInitialisable
{
    [SerializeField] public bool InDebug;
    private NavMeshPath _path;

    private bool _isInitialised;

    private void Awake()
    {
        Init();
    }
    public void Init()
    {

        _path = new NavMeshPath();
        _isInitialised = true;
    }

    public NavMeshPath GetPathToTarget(Vector3 origin, Vector3 targetPoint, int areaMask)
    {
        NavMesh.CalculatePath(origin, targetPoint, areaMask, _path);

        return _path;
    }

    private void OnDrawGizmos()
    {
        if (InDebug && _path != null && _isInitialised)
        {
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
        }
    }

    private void Update()
    {
        if (!_isInitialised || _path.corners.Length <= 0) return;

    }


    public NavMeshPath Path { get { return _path; } }
}

