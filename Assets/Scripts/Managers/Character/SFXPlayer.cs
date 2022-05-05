using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{


    [SerializeField]  private AudioManager AM;
    [SerializeField] private CamShakeSetting SFXShake;

    public void DoSFX(string sfx)
    {
        PlaySFX(sfx,false);
    }
    public void DoSFXRandomPitch(string sfx)
    {
        PlaySFX(sfx, true);
    }

    public void DoSFXScrenShake()
    {
        if (CamShake.instance)
        {
            CamShake.instance.DoScreenShake(SFXShake);
        }
    }

    public virtual AudioPlayer PlaySFX(string sfxName, bool randPitch)
    {
        if (AM)
        {
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }

            AM = GameStateManager.instance.AudioManager;
            return AM.PlayThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
    }
    public virtual AudioPlayer PlayGroupSFX(string sfxName, bool randPitch)
    {
        if (AM)
        {
            return AM.PlayGroupThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
        else
        {
            if (!GameStateManager.instance || !GameStateManager.instance.AudioManager)
            {
                return null;
            }

            AM = GameStateManager.instance.AudioManager;
            return AM.PlayGroupThroughAudioPlayer(sfxName, transform.position, randPitch);
        }
    }
}
