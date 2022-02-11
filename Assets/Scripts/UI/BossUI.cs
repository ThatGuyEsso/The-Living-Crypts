using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BossUI : MonoBehaviour
{ 
  
    public ScalingProgressBar progressBar;
    [SerializeField] private TextMeshProUGUI bossNameDisplay;
    [SerializeField] private Animator animator;
    [SerializeField] private UIElementShake uiShaker;
    [SerializeField] private ResizeToFitText displayRefit;

    public Action OnUISpawned;
    public void InitialiseUI(string bossName,float maxHealth)
    {
        progressBar.SetMaxValue(maxHealth);
        bossNameDisplay.text = bossName;
        displayRefit.FitText();
        animator.Play("InitHealth");
    }

    public void HideUI()
    {
        animator.enabled = true;
        animator.Play("HideBossHealth");
    }

    public void OnUISpawnAnimComplete()
    {
        OnUISpawned?.Invoke();
        animator.enabled = false;
    }

    public void PlaySFX()
    {
        if (AudioManager.instance)
        {
            //AudioManager.instance.PlayUISound("RoomSpawn",transform.position);
        }
    }
    public void DoHurtUpdate(float newHealth)
    {
        progressBar.UpdateValue(newHealth);
        uiShaker.BeginViewBob();

    }
 
}
