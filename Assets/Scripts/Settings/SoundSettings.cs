using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum AudioType
{
    soundEffects,
    UIAudio,
    Music
};

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer musicVolume;
    [SerializeField] private AudioMixer sfxVolume;
    [SerializeField] private AudioMixer uiVolume;
    [SerializeField] private Slider slider;
    [SerializeField] private string soundparamater;
    [SerializeField] private AudioType audioType;
    [SerializeField] private MusicManager MusicManager;
    public void SetVolume(float value)
    {

        switch (audioType)
        {
            case AudioType.soundEffects:
                sfxVolume.SetFloat(soundparamater, Mathf.Log10( value)*20f);
                break;
            case AudioType.UIAudio:
                uiVolume.SetFloat(soundparamater, Mathf.Log10(value) * 20f);
                break;
            case AudioType.Music:

                musicVolume.SetFloat(soundparamater, Mathf.Log10(value) * 20f);
                if (value <= -1e-06)
                {
                    if (!MusicManager)

                    {
                        MusicManager = GetMusicManager();


                    }
                    if (MusicManager)

                    {
                        MusicManager.ToggleMusic(false);
                    }
                }
                else
                {
                    if (!MusicManager)

                    {
                        MusicManager = GetMusicManager();


                    }
                    if (MusicManager)

                    {
                        MusicManager.ToggleMusic(true);
                    }
                }
     
                break;
        }


  

    }

    public MusicManager GetMusicManager()
    {
        if (MusicManager)
        {
            return MusicManager;
        }
        else
        {
              if (!GameStateManager.instance || !GameStateManager.instance.MusicManager)
                {
                    return null;
                }
                else
                {
                    return GameStateManager.instance.MusicManager;
                }
         
        }

        return null;
    }

    private void OnEnable()
    {
        //if (slider)
        //{
        //    float vol;
        //    switch (audioType)
        //    {
        //        case AudioType.soundEffects:
        //            sfxVolume.GetFloat(soundparamater, out vol);
        //            slider.value = Mathf.Pow(vol,10);

        //            break;
        //        case AudioType.UIAudio:
        //            uiVolume.GetFloat(soundparamater, out vol);
        //            slider.value = Mathf.Pow(vol, 10);
        //            break;
        //        case AudioType.Music:
        //            musicVolume.GetFloat(soundparamater, out vol);
        //            slider.value = Mathf.Pow(vol, 10);
               
        //            break;
        //    }
        //}
    }

}
