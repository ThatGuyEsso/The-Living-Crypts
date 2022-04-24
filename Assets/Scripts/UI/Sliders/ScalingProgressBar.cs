using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ScalingProgressBar : MonoBehaviour
{
    [SerializeField] public Image frame;
    [SerializeField] public Image fill;
    [SerializeField] protected float maxValue;
    [SerializeField] protected float value;

    [SerializeField] protected float scaleRate=0.5f;
    protected float targetPercent;
    public Action ScalingBegun;


    public void SetMaxValue(float maxVal) { maxValue = maxVal; }
    public void SetValue(float newVal) { value = newVal; }

    public void UpdateValue(float newVal)
    {
        SetValue(newVal);
        if(value <=0)
        {

            fill.enabled = false;
        }
        else
        {
            fill.enabled = true;
            targetPercent = value / maxValue;
            UpdateFill();
            ScalingBegun?.Invoke();
        }

       
    }

    public virtual void UpdateFill()
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
