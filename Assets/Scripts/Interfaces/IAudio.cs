using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudio 
{
    void SetUpAudioSource(Sound sound, AudioManager AM);
    void Play();

    void PlayAtRandomPitch();
}
