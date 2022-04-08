using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public struct CamShakeSetting
{
    public float _duration;
    public float _magnitude;
    public float _timeSmoothIn;
    public float _timeSmoothOut;
    public float _frequency;

    public CamShakeSetting(float duration, float magnitude, float timeSmoothIn, float timeSmoothOut, float frequency)
    {
        _duration = duration;
        _magnitude = magnitude;
        _timeSmoothIn = timeSmoothIn;
        _timeSmoothOut = timeSmoothOut;
        _frequency = frequency;
    }
}

public class CamShake : MonoBehaviour
{

    private CinemachineVirtualCamera vCamera;//Virtual camera
    public CinemachineBrain brain;//Virtual camera
    //Time variables
    private float _timeLeft;
    private float _timeIn;
    private float _timeOut;


    //Shake intensity variables
    private float startIntensity;
    private float currentIntensity;
    private float maxIntensity;
    //Component of shake noise
    private CinemachineBasicMultiChannelPerlin shakeNoise;

    public static CamShake instance;
    public void Awake()
    {
        //Get cashe Refs
        vCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        startIntensity = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        shakeNoise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        instance = this;
    }


    public void Update()
    {
        //if shake time does not equal 0 shake should begin
        if(_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;

            if (currentIntensity < maxIntensity)//If current magnitude is greater than max intensity stop increasing
            {
                //If there is fade in time
                if (_timeIn > 0f)
                {
                    currentIntensity= Mathf.Lerp(startIntensity, maxIntensity, _timeIn);
                    shakeNoise.m_AmplitudeGain = currentIntensity;
                    _timeIn -= Time.deltaTime;

                }
                //If there is no fade in time
                else
                {
                    shakeNoise.m_AmplitudeGain = maxIntensity;
                }
            }


        }
        else
        {
            //If amplitude is not 0f
            if (shakeNoise.m_AmplitudeGain > 0)
            {
                //if there is fade out time
                if (_timeOut > 0f)
                {
                    //Stop shaking
                    shakeNoise.m_AmplitudeGain = Mathf.Lerp(currentIntensity, 0f, _timeOut); ;
                    _timeOut -= Time.deltaTime;

                }
                //if there is not fade out time
                else
                {
                    shakeNoise.m_AmplitudeGain = 0;
                }
            }

        }
    }
    //Set time variables which begins screenshake
    public void DoScreenShake(CamShakeSetting setting)
    {
        
        _timeLeft = setting._duration;
        maxIntensity = setting._magnitude;
        _timeIn = setting._timeSmoothIn;
        _timeOut = setting._timeSmoothOut;
        shakeNoise.m_FrequencyGain = setting._frequency;
    
    }
    
    public void SetShakeCam(CinemachineVirtualCamera cam)
    {
        vCamera = cam;
        startIntensity = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        shakeNoise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }
    public void DisableDefault()
    {
        //Get cashe Refs
        vCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        vCamera.enabled = false;
    }

    public void UseDefault()
    {
        //Get cashe Refs
        vCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        vCamera.enabled = true;
        startIntensity = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        shakeNoise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }
}
