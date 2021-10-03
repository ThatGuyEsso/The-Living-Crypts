using System;
using System.Collections.Generic;
using UnityEngine;
public interface IVolumes
{
    void SetIsPlayerZone(bool isPlayerZone);
    void SetUpDamageVolume(float dmg,float kBack, Vector2 dir,GameObject owner);
    void SetDespawnTime(float time);
}