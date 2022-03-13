using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour,IManager,IInitialisable
{
    public Sound[] sounds;
    public SoundGroup[] soundGroups;

    [SerializeField] private AudioMixerGroup uiGroup, soundEffectGroup;

    [Header("AudioPlayers")]
    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private GameObject uiAudioPlayer;
    [SerializeField] private bool inDebug = false;
    public void Init()
    {

        //Create sound class
        foreach (Sound s in sounds)
        {
            //Create audio source or respective sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
        for(int i=0;i<soundGroups.Length;i++)
        {
            ////Create audio source for each sound group
            soundGroups[i].source = gameObject.AddComponent<AudioSource>();
            soundGroups[i].source.volume = soundGroups[i].volume;
            soundGroups[i].source.pitch = soundGroups[i].pitch;
            soundGroups[i].source.loop = soundGroups[i].loop;
            soundGroups[i].source.outputAudioMixerGroup = soundGroups[i].mixerGroup;
        }

        if (GameStateManager.instance)
        {
            GameStateManager.instance.AudioManager = this;
        }
    }

    public void PlayRandFromGroup(string groupName)
    {
        //Find Sound Group
        SoundGroup soundGroup = Array.Find(soundGroups, group => group.name == groupName);

        //load new clip into source
        soundGroup.source.clip = soundGroup.GetRandClip();

        if(soundGroup != null)
        {
            //Play new sound if it exists
            soundGroup.source.Play();
        }
        else
        {
            Debug.Log("Group of name:" + groupName + " was not found");
        }
    }

    //Play sound from sound name
    public void Play(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {

            currentSound.source.Play();


        }
        else
        {
            Debug.Log("Sound of name:" + name + " was not found");
        }
    }

    public void PlayIfFree(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {

            if (!currentSound.source.isPlaying) currentSound.source.Play();


        }
        else
        {
            Debug.Log("Sound of name:" + name + " was not found");
        }
    }


    //Play sound of at random pitch 
    public void PlayAtRandomPitch(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {
            float ogPitch = currentSound.pitch;
            currentSound.pitch = UnityEngine.Random.Range( currentSound.pitch- currentSound.pitchChange, currentSound.pitch+ currentSound.pitchChange);

         

            currentSound.source.Play();
       
            currentSound.pitch = ogPitch;


        }
        else
        {
            //Debug.Log("Sound of name:" + name + " was not found");
        }
    }

    //Stop a currently playing sound
    public void Stop(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {
            currentSound.source.Stop();
        }
        else
        {
            //Debug.Log("Sound of name:" + name + " was not found");
        }
    }

    public Sound GetSound(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        return currentSound;
    }
    public Sound GetSound(Sound sound)
    {
        foreach(Sound currentSound in sounds)
        {
            if (currentSound == sound) return currentSound;
        }
        return null;
    }
    public float GetRandomPitchOfSound(Sound sound)
    {
        return UnityEngine.Random.Range(sound.pitch - sound.pitchChange, sound.pitch + sound.pitchChange);
    }



    public AudioPlayer PlayThroughAudioPlayer(string name, Vector3 pos) //Function Spawns Audio player then players the sound
    {
        if (name == string.Empty)
        {
            return null;
        }
        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                audio.SetUpAudioSource(GetSound(name)); //set the Audio up
                audio.Play(); //Then play 
                return audio;
            }
        }
        return null;
    }
    public AudioPlayer PlayThroughAudioPlayer(string name, Transform targetTransform) //Function Spawns Audio player then players the sound
    {
        if(name == string.Empty)
        {
            return null;
        }
        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, targetTransform).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                audio.SetUpAudioSource(GetSound(name)); //set the Audio up
                audio.Play(); //Then play 
                return audio;
            }
        }
        return null;
    }

    public AudioPlayer PlayGroupThroughAudioPlayer(string name, Vector3 pos) //Function Spawns Audio player then players the sound
    {
        if (name == string.Empty)
        {
            return null;
        }
        //Find Sound Group
        SoundGroup soundGroup = Array.Find(soundGroups, group => group.name == name);
        if (soundGroup == null) return null;
        //load new clip into source
        Sound groupMemeber = soundGroup.GetRandomSoundMember();

        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                audio.SetUpAudioSource(groupMemeber); //set the Audio up
                audio.Play(); //Then play 
                return audio;
            }
        }
        return null;
    }

    public AudioPlayer PlayGroupThroughAudioPlayer(string name, Vector3 pos, bool randPitch) //Function Spawns Audio player then players the sound
    {
        if (name == string.Empty)
        {
            return null;
        }
        //Find Sound Group
        SoundGroup soundGroup = Array.Find(soundGroups, group => group.name == name);
        if (soundGroup == null) return null;
        //load new clip into source
        Sound groupMemeber = soundGroup.GetRandomSoundMember();

        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                audio.SetUpAudioSource(groupMemeber); //set the Audio up
                audio.PlaySoundAtRandomPitch(); //Then play 
                return audio;
            }
        }
        return null;
    }


    public AudioPlayer PlayGThroughAudioPlayerPitchShift(string name, Vector3 pos, float pitchShift)
    {
        if (name == string.Empty)
        {
            return null;
        }
        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                Sound sound = GetSound(name);
                audio.SetUpAudioSource(GetSound(name), sound.pitch + pitchShift); //set the Audio up
                audio.Play(); //Then play 
                return audio;
            }
        }
        return null;
    }
    //Function Spawns Audio player then players the sound
    public AudioPlayer PlayThroughAudioPlayer(string name, Vector3 pos, bool randPitch) //Function Spawns Audio player then players the sound
    {
        if (name == string.Empty)
        {
            return null;
        }
        if (ObjectPoolManager.instance) //check instance of object pool
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(audioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer

            if (audio)
            {
                audio.SetUpAudioSource(GetSound(name)); //set the Audio up
                if (randPitch) audio.PlayAtRandomPitch();
                else
                    audio.Play(); //Then play 

                return audio;

            }
        }

        return null;
    }

    public AudioPlayer PlayUISound(string name, Vector3 pos) //Function sets up UI Audio player then plays the sound
    {
        if (name == string.Empty)
        {
            return null;
        }
        if (ObjectPoolManager.instance) //check instance
        {
            AudioPlayer audio = ObjectPoolManager.Spawn(uiAudioPlayer, pos, Quaternion.identity).GetComponent<AudioPlayer>(); //Gets the audioplayer
            if (audio)
            {
                audio.SetUpAudioSource(GetSound(name));
                audio.Play();
                return audio;
            }
        }
        return null;

    }

    public void PlayUISound(string name, Vector3 pos, bool randPitch) //Function sets up UI Audio player then plays the sound
    {
        if (name == string.Empty)
        {
            return;
        }
        if (ObjectPoolManager.instance) //check instance
        {
            IAudio audio = ObjectPoolManager.Spawn(uiAudioPlayer, pos, Quaternion.identity).GetComponent<IAudio>(); //Get interface from spawned gameobject
            if (audio != null)
            {
                audio.SetUpAudioSource(GetSound(name));
                if (randPitch)
                    audio.PlayAtRandomPitch();
                else
                    audio.Play();
            }
        }

    }

    public void BindToGameStateManager()
    {
        GameStateManager.instance.OnNewGameState += EvaluateGameState;
    }

    private void OnDisable()
    {
        if (GameStateManager.instance)
             GameStateManager.instance.OnNewGameState -= EvaluateGameState;
    }
    private void OnDestroy()
    {
        if (GameStateManager.instance)
            GameStateManager.instance.OnNewGameState -= EvaluateGameState;
    }
    public void EvaluateGameState(GameState newState)
    {
    ///
    }
}
