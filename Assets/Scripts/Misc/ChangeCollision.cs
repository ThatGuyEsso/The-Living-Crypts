using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollision : MonoBehaviour
{
    private LayerMask targetLayer;


    public void WaitToChangeLayer(LayerMask newLayer,float time)
    {
        targetLayer = newLayer;
        Invoke("ChangeToTargetLayer", time);
    }

    public void ChangeToTargetLayer()
    {
        gameObject.layer = targetLayer;
    }

    public void SetLayer(LayerMask newLayer)
    {
        gameObject.layer = newLayer;
    }
}
