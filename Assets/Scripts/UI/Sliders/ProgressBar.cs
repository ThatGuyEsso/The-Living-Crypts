using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;
    public event SliderUpdateDelegate OnSliderChange;
    public delegate void SliderUpdateDelegate();
    public void InitSlider(float maxValue)
    {
        slider.maxValue = maxValue;
       
    }
    public void UpdateSlider(float newValue)
    {
        slider.value = newValue;
        OnSliderChange?.Invoke();
    }
}
