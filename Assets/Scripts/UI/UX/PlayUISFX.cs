using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUISFX : MonoBehaviour
{
   
    [SerializeField] private string sfxName;
    [SerializeField] private bool isPitchRandom;
     private AudioManager _audioMananger = null;
    public void PlayOnHooverSFX()
    {
        if (_audioMananger)
        {

            _audioMananger.PlayUISound(sfxName, Vector3.zero, isPitchRandom);
        }
        else
        {
            _audioMananger = GameStateManager.instance.AudioManager;
            if (_audioMananger)
            {
                _audioMananger.PlayUISound(sfxName, Vector3.zero, isPitchRandom);
            }
        }



    }
    public void PlayOnHooverSFX(string SoundEffectName)
    {
        if (_audioMananger)
        {

            _audioMananger.PlayUISound(SoundEffectName, Vector3.zero, isPitchRandom);
        }
        else
        {
            if(GameStateManager.instance)
            {
                _audioMananger = GameStateManager.instance.AudioManager;
                if (_audioMananger)
                {
                    _audioMananger.PlayUISound(SoundEffectName, Vector3.zero, isPitchRandom);
                }
            }
      
        }



    }

    public void PlayOnClickSFX()
    {
        if (_audioMananger)
        {

            _audioMananger.PlayUISound(sfxName, Vector3.zero, isPitchRandom);
        }
        else
        {
            _audioMananger = GameStateManager.instance.AudioManager;
            if (_audioMananger)
            {
                _audioMananger.PlayUISound(sfxName, Vector3.zero, isPitchRandom);
            }
        }



    }
    public void PlayOnClickSFX(string SoundEffectName)
    {
        if (_audioMananger)
        {

            _audioMananger.PlayUISound(SoundEffectName, Vector3.zero, isPitchRandom);
        }
        else
        {
            if (GameStateManager.instance)
            {
                _audioMananger = GameStateManager.instance.AudioManager;
                if (_audioMananger)
                {
                    _audioMananger.PlayUISound(SoundEffectName, Vector3.zero, isPitchRandom);
                }
            }

        }



    }
}
