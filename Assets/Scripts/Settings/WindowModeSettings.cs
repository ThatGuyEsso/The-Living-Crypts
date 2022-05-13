using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WindowModeSettings : MonoBehaviour
{
    public void SetWindowMode(int index)
    {
        if (index == 0)
            Screen.fullScreenMode = FullScreenMode.Windowed;
        else if (index == 1)
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else if (index == 2)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

}
