using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingSliderFlash : MonoBehaviour
{
    private ScalingProgressBar scalingSider;
    private Color defaultColour;
    private Color flashColour = Color.white;
    [SerializeField] private float flashTime;
    private void Awake()
    {
        scalingSider = gameObject.GetComponent<ScalingProgressBar>();
        defaultColour = scalingSider.fill.color;
        scalingSider.ScalingBegun += FlashOn;

    }

    private void FlashOn()
    {
        scalingSider.fill.color = flashColour;
        Invoke("FlashOff", flashTime);
    }

    private void FlashOff()
    {
        scalingSider.fill.color = defaultColour;
    }
}

