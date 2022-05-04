using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, IAudio
{
    AudioSource source;
    string currentName;
    Sound currentSound;
    bool isFadingOut;
    float fadeOutRate = 2f;
    private AudioManager AM;
    public void Awake()
    {
        isFadingOut = false;
        source = gameObject.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void SetUpAudioSource(Sound sound, AudioManager manager)
    {
        isFadingOut = false;
        currentSound = sound;
        currentName = sound.name;
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.outputAudioMixerGroup = sound.mixerGroup;
        AM = manager;
    }

    public void SetUpAudioSource(Sound sound, float pitch, AudioManager manager)
    {
        if (sound!=null)
        {
            isFadingOut = false;
            currentSound = sound;
            currentName = sound.name;
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = pitch;
            source.loop = sound.loop;
            source.outputAudioMixerGroup = sound.mixerGroup;
            AM = manager;
        }
      
    }


    public void Play()
    {
        StartCoroutine(ListiningToFinish());
    }

    public void KillAudio()
    {
        isFadingOut = false;
        StopAllCoroutines();
        source.Stop();
        if (ObjectPoolManager.instance)
        {
            ObjectPoolManager.Recycle(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void BeginFadeOut() { isFadingOut = true; }
    private void Update()
    {
        if (isFadingOut)
        {
            source.volume = Mathf.Lerp(source.volume, 0f, Time.deltaTime * fadeOutRate);
            if (source.volume <= 0.01f)
            {
                isFadingOut = false;
                KillAudio();
            }
        }
    }
    public void PlayAtRandomPitch()
    {
        if (!AM)
        {
            if (!GameStateManager.instance)
            {
                return;
            }
            AM = GameStateManager.instance.AudioManager;
        }
    

        float randPitch = AM.GetRandomPitchOfSound(AM.GetSound(currentName));
        source.pitch = randPitch;
        StartCoroutine(ListiningToFinish());
    }

    public void PlaySoundAtRandomPitch()
    {
        float randPitch = Random.Range(currentSound.pitch - currentSound.pitchChange, currentSound.pitch + currentSound.pitchChange);
        source.pitch = randPitch;
        StartCoroutine(ListiningToFinish());
    }


    public string GetName() { return currentName; }

    IEnumerator ListiningToFinish()
    {

        source.Play();

        while (source.isPlaying)
        {
            yield return null;
        }
        if(gameObject)
            ObjectPoolManager.Recycle(gameObject);
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }
}
