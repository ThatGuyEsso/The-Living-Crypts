using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ScalingProgressBar : MonoBehaviour
{
    [SerializeField] public Image frame;
    [SerializeField] public Image fill;
    [SerializeField] private float maxValue;
    [SerializeField] private float value;

    [SerializeField] private float scaleRate=0.5f;
    float targetPercent;
    public Action ScalingBegun;


    public void SetMaxValue(float maxVal) { maxValue = maxVal; }
    public void SetValue(float newVal) { value = newVal; }

    public void UpdateValue(float newVal)
    {
        SetValue(newVal);
        targetPercent= value / maxValue;
        UpdateFill();
        ScalingBegun?.Invoke();
       
    }

    public void UpdateFill()
    {
    
        
         fill.transform.localScale = new Vector3(targetPercent, fill.transform.localScale.y, fill.transform.localScale.z);

    
    }

    public void HideBar()
    {
        frame.enabled = false;
        fill.enabled = false;
    }
    public void ShowBar()
    {
        frame.enabled = true;
        fill.enabled = true;
    }
}
