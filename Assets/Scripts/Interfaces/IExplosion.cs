using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosion
{
    public void InitExplosion(ExplosionData data);

    public GameObject GetOwner();
}
