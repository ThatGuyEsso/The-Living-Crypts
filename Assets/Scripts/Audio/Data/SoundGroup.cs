using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class SoundGroup 
{
    public string name;
    public AudioClip[] clips;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
    public AudioMixerGroup mixerGroup;
    public float pitchChange;
    public AudioClip GetRandClip()
    {
        int rand = Random.Range(0, clips.Length - 1);
        return clips[rand];
    }
    public Sound GetRandomSoundMember()
    {
        int rand = Random.Range(0, clips.Length - 1);

        Sound newSound = new Sound(clips[rand], volume, pitch, loop, mixerGroup, pitchChange, name);

        return newSound;
    }
}
