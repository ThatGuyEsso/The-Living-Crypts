using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{

    private CinemachineVirtualCamera vCamera;//Virtual camera
    public CinemachineBrain brain;//Virtual camera
    //Time variables
    private float shakeTime;
    private float timeIn;
    private float timeOut;

    //Shake intensity variables
    private float startIntensity;
    private float currentIntensity;
    private float maxIntensity;
    //Component of shake noise
    private CinemachineBasicMultiChannelPerlin shakeNoise;

    public CinemachineConfiner bounds;
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
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            if (currentIntensity < maxIntensity)//If current magnitude is greater than max intensity stop increasing
            {
                //If there is fade in time
                if (timeIn > 0f)
                {
                    currentIntensity= Mathf.Lerp(startIntensity, maxIntensity, timeIn);
                    shakeNoise.m_AmplitudeGain = currentIntensity;
                    timeIn -= Time.deltaTime;

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
                if (timeOut > 0f)
                {
                    //Stop shaking
                    shakeNoise.m_AmplitudeGain = Mathf.Lerp(currentIntensity, 0f, timeOut); ;
                    timeOut -= Time.deltaTime;

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
    public void DoScreenShake(float duration, float magnitude,float timeSmoothIn,float timeSmoothOut,float frequency)
    {
        shakeTime = duration;
        maxIntensity = magnitude;
        timeIn = timeSmoothIn;
        timeOut = timeSmoothOut;
        shakeNoise.m_FrequencyGain = frequency;
        //Debug.Log("shake");
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
