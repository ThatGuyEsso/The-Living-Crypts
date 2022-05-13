using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionSettings : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropDown;

    Resolution[] allResolutions;

    public void Awake()
    {
        allResolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();
        List<string> r = new List<string>();

        int currentRes = 0;
        for (int i = 0; i < allResolutions.Length; i++)
        {
            string option = allResolutions[i].width + " x " + allResolutions[i].height;
            r.Add(option);

            if (allResolutions[i].width == Screen.currentResolution.width && allResolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }
        resolutionDropDown.AddOptions(r);
        resolutionDropDown.value = currentRes;
        resolutionDropDown.RefreshShownValue();
    }


    public void SetResolution(int resIndex)
    {
        Resolution resolution = allResolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
