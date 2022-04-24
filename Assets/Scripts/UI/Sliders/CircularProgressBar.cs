using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularProgressBar : ScalingProgressBar
{
    public override void UpdateFill()
    {
        fill.transform.localScale = new Vector3(targetPercent, targetPercent, fill.transform.localScale.z);
    }
}
