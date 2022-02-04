using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    private Transform _origin;
    public void Init()
    {
        if (!_line)
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 0;
            _line.enabled = false;
        }
        else
        {
            _line.positionCount = 0;
            _line.enabled = false;
        }
    }


    public void SetOrigin(Transform origin)
    {
        _origin = origin;
    }


    public void DrawLinePositions(List<Vector3> points)
    {
        if (!_line.enabled) _line.enabled = true;

        int posCount = points.Count + 1;

        _line.positionCount = posCount;

        _line.SetPosition(0, _origin.position);

        for(int i=0; i < points.Count; i++)
        {
            _line.SetPosition(i+1, points[i]);
        }
    }

    public void ClearLine()
    {
        _line.positionCount = 0;
        if (_line.enabled) _line.enabled = false;
    }
}
