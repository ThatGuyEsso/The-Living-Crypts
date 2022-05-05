using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshRebuild : MonoBehaviour
{
    NavMeshSurface surface;
    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();

        if(surface && !surface.navMeshData)
        {
            surface.BuildNavMesh();
            Debug.Log("NavMesh Rebuilt");
        }
    }
}
