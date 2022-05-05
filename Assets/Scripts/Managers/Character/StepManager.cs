using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField] private GameObject StepVFX;
    [Header("SFX")]
    [SerializeField] private string StepSFX;
    [Header("Feet")]
    [SerializeField] private Transform RightFoot, LeftFoot;

    [SerializeField]  private AudioManager AM;
    [SerializeField] private CamShakeSetting StepShakeSetting;
    public void DoStep(bool randomPitch, Vector3 point)
    {
        PlaySFX(StepSFX, randomPitch);
        SpawnStepSFX(point);
    }

    public void DoStep(bool randomPitch, Transform pointTransform)
    {
        PlaySFX(StepSFX, randomPitch);
        SpawnStepSFX(pointTransform);
    }

    public void DoStepGroupRight()
    {
        DoStepGroup(true, RightFoot);
        SpawnStepSFX(RightFoot);
    }
    public void DoStepGroupLeft()
    {
        DoStepGroup(true, LeftFoot);
        SpawnStepSFX(LeftFoot);

        
    }
    public void DoHeavyStepGroupRight()
    {
        DoStepGroup(true, RightFoot);
        SpawnStepSFX(RightFoot);
        if (CamShake.instance)
        {
            CamShake.instance.DoScreenShake(StepShakeSetting);
        }
    }
    public void DoHeavyStepGroupLeft()
    {
        DoStepGroup(true, LeftFoot);
        SpawnStepSFX(LeftFoot);
        if (CamShake.instance)
        {
            CamShake.instance.DoScreenShake(StepShakeSetting);
        }
    }
    public void DoStepGroup(bool randomPitch, Transform pointTransform)
    {
        PlayGroupSFX(StepSFX, randomPitch);
        SpawnStepSFX(pointTransform);
    }

    public void SpawnStepSFX(Vector3 point)
    {
        if (StepVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(StepVFX, point, Quaternion.identity);
            }
            else
            {
                Instantiate(StepVFX, point, Quaternion.identity);
            }
        }
    }


    public void SpawnStepSFX(Transform pointTransform)
    {
        if (StepVFX)
        {
            if (ObjectPoolManager.instance)
            {
                ObjectPoolManager.Spawn(StepVFX, pointTransform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(StepVFX, pointTransform.position, Quaternion.identity);
            }
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
