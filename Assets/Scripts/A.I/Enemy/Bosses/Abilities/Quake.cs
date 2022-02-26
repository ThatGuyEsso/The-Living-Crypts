using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quake : BaseBossAbility
{
    public override void Execute()
    {
        Debug.Log("Trying to do quake");
    }

    public override void Terminate()
    {
        throw new System.NotImplementedException();
    }


}
