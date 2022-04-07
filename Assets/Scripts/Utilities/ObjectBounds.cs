using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBounds : MonoBehaviour
{
    [SerializeField] private float _width, _length, _height;

    public Vector3 GetHalfExtents()
    {
        return new Vector3(_width* transform.localScale.x, _height * transform.localScale.y, _length * transform.localScale.z);
    }



    private void OnDrawGizmos()
    {
        
        Vector3 centre = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centre , new Vector3(_width * transform.localScale.x, _height * transform.localScale.y, _length * transform.localScale.z));

      
    }
}
