using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;
public class LimbTargetManager : MonoBehaviour
{
    private FastIKFabric[] IKs;
    private List<Transform> previousTarget = new List<Transform>();

    private void Awake()
    {
        IKs = GetComponentsInChildren<FastIKFabric>();
    }

    public void UseSelfAsTarget()
    {
        previousTarget.Clear();
        if (IKs.Length == 0) return;
        for(int i=0; i< IKs.Length; i++)
        {
            previousTarget.Add(IKs[i].Target);
            IKs[i].Target = IKs[i].transform;
        }
    }


    public void UseInitialTarget()
    {
        if (previousTarget.Count == 0) return;
        for (int i = 0; i < IKs.Length; i++)
        {
        
            IKs[i].Target = previousTarget[i];
        }
    }
}
