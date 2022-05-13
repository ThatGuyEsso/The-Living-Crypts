using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;




    public void ToggleScreen(bool isOn)
    {

        settings.SetActive(isOn);
        
     

    }




}
