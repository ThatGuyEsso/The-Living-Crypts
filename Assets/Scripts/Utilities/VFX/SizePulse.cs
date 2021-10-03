using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private Vector3 sizeMultiplier;
    [SerializeField] private bool isActive;
    [SerializeField] private float pulseRate;
    [SerializeField] private float peakWaitTime;
    [SerializeField] private Transform targetTransfrom;
    private Vector3 initialSize;
    bool isGrowing;
 
    bool canPulse;
    private float currPeakWait;
    private void Awake()
    {
        if (initialSize == Vector3.zero&&targetTransfrom) initialSize = targetTransfrom.localScale;
    }

    private void Update()
    {
        if (!isActive) return;
        if(canPulse)
        {
            if (isGrowing) InreaseSize();
            else DecreaseSize();
        }
        else{
            if (currPeakWait <= 0f)
            {
                isGrowing = !isGrowing;
                canPulse = true;
            }
            else
            {
                currPeakWait -= Time.deltaTime;
            }
        }
    }



    public void InreaseSize()
    {
        Vector3 targetSize = new Vector3(initialSize.x * sizeMultiplier.x, initialSize.y * sizeMultiplier.y, initialSize.z * sizeMultiplier.z);
        targetTransfrom.localScale = Vector3.Lerp(targetTransfrom.localScale,
           targetSize,Time.deltaTime * pulseRate);

        if(Vector3.Distance(targetTransfrom.localScale,targetSize)<= 0.01f)
        {
            targetTransfrom.localScale = targetSize;
            currPeakWait = peakWaitTime;
            canPulse = false;

        }
    }

    public void DecreaseSize()
    {
        targetTransfrom.localScale = Vector3.Lerp(targetTransfrom.localScale,
           initialSize, Time.deltaTime * pulseRate);

        if (Vector3.Distance(targetTransfrom.localScale, initialSize) <= 0.01f)
        {
            targetTransfrom.localScale = initialSize;
            currPeakWait = peakWaitTime;
            canPulse = false;

        }
    }


    public void Init(Transform targetTrans, Vector3 initSize)
    {
        isGrowing = true;
        initialSize = initSize;
        targetTransfrom = targetTrans;

    }

    public void StartPulse()
    {
        isActive = true;
    }
    public void StopPulse()
    {
        isActive = false;
    }

    public void StopAndReset()
    {
        isActive = false;
        targetTransfrom.localScale = initialSize;
    }
    public Vector3 GetInitSize() { return initialSize; }
    
}
