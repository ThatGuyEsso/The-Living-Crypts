using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour,IInitialisable
{

    [SerializeField] private AudioSource primarySource;
    [SerializeField] private AudioSource secondarySource;
    [SerializeField] private Dictionary<string,List<Sound>> Tracks;

    [SerializeField] private float crossFadeRate;
    [SerializeField] private float fadeInRate;
    [SerializeField] private float fadeOutRate;
 
    [SerializeField] private AudioMixerGroup musicAudioGroup;
    public Sound[] music;

    private GameManager GM;
    private Sound nextSong;
    private Sound currentSongPlaying;
    private float currentTimeToNextSong;

    bool isInitialised;

    bool isFadingIn;
    bool isFadingOut;

    bool isMusicOff;

    public Action OnSongEnd;
    public Action OnFadeComplete;
    public void Init()
    {

        DontDestroyOnLoad(gameObject);

        primarySource.clip = null;
        secondarySource.clip = null;
        primarySource.Stop();
        secondarySource.Stop();
        //Initialise variables
        if (GameStateManager.instance)
        {
            GameStateManager.instance.MusicManager = this;
        }
        //Subscribe to intiation manager
    }


    public void SetUpGameManager(GameManager GameManager)
    {
        GM = GameManager;
    }

    private void EvaluateGameplayEvent(GameplayEvents newEvent)
    {
        
    }

    public void Update()
    {
        if (isMusicOff) return;
        if (isFadingIn)
        {
            primarySource.volume = Mathf.Lerp(primarySource.volume, currentSongPlaying.volume, Time.deltaTime * fadeInRate);
            if(Mathf.Abs(primarySource.volume -currentSongPlaying.volume) <= 0.01f)
            {
                isFadingIn = false;
               
                primarySource.volume = currentSongPlaying.volume;
                OnFadeComplete?.Invoke();
            }
        }



        if (isFadingOut)
        {
            primarySource.volume = Mathf.Lerp(primarySource.volume,0f, Time.deltaTime * fadeOutRate);
            if (primarySource.volume  <= 0.01f)
            {
                isFadingOut = false;
                primarySource.Stop();
                primarySource.volume = 0f;
                OnFadeComplete?.Invoke();
            }
        }
    }
    public void BeginSongFadeIn(string songName, float fadeRate, float minTimeNext, float maxTimeNext)
    {
        if (isMusicOff) return;
        Sound newSong = GetSong(songName);

        if (newSong != null)
        {
            fadeInRate = fadeRate;
            currentTimeToNextSong = UnityEngine.Random.Range(minTimeNext, maxTimeNext);
          
            if (primarySource.isPlaying)
            {
                StopCoroutine(WaitToPrimarySongFinish(newSong));
                BeginSongFadeOut(fadeOutRate);
                nextSong = newSong;
                OnFadeComplete += FadeInNewSong;


                WaitToPrimarySongFinish(newSong);
            }
            else
            {
                DoSongFadeIn(newSong);
            }
        }
    }

    public void BeginSongFadeIn(string songName, float fadeRate, float minTimeNext, float maxTimeNext,float delay)
    {
        if (isMusicOff) return;
        Sound newSong = GetSong(songName);

        if (newSong != null)
        {
            fadeInRate = fadeRate;
            currentTimeToNextSong = UnityEngine.Random.Range(minTimeNext, maxTimeNext);
            if (primarySource.isPlaying)
            {
                StopCoroutine(WaitToPrimarySongFinish(newSong));
                BeginSongFadeOut(fadeOutRate);
                nextSong = newSong;
            }
            else
            {
                StartCoroutine(FadeInWithDelay(newSong, delay));

            }
        }
    }
    public void FadeInNewSong()
    {
        OnFadeComplete -= FadeInNewSong;
        DoSongFadeIn(nextSong);
    }

    public IEnumerator FadeInWithDelay(Sound song, float delay)
    {

        yield return new WaitForSeconds(delay);
        DoSongFadeIn(song);
    }

    public void BeginSongFadeOut(float fadeRate)
    {
        StopAllCoroutines();
        if (IsPlaying())
        {
            fadeOutRate = fadeRate;

            isFadingIn = false;
            isFadingOut = true;
        }
        else
        {
            OnFadeComplete?.Invoke();
        }
    }

    public void DoSongFadeIn(Sound song)
    {
        if (isMusicOff) return;
        StopAllCoroutines();
        primarySource.volume = 0f;
        primarySource.clip = song.clip;
        primarySource.pitch = song.pitch;
        primarySource.outputAudioMixerGroup = song.mixerGroup;
        currentSongPlaying = song;
        primarySource.Play();
        isFadingIn = true;
        isFadingOut = false;
        StartCoroutine(ListenToSongEnd());
    }

    public IEnumerator ListenToSongEnd( )
    {
        while (primarySource.isPlaying)
        {

            yield return null;
        }
        StartCoroutine(WaitToRepeatSong());
        OnSongEnd?.Invoke();

    }


    public IEnumerator WaitToRepeatSong()
    {
        yield return new WaitForSeconds(currentTimeToNextSong);
        DoSongFadeIn(currentSongPlaying);
    }


    public IEnumerator WaitToPrimarySongFinish(Sound song)
    {
        while (primarySource.isPlaying)
        {

            yield return null;
        }
        yield return new WaitForSeconds(2f);
        DoSongFadeIn(song);
    }
    public Sound GetSong(string name)
    {
        Sound currSong = Array.Find(music, music => music.name == name);
        return currSong;
    }
    public Sound GetSound(Sound sound)
    {
        foreach (Sound currSong in music)
        {
            if (currSong == sound) return currSong;
        }
        return null;
    }


    public bool IsPlaying()
    {
        return primarySource.isPlaying || secondarySource.isPlaying;
    }

    private void OnDestroy()
    {
        if(GM)
        {
            GM.OnNewGamplayEvent -= EvaluateGameplayEvent;
        }
    }


    public void StopMusic()
    {
        StopAllCoroutines();

        primarySource.Stop();
        secondarySource.Stop();
        isFadingIn = false;
        isFadingOut = false;
    }

    public bool IsMusicOff() { return isMusicOff; }
    public void ToggleMusic(bool isOn)
    {
        if (isOn) isMusicOff = false;
        else
        {
            isMusicOff = true;
            StopAllCoroutines();
            StopMusic();
        }
    }
}




