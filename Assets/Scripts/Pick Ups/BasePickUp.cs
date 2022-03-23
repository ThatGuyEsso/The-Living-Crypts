using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickUp : MonoBehaviour
{
    public abstract bool IsEnabled();
    protected abstract void DoPickUp();
    protected abstract void DisablePickUp();
    public abstract void EnablePickUp();
}
