using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BaseBossAbility
{
    [SerializeField] private string ReadyUpAnimation, ChargeAnimation, EndAnimation;

    public override void Execute()
    {
        Debug.Log("Trying to charge");
    }

    public override void Terminate()
    {
        throw new System.NotImplementedException();
    }
}
