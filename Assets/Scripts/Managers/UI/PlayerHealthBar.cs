using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private ScalingProgressBar ProgressBar;
    [SerializeField] private ScalingSliderFlash Flasher;
    [SerializeField] private UIElementShake Shaker;
    public void SetUpHealthBar(float newHealth, float maxHealth)
    {
        ProgressBar.SetMaxValue(maxHealth);
        ProgressBar.SetValue(newHealth);
    }
    public void OnUpdateDamageDisplay(float health)
    {
        Shaker.BeginViewBob();
        ProgressBar.UpdateValue(health);
    }


    
}
