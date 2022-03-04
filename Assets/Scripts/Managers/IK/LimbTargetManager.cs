using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzelGames.FastIK;
public class LimbTargetManager : MonoBehaviour
{
    private FastIKFabric[] IKs;
    private List<Transform> previousTarget = new List<Transform>();
    private BipedalProcAnim animManager;
    private void Awake()
    {
        IKs = GetComponentsInChildren<FastIKFabric>();
        animManager = GetComponent<BipedalProcAnim>();
    }

    public void UseSelfAsTarget()
    {
        previousTarget.Clear();
        if (IKs.Length == 0) return;
        for(int i=0; i< IKs.Length; i++)
        {
            previousTarget.Add(IKs[i].Target);
            IKs[i].Target = IKs[i].transform;
            IKs[i].enabled = false;
        }
    }


    public void UseInitialTarget()
    {
        if (previousTarget.Count == 0) return;
        animManager.SetTargetsAtCurrentFootPoint();
        for (int i = 0; i < IKs.Length; i++)
        {
            IKs[i].enabled = true;
            IKs[i].Target = previousTarget[i];
        }
    }
}
