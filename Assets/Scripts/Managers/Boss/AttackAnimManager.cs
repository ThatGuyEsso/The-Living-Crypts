using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackAnimManager : MonoBehaviour
{

    public System.Action OnReadyUpBegin;
    public System.Action OnReadyUpComplete;

    public System.Action OnAttackBegin;
    public System.Action OnAttackEnd;

    public void AttackReadyUpStarted()
    {
        OnReadyUpBegin?.Invoke();
    }
    public void AttackReadyUpCompleted()
    {
        OnReadyUpComplete?.Invoke();
    }

    public void AttackStarted()
    {
        OnAttackBegin?.Invoke();
    }

    public void AttackCompleted()
    {
        OnAttackEnd?.Invoke();
    }

}
