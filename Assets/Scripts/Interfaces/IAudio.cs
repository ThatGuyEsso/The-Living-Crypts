using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudio 
{
    void SetUpAudioSource(Sound sound);
    void Play();

    void PlayAtRandomPitch();
}
