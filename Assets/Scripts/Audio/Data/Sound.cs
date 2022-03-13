using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
    public AudioMixerGroup mixerGroup;
    public float pitchChange;


    public Sound(AudioClip sClip, float soundVolume, float soundPitch, bool isLooping, AudioMixerGroup mixGroup, float maxPitchChange, string sName)
    {
        name = sName;
        clip = sClip;
        volume = soundVolume;
        pitch = soundPitch;
        loop = isLooping;
        mixerGroup = mixGroup;
        pitchChange = maxPitchChange;
    }
}


