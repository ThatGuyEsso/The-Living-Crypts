using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSizeInRange : MonoBehaviour
{
    [SerializeField] private float MinSize, MaxSize;
    [SerializeField] private bool ResizeOnAwake;

    private void Awake()
    {
        if (ResizeOnAwake) SetRandomSize();
    }
    public void SetRandomSize()
    {
        transform.localScale = Vector3.one * Random.Range(MinSize, MaxSize);
    }

    public void SetRandomSize(float minSize, float maxSize,bool resize)
    {
        MinSize = minSize;
        MaxSize = maxSize;
        if(resize)
            transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
    }
}
