using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFlash : MonoBehaviour
{
    private ProgressBar _slider;
    private Color defaultColour;
    [SerializeField] private Color flashColour = Color.white;
    [SerializeField] private float flashTime;
    private void Awake()
    {
        _slider = gameObject.GetComponent<ProgressBar>();
        _slider.OnSliderChange += FlashOn;
        defaultColour = _slider.sliderFill.color;
    }

    private void FlashOn()
    {
        _slider.sliderFill.color = flashColour;
        Invoke("FlashOff", flashTime);
    }

    private void FlashOff()
    {
        _slider.sliderFill.color = defaultColour;
    }
}

